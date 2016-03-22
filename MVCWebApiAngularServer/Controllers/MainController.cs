using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PhidgetMotorApi;
using SignalRChat;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace GojiBase.Controllers
{
    
    
    public partial class MainController : ApiController
    {

        static MotorSimulator m_motor = null;

        public MainController()
        {

            if (m_motor == null)
            {
                PhidgetMotor.MotorCallback p = new PhidgetMotor.MotorCallback(MotorFunctionCallback);
                m_motor = new MotorSimulator(p);
                m_motor.SetMotorLength(100);
                m_motor.SetAlphaConstant(60);
            }


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
        
        [HttpPost]
        public IHttpActionResult RunDish(JObject  data)
        {

            //var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            //context.Clients.All.ShowMessage("eee");

            try
            {
                dynamic json = data;
                string dishname = json.Name;
                string dishscript = json.Script;

                DishBuilder dish = new DishBuilder(dishname);
                dish.SetScript(dishscript);

                TimeSpan time = dish.getTotalTime();
                string str = time.Minutes + ":" + time.Seconds;

                var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
                context.Clients.All.UpdateClock(str);

                m_motor.RunScript(dish, true);
                return Ok("ok");
            }
            catch (Exception err)
            {
                return Content(System.Net.HttpStatusCode.BadRequest, err.Message);
            }
        }

        protected void MotorFunctionCallback(string code, string msg)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            try
            {
                switch (code)
                {
                    case "motoControl_Detach":
                        context.Clients.All.ShowMessage("motoControl_Detach");
                    break;
                    case "motoControl_Attach":                      
                        context.Clients.All.ShowMessage("motoControl_Attach");
                    break;
                    case "motoControl_SensorUpdate":
                        //Console.WriteLine("update code:"  + msg);                        
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
                        context.Clients.All.ShowMessage("Finished");
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
            return m_motor.IsRunning();
        }
    }
}
