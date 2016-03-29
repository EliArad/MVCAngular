using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhidgetMotorApi
{
    public enum MOTOR_CMD
    {
        MOVMENT,
        WAIT,
        LOOP_START,
        LOOP_END,
        SPEED
    }
    // example of script:
    // name: potato
    // move:40
    // wait: 1 seconds

    public class DishBuilder
    {

        bool m_isOk = false;
        string m_dishName;
        int m_curIndex = 0;
        List<Tuple<MOTOR_CMD, float>> m_motorCommands = new List<Tuple<MOTOR_CMD, float>>();
        TimeSpan m_totalTime;
        int m_listSize = 0;
        public DishBuilder(string name)
        {
            m_dishName = name;
        }
        public TimeSpan getTotalTime()
        {
            return m_totalTime;
        }
        private void AddCommand(MOTOR_CMD cmd, float data)
        {
            m_motorCommands.Add(new Tuple<MOTOR_CMD, float>(cmd, data));
        }
        public void Clear()
        {
            m_motorCommands.Clear();
            m_listSize = 0;
            m_isOk = false;
        }
        public Tuple<MOTOR_CMD, float> getNextCommand()
        {
            if (m_curIndex < m_listSize)
                return m_motorCommands[m_curIndex++];
            else
            {
                throw (new SystemException("Finished"));
            }
        }

        public Tuple<MOTOR_CMD, float> getCommand(int pc)
        {
            if (pc < m_listSize)
                return m_motorCommands[pc];
            else
            {
                throw (new SystemException("Finished"));
            }
        }

        public bool isOk()
        {
            return m_isOk;
        }
        public void RestartScript()
        {
            m_curIndex = 0;
        }
        public void SetScript(string script)
        {
            int loopCount = 1;
            int [] k = new int[3];
            try
            {
                TimeSpan m_loopTime = new TimeSpan(0, 0, 0);
                string[] lines = script.Split(';');
                bool loopDetected = false;
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] s = lines[i].Split(':');
                    string c = s[0].ToLower().Trim();
                    if (c == "name")
                    {
                        m_dishName = s[1];
                        k[0] = 1;
                    }
                    else if (c == "loop")
                    {
                        loopCount = int.Parse(s[1]);
                        m_motorCommands.Add(new Tuple<MOTOR_CMD, float>(MOTOR_CMD.LOOP_START, float.Parse(s[1])));
                        m_loopTime = new TimeSpan(0, 0, 0);
                        loopDetected = true;
                    }
                    else if (c == "loopend")
                    {
                        m_motorCommands.Add(new Tuple<MOTOR_CMD, float>(MOTOR_CMD.LOOP_END, 0));
                        TimeSpan t = m_loopTime;
                        for (int j = 0 ; j < loopCount - 1 ; j++)
                        {
                            m_loopTime = m_loopTime.Add(t);
                        }
                        m_totalTime = m_totalTime + m_loopTime;
                        loopDetected = false;
                    }
                    else if (c == "move")
                    {
                        m_motorCommands.Add(new Tuple<MOTOR_CMD, float>(MOTOR_CMD.MOVMENT, float.Parse(s[1])));
                        k[1] = 1;
                    }
                    else if (c == "speed")
                    {
                        m_motorCommands.Add(new Tuple<MOTOR_CMD, float>(MOTOR_CMD.SPEED, float.Parse(s[1])));
                    }
                    else if (c == "wait")
                    {
                        m_motorCommands.Add(new Tuple<MOTOR_CMD, float>(MOTOR_CMD.WAIT, float.Parse(s[1])));
                        k[2] = 1;
                        TimeSpan t = new TimeSpan(0, 0, int.Parse(s[1]));
                        if (loopDetected == false)
                        {
                            m_totalTime = m_totalTime.Add(t);
                        }
                        else
                        {
                            m_loopTime = m_loopTime.Add(t);
                        }
                    }
                }
                m_listSize = m_motorCommands.Count;
                int check = k[0] + k[1] + k[2];
                if (check >= 3)
                    m_isOk = true;
                m_curIndex = 0;
            }
            catch (Exception err)
            {
                throw (new SystemException(err.Message));
            }
        }
    }
}
