using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhidgetMotorApi
{
    public class Logger
    {
        private Logger()
        {

        }
        static Logger m_instance = null;
        public static Logger getInstance()
        {
            if (m_instance == null)
            {
                m_instance = new Logger();
                return m_instance;
            }
            else
            {
                return m_instance;
            }
        }
        public void Write(string str)
        {
            //FileStream f = new FileStream("log.txt" , FileAccess.ReadWrite,)
          using (StreamWriter w = File.AppendText("c:\\log.txt"))
          {
              w.WriteLine(str);
          }
        }
    }
}
