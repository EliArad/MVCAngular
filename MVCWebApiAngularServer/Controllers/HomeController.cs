using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using SignalRChat;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace GojiBase.Controllers
{
    
    
    public partial class HomeController : Controller
    {

        public HomeController()
        {
                   
        }
         
        public ActionResult Index()
        {
            try
            {

                if (User.Identity.IsAuthenticated == false)
                {
                   
                }
                return View();
            }
            catch (Exception err)
            {
                return View();
            }
        }
    }
}
