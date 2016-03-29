using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhidgetMotorApi;
using Phidgets; //Needed for the MotorControl class, Phidget base classes, and the PhidgetException class
using Phidgets.Events;
using System.Threading;
using System.Diagnostics; //Needed for the Phidget event handling classes



namespace PhidgetMotorApi
{
    public class GojiMotor : PhidgetMotor
    {
        float m_lastPosition = 0;
        Stopwatch m_stopwatch = new Stopwatch();        
        protected double m_motorLengthInMili;

        public GojiMotor(MotorCallback p, int motorIndex = 0):base(p, motorIndex)
        {

        }
        public void SetMotorLength(int langthInMilimters)
        {
            m_motorLength = langthInMilimters;
            m_motorLengthInMili = (double)(m_motorLength / 1000.0);
        }

        protected void MotorLogic(float position)
        {
            if (m_runmotor == false)
            {
                motoControl.motors[m_motorIndex].Velocity = 0;
                m_stopwatch.Stop();
                return;
            }
            float x = Math.Abs(position - m_targetPosition);
            //int x1 = (int)(x * 100.0);
            //x = (float)(x1 / 100.0);
         
            if (position > m_motorLength)
            {
                motoControl.motors[m_motorIndex].Velocity = 0;
                m_stopwatch.Stop();
                m_runmotor = false;
                m_runScript = false;
                pCallback("Error", "Position bigger then motor length: " + position);
                return;
            }

            if (((int)m_lastPosition == (int)position) && m_stopwatch.IsRunning == false)
            {
                m_stopwatch.Restart();
            }
            if ((int)m_lastPosition != (int)position)
            {
                m_stopwatch.Stop();
            }
            m_lastPosition = position;

            if (m_stopwatch.ElapsedMilliseconds > 16000)
            {
                motoControl.motors[m_motorIndex].Velocity = 0;
                m_stopwatch.Stop();
                m_runmotor = false;
                m_runScript = false;
                pCallback("Error" ,"Stop dute timeout reacing end");
                return;
            }

            if (Math.Abs(x) <= m_precision)
            {
                motoControl.motors[m_motorIndex].Velocity = 0;
                m_runmotor = false;
                m_stopwatch.Stop();
                base.RechedPosition();
                pCallback("Motor Reached Position", m_targetPosition.ToString());
                return;
            }
            float toWrite  = 0;
            float d = (m_alphaConstant * ( m_targetPosition - position));
            float maxWrite;
            if (d > 100)
            {
                toWrite = 100;
            }
            else if (d < -100)
            {
                toWrite = -100;
            }
            else
            {
                toWrite = d;
            }
            if (toWrite > 0)
                maxWrite = 100;
            else if (toWrite < 0)
                maxWrite = -100;
            else
                maxWrite = 0;

            int cmto = 1;
            if (m_speedLevel > 2)
                cmto = 4;
            if (Math.Abs(x) > cmto)
                toWrite = toWrite / m_speedLevel;
            else
                toWrite = maxWrite;

            motoControl.motors[m_motorIndex].Velocity = toWrite;
        }

        protected override void motoControl_SensorUpdate(object sender, SensorUpdateEventArgs e)
        {
            if (m_motorIndex != e.Index)
                return;

            string position = (e.Value * m_motorLengthInMili).ToString();
            MotorLogic((float)(e.Value * m_motorLengthInMili));

            pCallback("motoControl_SensorUpdate", position.ToString());
        }      
    }
}
