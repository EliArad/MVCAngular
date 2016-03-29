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
using System.IO;


namespace MotoApiApp
{

    public enum CLOCK_MODE
    {
        STEP_CLOCK,
        OVERAL_CLOCK
    }

    public partial class Form1 : Form
    {
        CLOCK_MODE m_clockMode = CLOCK_MODE.OVERAL_CLOCK;
        GojiMotor m_motor = null;
        List<DishBuilder> m_dish = new List<DishBuilder>();
        bool m_initialized = false;
        bool m_connected = false;
        public Form1()
        {

            InitializeComponent();
            textBox3.Text = Properties.Settings.Default.MotorLength;
            textBox4.Text = Properties.Settings.Default.Script;
            textBox5.Text = Properties.Settings.Default.MaxLength;
            textBox6.Text = Properties.Settings.Default.MinLength;
            textBox8.Text = Properties.Settings.Default.precision;

            txtSec.Text = Properties.Settings.Default.seconds;
            txtMin.Text = Properties.Settings.Default.minutes;
            comboBox1.Text = Properties.Settings.Default.speedLevel.ToString();

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

            textBox7.Text = Properties.Settings.Default.AlphaConstant;
        }

        void Initialize()
        {
            lock (this)
            {
                try
                {
                    Control.CheckForIllegalCrossThreadCalls = false;
                    PhidgetMotor.MotorCallback p = new PhidgetMotor.MotorCallback(MotorFunctionCallback);
                    m_motor = new GojiMotor(p);
                    m_motor.SetMotorLength(int.Parse(textBox3.Text));
                    m_motor.SetAlphaConstant(60);
                    m_initialized = true;
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                }
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
                        m_connected = false;
                        break;
                    case "motoControl_Attach":
                        {
                            label28.Text = "Connected";
                            Logger m_logger = Logger.getInstance();
                            m_logger.Write("code: " + code + "   msg:" + msg);
                            m_connected = true;
                        }
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
                    {
                        Logger m_logger = Logger.getInstance();
                        m_logger.Write("code: " + code + "   msg:" + msg);
                        MessageBox.Show("Dish ready");
                    }
                    break;
                    case "Position error":
                    {
                        Logger m_logger = Logger.getInstance();
                        m_logger.Write("code: " + code + "   msg:" + msg);
                        MessageBox.Show("Position error: " + msg);
                    }
                    break;
                    case "Error":
                    {
                        Logger m_logger = Logger.getInstance();
                        m_logger.Write("code: " + code + "   msg:" + msg);
                        MessageBox.Show("Error: " + msg);
                        break;
                    }
                    case "Motor In Progress":
                        label1.Text = "Motor In Progress";
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
                if (m_connected == false)
                {
                    MessageBox.Show("Motor is not connected");
                    return;
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
            saveSettings();
        }
        void saveSettings()
        {
            Properties.Settings.Default.MotorLength = textBox3.Text;
            Properties.Settings.Default.Script = textBox4.Text;

            Properties.Settings.Default.MaxLength = textBox5.Text;
            Properties.Settings.Default.MinLength = textBox6.Text;

            Properties.Settings.Default.seconds = txtSec.Text;
            Properties.Settings.Default.minutes = txtMin.Text;
            Properties.Settings.Default.AlphaConstant = textBox7.Text;
            Properties.Settings.Default.speedLevel = int.Parse(comboBox1.Text);
            Properties.Settings.Default.precision = textBox8.Text;
            Properties.Settings.Default.Save();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_connected == false)
                {
                    MessageBox.Show("Motor is not connected");
                    return;
                }
                saveSettings();
                if (textBox4.Text == "")
                {
                    MessageBox.Show("Script is empty");
                    return;
                }

                if (m_initialized == false)
                {
                    Initialize();
                }
                if (m_motor.IsRunning() == true)
                {
                    MessageBox.Show("Already running");
                    return;
                }

                try
                {
                    File.Delete("c:\\log.txt");
                }
                catch (Exception err)
                {

                }

                try
                {
                    DishBuilder d = new DishBuilder("Potato");
                    //string script = "name:potato;loop:2;move:40;wait:2;move:20;wait:1;move:40;wait:2;move:10;loopend;loop:2;move:40;wait:2;move:20;wait:1;move:40;wait:2;move:10;loopend";
                    //string script = "name:potato;move:40;wait:2;move:20;wait:1;move:40;wait:2;move:10;move:40;wait:2;move:20;wait:1;move:40;wait:2;move:10";
                    //string script = "name:potato;loop:2;move:40;wait:2;move:20;wait:1;move:40;wait:2;move:10;move:40;wait:2;move:20;wait:1;move:40;wait:2;move:10;loopend";
                    string script = textBox4.Text;
                    d.SetScript(script);
                    m_dish.Add(d);

                    int miniute = int.Parse(txtMin.Text);
                    int seconds = int.Parse(txtSec.Text);

                    TimeSpan time1 = new TimeSpan(0, miniute, seconds);
                    if (m_clockMode == CLOCK_MODE.OVERAL_CLOCK)
                    {
                        string str = time1.Minutes.ToString("00") + ":" + time1.Seconds.ToString("00");
                        lblTime.Text = str;
                        m_motor.RunScript(d, time1, true);
                    }
                    else
                    {
                        TimeSpan time = d.getTotalTime();
                        string str = time.Minutes.ToString("00") + ":" + time.Seconds.ToString("00");
                        lblTime.Text = str;
                        m_motor.RunScript(d, true);
                    }
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                }
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

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            if (m_initialized == false)
            {
                Initialize();
            }
            if (m_motor != null)
            {
                try
                {
                    double a = int.Parse(textBox7.Text);
                    m_motor.SetAlphaConstant((int)a);
                }
                catch (Exception err)
                {

                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (m_initialized == false)
            {
                Initialize();
            }
            if (m_motor != null)
            {
                try
                {
                    int speed = int.Parse(comboBox1.Text);
                    m_motor.SetSpeedLevel(speed);
                }
                catch (Exception err)
                {

                }
            }
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            if (m_initialized == false)
            {
                Initialize();
            }
            if (m_motor != null)
            {
                try
                {
                    float precision = float.Parse(textBox8.Text);
                    m_motor.SetPrecisionOnStop(precision);
                }
                catch (Exception err)
                {

                }
            }
        }
    }
}
