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
    public class AppConfig 
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int MotorLength { get; set; }
        
        [Required]
        public int MaxLength { get; set; }

        [Required]
        public int MinLength { get; set; }                     
    }
}
