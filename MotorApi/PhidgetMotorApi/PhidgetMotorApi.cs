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
        AutoResetEvent m_timerCounter = new AutoResetEvent(false);
        protected float m_precision = 0.15f;
        protected int m_speedLevel = 1;
        protected int m_motorIndex = 0;
        protected int m_motorLength = 0;
        ManualResetEvent m_suspendClock = new ManualResetEvent(false);
        Thread m_process;
        protected bool m_runScript = false;
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
                m_motorIndex = motorIndex;
                motoControl.Attach += new AttachEventHandler(motoControl_Attach);
                motoControl.Detach += new DetachEventHandler(motoControl_Detach);
                motoControl.Error += new ErrorEventHandler(motoControl_Error);
                motoControl.CurrentChange += new CurrentChangeEventHandler(motoControl_CurrentChange);
                motoControl.InputChange += new InputChangeEventHandler(motoControl_InputChange);
                motoControl.VelocityChange += new VelocityChangeEventHandler(motoControl_VelocityChange);
                motoControl.BackEMFUpdate += new BackEMFUpdateEventHandler(motoControl_BackEMFUpdate);
                motoControl.EncoderPositionChange += new EncoderPositionChangeEventHandler(motoControl_EncoderPositionChange);
                motoControl.SensorUpdate += new SensorUpdateEventHandler(motoControl_SensorUpdate);


                NewMaxLength = 0;
                NewMinLength = 0;

                openCmdLine(motoControl);

            }
            catch(Exception err)
            {
                throw (new SystemException(err.Message));
            }
        }

        public int MaxLength
        {
            set
            {
                NewMaxLength = value;                
            }
            get
            {
                return NewMaxLength;
            }
        }

        public int MinLength
        {
            set
            {
                NewMinLength = value;
            }
            get
            {
                return NewMinLength;
            }
        }

        public void StopScript()
        {
            m_runScript = false;
            m_runmotor = false;
            m_timerCounter.Set();

            motoControl.motors[m_motorIndex].Velocity = 0;

            m_event.Set();
            if (m_process != null)
                m_process.Join();

            m_timerEvent.Set();
            if (m_clockThread != null)
                m_clockThread.Join();
        }

        private void openCmdLine(Phidget p)
        {
            openCmdLine(p, null);
        }
        public void SetPrecisionOnStop(float precision)
        {
            m_precision = precision;
        }
        public void SetSpeedLevel(int speed)
        {
            if (speed < 1)
                return;
            m_speedLevel = speed;
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
          
            pCallback("ScriptStarted", "");
            int jumploop = -1;
            int loopStartAddress = -1;
            int pc = 0;
            while (m_runScript)
            {
                try
                {
                    Tuple<MOTOR_CMD, float> n = dishList.getCommand(pc);
                    switch (n.Item1)
                    {
                        case MOTOR_CMD.MOVMENT:
                            if ((int)n.Item2 > m_motorLength)
                            {
                                m_runScript = false;
                                m_suspendClock.Set();
                                string s = (int)n.Item2 + "  " + m_motorLength;
                                pCallback("Position error", s);
                                return;
                            } else 
                            if ((int)n.Item2 > MaxLength)
                            {
                                m_runScript = false;
                                m_suspendClock.Set();
                                string s = (int)n.Item2 + "  " + MaxLength;
                                pCallback("Position error", s);
                                return;
                            }else
                            if ((int)n.Item2 < MinLength)
                            {
                                m_runScript = false;
                                m_suspendClock.Set();
                                string s = (int)n.Item2 + "  " + MinLength;
                                pCallback("Position error", s);
                                return;
                            }
                            SetNewPosition((int)n.Item2);
                            m_event.Reset();
                            m_event.WaitOne();
                        break;
                        case MOTOR_CMD.SPEED:
                            m_speedLevel = (int)n.Item2;
                        break;
                        case MOTOR_CMD.WAIT:
                            m_suspendClock.Set();                            
                            m_timerCounter.WaitOne((int)n.Item2 * 1000);
                            m_suspendClock.Reset();
                        break;
                        case MOTOR_CMD.LOOP_START:
                            jumploop = (int)n.Item2;
                            loopStartAddress = pc + 1;
                        break;
                        case MOTOR_CMD.LOOP_END:
                            jumploop--;
                            if (jumploop > 0)
                            {
                                pc = loopStartAddress;
                                continue;
                            }
                        break;
                    }
                    pc++;
                }
                catch (Exception err)
                {
                    if (err.Message == "Finished")
                    {
                        m_runScript = false;
                        m_runmotor = false;
                        m_suspendClock.Set();
                        pCallback("Finished", "");
                        return;
                    }
                }
            }
        }
        public void RunScript(DishBuilder dish, bool broadcastClock)
        {
            if (m_runScript == true)
            {
                throw (new SystemException("Already running"));
            }

            if (!dish.isOk())
            {
                throw (new SystemException("Script is not valid"));
            }
            m_suspendClock.Reset();
            m_runScript = true;
            m_process = new Thread(() => DishProcess(dish));
            m_process.Start();

        
            m_clockThread = new Thread(() => UpdateClock(dish.getTotalTime()));        
            m_clockThread.Start();
                 
        }

        public void RunScript(DishBuilder dish, TimeSpan time, bool broadcastClock)
        {
            if (!dish.isOk())
            {
                throw (new SystemException("Script is not valid"));
            }
            m_runScript = true;
            m_suspendClock.Reset();
            m_process = new Thread(() => DishProcess(dish));
            m_process.Start();


            m_clockThread = new Thread(() => BroadcastClock(time));
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
                str = time.Minutes.ToString("00") + ":" + time.Seconds.ToString("00");
                pCallback("updateClock", str);
            }
        }

        private void BroadcastClock(TimeSpan time)
        {
            int totalSeconds = (int)time.TotalSeconds;
            string str;
            TimeSpan tsub = new TimeSpan(0, 0, 1);
            while (m_runScript && totalSeconds > 0)
            {
                m_timerEvent.WaitOne(1000);
                if (m_runScript == false)
                    return;
                time = time.Subtract(tsub);
                str = time.Minutes.ToString("00") + ":" + time.Seconds.ToString("00");
                pCallback("updateClock", str);
                totalSeconds--;
            }
            m_runScript = false;
            m_runmotor = false;
            m_suspendClock.Set();
            pCallback("Finished", "");
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
            if (newPosition > m_motorLength)
            {
                throw new SystemException("Please choose position between between motor length in [mm]");
            }
            m_targetPosition = (float)newPosition;
            pCallback("Motor In Progress", m_targetPosition.ToString());
            m_runmotor = true;

        }
    }
}
