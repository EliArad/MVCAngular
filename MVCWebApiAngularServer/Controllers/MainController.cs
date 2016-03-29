using GojiBaseWebApiAngular.Models;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PhidgetMotorApi;
using SignalRChat;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace GojiBase.Controllers
{
    
    
    public partial class MainController : ApiController
    {

        static GojiMotor m_motor = null;
        static bool m_connected = false;

        Stopwatch m_sendWatch = new Stopwatch();
        public MainController()
        {

            if (m_motor == null)
            {

                try
                {
                    PhidgetMotor.MotorCallback p = new PhidgetMotor.MotorCallback(MotorFunctionCallback);
                    m_motor = new GojiMotor(p);
                    m_sendWatch.Restart();
                    using (var ctx = new PicardDb())
                    {
                        List<AppConfig> data = (from r in ctx.m_appConfig
                                                select r).ToList();

                        m_motor.SetMotorLength(data[0].MotorLength);
                        m_motor.MaxLength = data[0].MaxLength;
                        m_motor.MinLength = data[0].MinLength;
                        m_motor.SetPrecisionOnStop(0.25f);
                    }
                }
                catch (Exception err)
                {
                    Console.WriteLine(err.Message);
                }
            }
        }
        public static GojiMotor getMotorControl()
        {
            return m_motor;
        }
        [HttpGet]
        public string isauth()
        {
            return "ok";
        }

        [HttpGet]
        public string Index()
        {

            return "ok";
        }
        public class Person
        {
            public int PersonID { get; set; }
            public string Name { get; set; }
        }
        [HttpGet]
        public string GetSensorsSelection(string selection)
        {
            return "ok";    
        }


        [HttpGet]
        public string StopCooking()
        {
            if (m_motor != null)
            {
                m_motor.StopScript();
            }
            return "ok";    
        }


        [HttpGet]
        public string isMotorConnected()
        {
            return m_connected == true ? "Connected" : "Disconnected";
        }        
        
        [HttpPost]
        public IHttpActionResult RunDish(JObject  data)
        {

            //var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            //context.Clients.All.ShowMessage("eee");

            try
            {
                if (m_connected == false)
                {
                    return Ok("ok");
                }
                dynamic json = data;
                string dishname = json.Name;
                string dishscript = json.Script;

                int minutes = json.TimeToRun.Minutes;
                int seconds = json.TimeToRun.Seconds;


                DishBuilder dish = new DishBuilder(dishname);
                dish.SetScript(dishscript);

                //TimeSpan time = dish.getTotalTime();


                try
                {
                    File.Delete("c:\\log.txt");
                }
                catch (Exception err)
                {

                }
                
                TimeSpan time = new TimeSpan(0, minutes, seconds);
                string str = time.Minutes.ToString("00") + ":" + time.Seconds.ToString("00");

                var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
                context.Clients.All.UpdateClock(str);

                string astr = json.AlphaConstant;
                int a = int.Parse(astr);
                m_motor.SetAlphaConstant(a);
                m_motor.RunScript(dish, time, true);
                return Ok("ok");
            }
            catch (Exception err)
            {
                return Content(System.Net.HttpStatusCode.BadRequest, err.Message);
            }
        }

        protected void MotorFunctionCallback(string code, string msg)
        {

            Logger m_logger = Logger.getInstance();
            
            var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            try
            {
                switch (code)
                {
                    case "motoControl_Detach":
                        context.Clients.All.ShowMessage("motoControl_Detach");
                        m_logger.Write("code: " + code + "   msg:" + msg);
                        m_connected = false;
                    break;
                    case "motoControl_Attach":                      
                        context.Clients.All.ShowMessage("motoControl_Attach");
                        m_logger.Write("code: " + code + "   msg:" + msg);
                        m_connected = true;
                    break;
                    case "motoControl_SensorUpdate":
                    {
                        if (m_sendWatch.ElapsedMilliseconds > 1000)
                        {
                            //System.Diagnostics.Debug.WriteLine("update code:" + msg);
                            context.Clients.All.MotorUpdateValue(msg);
                            m_sendWatch.Restart();
                        }
                    }
                    break;
                    case "Motor Reached Position":
                        context.Clients.All.ShowMessage("Motor Reached Position");
                    break;
                    case "updateClock":
                        context.Clients.All.UpdateClock(msg);
                    break;
                    case "ScriptStarted":
                        context.Clients.All.ShowMessage("ScriptStarted");
                    break;                    
                    case "Finished":
                        m_logger.Write("code: " + code + "   msg:" + msg);
                        context.Clients.All.ShowMessage("Finished");
                    break;
                    case "Position error":
                        m_logger.Write("code: " + code + "   msg:" + msg);
                        context.Clients.All.ShowMessage("Position error");
                    break;
                    case "Error":
                         m_logger.Write("code: " + code + "   msg:" + msg);
                        context.Clients.All.ShowMessage("Error: " + msg);
                    break;
                }
            }
            catch (Exception err)
            {
                context.Clients.All.ShowMessage("Error in motor function callback " +  err.Message);
            }
        }
                 
        [HttpGet]
        public IHttpActionResult GetProduct(int id)
        {                         
            return Ok("ok");
        }

        [HttpGet]
        public bool IsRunning()
        {
            if (m_motor != null)
                return m_motor.IsRunning();
            else
            {
                return false;
            }
        }
    }
}
