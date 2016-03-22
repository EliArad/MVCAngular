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
    public class dishesconfigController : ApiController
    {
        
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
            Dishes d = new Dishes();
            dynamic json1 = data;
            d.Name = json1.Name;
            d.Id = json1.Id;
            d.ImageSrc = json1.ImageSrc;
            d.Script = json1.Script;
         
            using (var ctx = new PicardDb())
            {
                ctx.Entry(d).State = EntityState.Modified;
                ctx.SaveChanges();

                return getAllDishes();
            }
        }
         


        [HttpPost]
        public string CreateNewDish(JObject data)
        {
            Dishes d = new Dishes();
            dynamic json1 = data;
            d.Name = json1.dishname;
            d.ImageSrc = json1.imagesrc;
            d.Script = json1.script;
         
            using (var ctx = new PicardDb())
            {
                ctx.Entry(d).State = EntityState.Added;
                ctx.SaveChanges();

                return getAllDishes();
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
