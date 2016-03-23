using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PhidgetMotorApi;
using System.Threading;


namespace MotoApiApp
{
    public partial class Form1 : Form
    {
        GojiMotor m_motor = null;
        List<DishBuilder> m_dish = new List<DishBuilder>();
        bool m_initialized = false;
        public Form1()
        {

            InitializeComponent();
            textBox3.Text = Properties.Settings.Default.MotorLength;
            textBox4.Text = Properties.Settings.Default.Script;
            textBox5.Text = Properties.Settings.Default.MaxLength;
            textBox6.Text = Properties.Settings.Default.MinLength;

            if (textBox3.Text == "")
            {
                textBox3.Text = "100";
                textBox5.Text = "100";
            }
            else
            {
                textBox5.Text = textBox3.Text;
            }                        

            if (textBox6.Text == "")
            {
                textBox6.Text = "0";
            }

                
        }


        void Initialize()
        {
            try
            {
                Control.CheckForIllegalCrossThreadCalls = false;
                PhidgetMotor.MotorCallback p = new PhidgetMotor.MotorCallback(MotorFunctionCallback);
                m_motor = new GojiMotor(p);
                m_motor.SetMotorLength(int.Parse(textBox3.Text));
                m_motor.SetAlphaConstant(60);

                label28.Text = "Connected";
                m_initialized = true;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }       
        }

        protected void MotorFunctionCallback(string code, string msg)
        {
            
            try
            {
                switch (code)
                {
                    case "motoControl_Detach":
                        label28.Text = "Disconnected";
                        break;
                    case "motoControl_Attach":
                        label28.Text = "Attached";
                        break;
                    case "motoControl_SensorUpdate":
                        //Console.WriteLine("update code:"  + msg);
                        textBox2.Text = msg;
                    break;
                    case "Motor Reached Position":
                        label1.Text = "Motor reached position";
                    break;
                    case "updateClock":
                        lblTime.Text = msg;
                    break;
                    case "Finished":
                        MessageBox.Show("Dish ready");
                    break;
                    case "Position error":
                        MessageBox.Show("Position error: " + msg);
                    break;
                }
            }
            catch (Exception err)
            {

            }
        }
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_initialized == false)
                {
                    Initialize();
                }
                label1.Text = "Motor is in progress";
                m_motor.SetNewPosition(int.Parse(textBox1.Text));
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_motor != null)
                m_motor.Close();
            int count = 5;
            while (count-- > 0)
            {              
                Application.DoEvents();
                Thread.Sleep(400);
            }

            Properties.Settings.Default.MotorLength = textBox3.Text;
            Properties.Settings.Default.Script = textBox4.Text;

            Properties.Settings.Default.MaxLength = textBox5.Text;
            Properties.Settings.Default.MinLength = textBox6.Text;
            Properties.Settings.Default.Save();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox4.Text == "")
                {
                    MessageBox.Show("Script is empty");
                    return;
                }

                if (m_initialized == false)
                {
                    Initialize();
                }

                DishBuilder d = new DishBuilder("Potato");
                //string script = "name:potato;loop:2;move:40;wait:2;move:20;wait:1;move:40;wait:2;move:10;loopend;loop:2;move:40;wait:2;move:20;wait:1;move:40;wait:2;move:10;loopend";
                //string script = "name:potato;move:40;wait:2;move:20;wait:1;move:40;wait:2;move:10;move:40;wait:2;move:20;wait:1;move:40;wait:2;move:10";
                //string script = "name:potato;loop:2;move:40;wait:2;move:20;wait:1;move:40;wait:2;move:10;move:40;wait:2;move:20;wait:1;move:40;wait:2;move:10;loopend";
                string script = textBox4.Text;
                d.SetScript(script);
                m_dish.Add(d);
                TimeSpan time = d.getTotalTime();
                string str = time.Minutes + ":" + time.Seconds;
                lblTime.Text = str;

                m_motor.RunScript(d, true);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (m_initialized == false)
            {
                Initialize();
            }
            if (m_motor != null)
            {
                m_motor.SetMotorLength(int.Parse(textBox3.Text));
                textBox5.Text = textBox3.Text;
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            if (m_initialized == false)
            {
                Initialize();
            }
            if (m_motor != null)
            {
                try
                {
                    m_motor.MaxLength = int.Parse(textBox5.Text);
                }
                catch (Exception err)
                {

                }
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (m_initialized == false)
            {
                Initialize();
            }
            if (m_motor != null)
            {
                try
                {
                    m_motor.MinLength = int.Parse(textBox6.Text);
                }
                catch (Exception err)
                {

                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (m_motor != null)
            {
                m_motor.StopScript();
            }
        }
    }
}
