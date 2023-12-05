using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BuiThiDieuNguyet.Models
{
    public class Genre
    {
        [Key]
        public int ID { get; set; }
        [Display(Name = "Tên")]
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        public virtual IList<Movie> Movies { get; set; }
    }
}