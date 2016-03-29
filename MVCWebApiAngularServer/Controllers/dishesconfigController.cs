using GojiBaseWebApiAngular.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace GojiBaseWebApiAngular.Controllers
{

    public class dishesconfigController : ApiController
    {
        static int m_id = -1;
        
        public string getAllDishes()
        {
            using (var ctx = new PicardDb())
            {
                IEnumerable<Dishes> data = from r in ctx.m_dishes
                                                           orderby r.Id ascending
                                                           select r;

                JavaScriptSerializer jss = new JavaScriptSerializer();
                string json = jss.Serialize(data);
                return json;
            }
        }
         
        [HttpPost]
        public string UpdateDish(JObject data)
        {
            try
            {
                Dishes d = new Dishes();
                dynamic json1 = data;
                d.Name = json1.Name;
                d.Id = json1.Id;
                d.ImageSrc = json1.ImageSrc;
                d.Script = json1.Script;
                d.AlphaConstant = json1.AlphaConstant;

                string time = json1.TimeToRun;

                string[] s = time.Split(':');
                d.TimeToRun = new TimeSpan(0, int.Parse(s[0]), int.Parse(s[1]));

                using (var ctx = new PicardDb())
                {
                    ctx.Entry(d).State = EntityState.Modified;
                    ctx.SaveChanges();

                    return getAllDishes();
                }
            }
            catch (Exception err)
            {
                return getAllDishes();
            }
        }
        [HttpGet]
        public string SetDishImageId(int id)
        {
            m_id = id;
            return "ok";
        }

        [HttpPost]
        public HttpResponseMessage SaveFiles()
        {
            
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    var filePath = HttpContext.Current.Server.MapPath("~/Images/" + postedFile.FileName);
                    postedFile.SaveAs(filePath);
                
                    if (m_id != -1)
                    {
                        using (var ctx = new PicardDb())
                        {
                            Dishes d = ctx.m_dishes.FirstOrDefault(i => i.Id == m_id);
                            d.ImageSrc = "/Images/" + postedFile.FileName;
                            ctx.Entry(d).State = EntityState.Modified;
                            ctx.SaveChanges();
                        }
                    }
                    else
                    {
                        m_id = -1;
                        return Request.CreateResponse(HttpStatusCode.BadRequest); 
                    }
                }
                m_id = -1;
                return Request.CreateResponse(HttpStatusCode.Created);
            }
            m_id = -1;
            return Request.CreateResponse(HttpStatusCode.BadRequest);          
        }
       

        [HttpPost]
        public string CreateNewDish(JObject data)
        {
            Dishes d = new Dishes();
            dynamic json1 = data;
            d.Name = json1.dishname;
            d.ImageSrc = json1.imagesrc;
            d.TimeToRun = json1.TimeToRun;
            d.Script = json1.script;
            d.AlphaConstant = json1.AlphaConstant;

            string time = json1.TimeToRun;
            string[] s = time.Split(':');
            d.TimeToRun = new TimeSpan(0, int.Parse(s[0]), int.Parse(s[1]));

         
            using (var ctx = new PicardDb())
            {
                ctx.Entry(d).State = EntityState.Added;
                ctx.SaveChanges();
                ctx.Entry(d).GetDatabaseValues();
                int id = d.Id;

                return id.ToString();
            }
        }

        [HttpGet]
        public string deleteDishById(int id)
        {
            using (var ctx = new PicardDb())
            {

                var result = (from r in ctx.m_dishes
                              where r.Id == id
                              select r).ToList();


                ctx.Entry(result[0]).State = EntityState.Deleted;
                ctx.SaveChanges();

                return getAllDishes();
            }
        }
      

        public string getDishById(int id)
        {
            using (var ctx = new PicardDb())
            {

                IEnumerable<Dishes> result = from r in ctx.m_dishes
                                             where r.Id == id
                                             select r;

                JavaScriptSerializer jss = new JavaScriptSerializer();
                string json = jss.Serialize(result);
                return json;
            }
        }

        
    }
}
