using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;


namespace GojiBaseWebApiAngular.Models
{
    public class PicardDb : DbContext
    {
        public DbSet<Dishes> m_dishes { get; set; }
        
      
    }
}