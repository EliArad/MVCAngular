namespace GojiBaseWebApiAngular.Migrations
{
    using GojiBaseWebApiAngular.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<GojiBaseWebApiAngular.Models.PicardDb>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(GojiBaseWebApiAngular.Models.PicardDb context)
        {
            //  This method will be called after migrating to the latest version.

              //You can use the DbSet<T>.AddOrUpdate() helper extension method 
              //to avoid creating duplicate seed data. E.g.
            /*
                context.m_dishes.AddOrUpdate(
                  p => p.ImageSrc,
                  new Dishes { ImageSrc = "Andrew 1", Name="e" },
                  new Dishes { ImageSrc = "Brice 2", Name = "e" },
                  new Dishes { ImageSrc = "Rowan 3", Name = "e" }
                );
            */
        }
    }
}
