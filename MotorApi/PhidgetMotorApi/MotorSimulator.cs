using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phidgets; //Needed for the MotorControl class, Phidget base classes, and the PhidgetException class
using Phidgets.Events;

namespace PhidgetMotorApi
{
    public class MotorSimulator : GojiMotor
    {
        float m_position = 0;
        public MotorSimulator(MotorCallback p, int motorIndex = 0)
            : base(p, motorIndex)
        {

        }

        protected override void motoControl_SensorUpdate(object sender, SensorUpdateEventArgs e)
        {
            if (m_motorIndex != e.Index)
                return;

            if (m_position < m_targetPosition)
                m_position += 0.1f;
            else if (m_position > m_targetPosition)
            {
                m_position -= 0.1f;
            }
            MotorLogic(m_position);

            pCallback("motoControl_SensorUpdate", m_position.ToString());
        }      

    }
}
