using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phidgets; //Needed for the MotorControl class, Phidget base classes, and the PhidgetException class
using Phidgets.Events;
using System.Threading; //Needed for the Phidget event handling classes



namespace PhidgetMotorApi
{

    public class PhidgetMotor
    {
        ManualResetEvent m_suspendClock = new ManualResetEvent(false);
        Thread m_process;
        bool m_runScript = false;
        AutoResetEvent m_timerEvent = new AutoResetEvent(false);
        protected bool m_runmotor = false;
        public delegate void MotorCallback(string code, string msg);
        protected MotorControl motoControl; //Declare a MotorControl object
        public int NewMaxVelocity;
        public int NewMaxLength;
        public int NewMinLength;
        protected float  m_targetPosition;
        protected int m_velocityToTarget = 50;
        protected float m_alphaConstant = 2;
        ManualResetEvent m_event = new ManualResetEvent(false);
        Thread m_clockThread = null;
        protected MotorCallback pCallback = null;

        public PhidgetMotor(MotorCallback p, int motorIndex = 0)
        {
            try
            {
                pCallback = p;
                motoControl = new MotorControl();

                motoControl.Attach += new AttachEventHandler(motoControl_Attach);
                motoControl.Detach += new DetachEventHandler(motoControl_Detach);
                motoControl.Error += new ErrorEventHandler(motoControl_Error);
                motoControl.CurrentChange += new CurrentChangeEventHandler(motoControl_CurrentChange);
                motoControl.InputChange += new InputChangeEventHandler(motoControl_InputChange);
                motoControl.VelocityChange += new VelocityChangeEventHandler(motoControl_VelocityChange);
                motoControl.BackEMFUpdate += new BackEMFUpdateEventHandler(motoControl_BackEMFUpdate);
                motoControl.EncoderPositionChange += new EncoderPositionChangeEventHandler(motoControl_EncoderPositionChange);
                motoControl.SensorUpdate += new SensorUpdateEventHandler(motoControl_SensorUpdate);


                NewMaxLength = 1000;
                NewMinLength = 0;

                openCmdLine(motoControl);

            }
            catch(Exception err)
            {
                throw (new SystemException(err.Message));
            }
        }
        private void openCmdLine(Phidget p)
        {
            openCmdLine(p, null);
        }
        public void SetAlphaConstant(int c)
        {
            m_alphaConstant = c;
        }
        public bool IsRunning()
        {
            return m_runScript;
        }
        void DishProcess(DishBuilder dishList)
        {
            m_runScript = true;
            pCallback("ScriptStarted", "");
            while (m_runScript)
            {
                try
                {
                    Tuple<MOTOR_CMD, float> n = dishList.getNextCommand();
                    switch (n.Item1)
                    {
                        case MOTOR_CMD.MOVMENT:
                            SetNewPosition((int)n.Item2);
                            m_event.Reset();
                            m_event.WaitOne();
                        break;
                        case MOTOR_CMD.WAIT:
                            m_suspendClock.Set();
                            Thread.Sleep((int)n.Item2 * 1000);
                            m_suspendClock.Reset();
                        break;
                    }
                }
                catch (Exception err)
                {
                    if (err.Message == "Finished")
                    {
                        m_runScript = false;
                        m_suspendClock.Set();
                        pCallback("Finished", "");
                        return;
                    }
                }
            }
        }
        public void RunScript(DishBuilder dish, bool broadcastClock)
        {
            if (!dish.isOk())
            {
                throw (new SystemException("Script is not valid"));
            }
            m_suspendClock.Reset();
            m_process = new Thread(() => DishProcess(dish));
            m_process.Start();

            m_clockThread = new Thread(() => UpdateClock(dish.getTotalTime()));
            m_clockThread.Start();
                 
        }

        private void UpdateClock(TimeSpan time)
        {
            TimeSpan tsub = new TimeSpan(0 , 0 , 1);
            string str;
            while (m_runScript) 
            {
                m_suspendClock.WaitOne();
                if (m_runScript == false)
                    return;
                m_timerEvent.WaitOne(1000);
                if (m_runScript == false)
                    return;
                time = time.Subtract(tsub);
                str = time.Minutes + ":" + time.Seconds;
                pCallback("updateClock", str);
            }
        }
        public void Close()
        {
            var t = new Thread(_close);
            t.Start();
        }

        private void _close()
        {
            try
            {
                m_runmotor = false;
                motoControl.Attach -= motoControl_Attach;
                motoControl.Detach -= motoControl_Detach;
                motoControl.Error -= motoControl_Error;
                motoControl.InputChange -= motoControl_InputChange;
                motoControl.CurrentChange -= motoControl_CurrentChange;
                motoControl.VelocityChange -= motoControl_VelocityChange;
                motoControl.BackEMFUpdate -= motoControl_BackEMFUpdate;
                motoControl.EncoderPositionChange -= motoControl_EncoderPositionChange;
                motoControl.SensorUpdate -= motoControl_SensorUpdate;

                if (motoControl.Attached)
                {
                    foreach (MotorControlMotor motor in motoControl.motors)
                    {
                        motor.Velocity = 0;
                    }
                }
                Thread.Sleep(0);
                //run any events in the message queue - otherwise close will hang if there are any outstanding events
                motoControl.close();
                motoControl = null;
            }
            catch (Exception err)
            {
                throw (new SystemException(err.Message));
            }
        }
        private void openCmdLine(Phidget p, String pass)
        {
            int serial = -1;
            String logFile = null;
            int port = 5001;
            String host = null;
            bool remote = false, remoteIP = false;
            
            string[] args = Environment.GetCommandLineArgs();
            String appName = args[0];

            try
            { 
                //if (logFile != null)
                  //  Phidget.enableLogging(Phidget.LogLevel.PHIDGET_LOG_INFO, logFile);
                if (remoteIP)
                    p.open(serial, host, port, pass);
                else if (remote)
                    p.open(serial, host, pass);
                else
                    p.open(serial);
                return; //success
            }
            catch 
            {
                throw (new SystemException("error"));
            }
            
        }

        void motoControl_Attach(object sender, AttachEventArgs e)
        { 
            MotorControl attached = (MotorControl)sender;
            pCallback("motoControl_Attach" , "");
        }
        void motoControl_Detach(object sender, DetachEventArgs e)
        {
            if (pCallback != null)
                pCallback("motoControl_Detach", "");
        }
        void motoControl_Error(object sender, ErrorEventArgs e)
        {

        }
        void motoControl_CurrentChange(object sender, CurrentChangeEventArgs e)
        {

        }
        void motoControl_VelocityChange(object sender, VelocityChangeEventArgs e)
        {
        }
        void motoControl_BackEMFUpdate(object sender, BackEMFUpdateEventArgs e)
        {

        }
        void motoControl_InputChange(object sender, InputChangeEventArgs e)
        {

        }
        protected virtual void motoControl_SensorUpdate(object sender, SensorUpdateEventArgs e)
        {
            if (m_runmotor == false)
                return;
            string position = e.Value.ToString();

            //pCallback("motoControl_SensorUpdate", position);
        }

        protected void RechedPosition()
        {
            m_event.Set();
        }

        void motoControl_EncoderPositionChange(object sender, EncoderPositionChangeEventArgs e)
        {

        }

        public void BuildDishMovment()
        {
           
        }

        public void SetNewPosition(int newPosition)
        {
            if ((newPosition > NewMaxLength) || (newPosition < NewMinLength))
            {
                throw new SystemException("Please choose position between " + NewMinLength.ToString() + "[mm] to " + NewMaxLength.ToString() + "[mm]");
            }
            m_targetPosition = (float)newPosition;
            m_runmotor = true;
        }
    }
}
