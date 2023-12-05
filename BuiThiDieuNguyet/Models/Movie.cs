using BuiThiDieuNguyet.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BuiThiDieuNguyet.Models
{
    public class Movie
    {
        public int ID { get; set; }
        [Display(Name = "Tiêu Đề")]
        [Required]
        [StringLength(200, MinimumLength = 3)]
        public string Title { get; set; }
        [Display(Name = "Tóm tắc")]
        [StringLength(int.MaxValue, MinimumLength = 3)]
        public string Summary { get; set; }
        [Display(Name = "Ngày phát hành")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode
        = true)]
        //"{0:yyyy-MM-dd}"
        [CheckDateGreaterThanTodayAttribute]
        public DateTime ReleaseDate { get; set; }

        [StringLength(10)]
        [Display(Name = "Thể loại")]
        [GenreAttribute]
        public string Genre { get; set; }
        [Display(Name = "Giá")]
        [Range(5000, double.MaxValue)]
        [DisplayFormat(DataFormatString = "{0:0,000}")]
        public decimal Price { get; set; }
        [Display(Name = "Xếp hạng")]
        [Range(1, 5)]
        public double Rated { get; set; }
        public byte[] Picture { get; set; }
        [NotMapped]
        [Display(Name = "Hình ảnh")]
        public HttpPostedFileBase PictureUpload { get; set; }
        [ForeignKey("GenreObj")]
        public int? GenreID { get; set; }
        public virtual Genre GenreObj { get; set; }
    }
}