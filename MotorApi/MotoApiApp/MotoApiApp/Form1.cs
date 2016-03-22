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
        MotorSimulator m_motor;
        List<DishBuilder> m_dish = new List<DishBuilder>();
               
        public Form1()
        {

            InitializeComponent();

            try
            {
                Control.CheckForIllegalCrossThreadCalls = false;
                PhidgetMotor.MotorCallback p = new PhidgetMotor.MotorCallback(MotorFunctionCallback);
                m_motor = new MotorSimulator(p);
                m_motor.SetMotorLength(100);
                m_motor.SetAlphaConstant(60);

                label28.Text = "Connected";
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
            m_motor.Close();
            int count = 5;
            while (count-- > 0)
            {              
                Application.DoEvents();
                Thread.Sleep(400);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DishBuilder d = new DishBuilder("Potato");
                string script = "name:potato;move:40;wait:10;move:20;wait:1;move:40;wait:4;move:10";
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
    }
}
