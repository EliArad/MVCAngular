using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
//using MPFMCommonDefenitions;
using System.Runtime.Serialization;

namespace GojiBaseWebApiAngular.Models
{
    [Serializable]
    public class Dishes 
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string ImageSrc { get; set; }

        [Required]
        public string Script { get; set; }
                     
    }
}
