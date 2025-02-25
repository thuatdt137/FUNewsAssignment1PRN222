using Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.Response
{
    public class NewsArticleViewModel
    {
        public string? NewsArticleId { get; set; }

        [Required]
        public string NewsTitle { get; set; }

        public string? Headline { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? NewsContent { get; set; }

        public string? NewsSource { get; set; }

        [Required]
        public short CategoryId { get; set; }  // Lưu category được chọn

        public bool? NewsStatus { get; set; }

        public short? CreatedById { get; set; }

        public short? UpdatedById { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public List<Category> Categories { get; set; } = new List<Category>(); // Danh sách Category
    }
}
