using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhidgetMotorApi;
using Phidgets; //Needed for the MotorControl class, Phidget base classes, and the PhidgetException class
using Phidgets.Events;
using System.Threading; //Needed for the Phidget event handling classes



namespace PhidgetMotorApi
{
    public class GojiMotor : PhidgetMotor
    {
        
        protected int m_motorIndex = 0;
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
                return;
            float x = Math.Abs(position - m_targetPosition);
            //int x1 = (int)(x * 100.0);
            //x = (float)(x1 / 100.0);
            if (Math.Abs(x) <= 0.1)
            {
                motoControl.motors[m_motorIndex].Velocity = 0;
                m_runmotor = false;
                base.RechedPosition();
                pCallback("Motor Reached Position", m_targetPosition.ToString());
                return;
            }
            float toWrite  = 0;
            float d = (m_alphaConstant * ( m_targetPosition - position));
            if (d > 100)
                toWrite = 100;
            else if (d < -100)
                toWrite = -100;
            else
            {
                toWrite = d;
            }

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
