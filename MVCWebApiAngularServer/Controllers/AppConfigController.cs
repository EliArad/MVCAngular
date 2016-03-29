using GojiBase.Controllers;
using GojiBaseWebApiAngular.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace GojiBaseWebApiAngular.Controllers
{
    public class AppConfigController : ApiController
    {
        public string GetAppConfig()
        {
            using (var ctx = new PicardDb())
            {
                IEnumerable<AppConfig> data = from r in ctx.m_appConfig
                                                           orderby r.Id ascending
                                                           select r;

                JavaScriptSerializer jss = new JavaScriptSerializer();
                string json = jss.Serialize(data);
                return json;
            }
        }

        [HttpPost]
        public string SetMotorLength(int length)
        {
            if (length < 1 || length > 99)
            {
                return GetAppConfig();
            }

            AppConfig d = new AppConfig();
            d.MotorLength = length; 
         
            using (var ctx = new PicardDb())
            {
                ctx.Entry(d).State = EntityState.Modified;
                ctx.SaveChanges();

                MainController.getMotorControl().SetMotorLength(length);

                return GetAppConfig();
            }
        }

        [HttpPost]
        public string SaveAppConfig(JObject data)
        {

            //var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            //context.Clients.All.ShowMessage("eee");

            try
            {
                int id = 1;
                using (var ctx = new PicardDb())
                {
                    var query = (from r in ctx.m_appConfig
                                 select r).First();

                    id = (int)query.Id;
                }

                dynamic json = data;
                using (var ctx = new PicardDb())
                {
                      
                    AppConfig a = new AppConfig();
                    a.Id = id;
                    a.MotorLength = json.MotorLength;
                    a.MaxLength = json.MaxLength;
                    a.MinLength = json.MinLength;
                    ctx.Entry(a).State = EntityState.Modified;
                    ctx.SaveChanges();
                    MainController.getMotorControl().SetMotorLength(a.MotorLength);
                    MainController.getMotorControl().MaxLength = a.MaxLength;
                    MainController.getMotorControl().MinLength = a.MinLength;
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);                
            }
            return GetAppConfig();
        }
    }
}
