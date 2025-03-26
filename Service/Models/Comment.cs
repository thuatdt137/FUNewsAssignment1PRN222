using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
	public class Comment
	{
		public int CommentId { get; set; }
		public string NewsArticleId { get; set; }
		public short AccountId { get; set; }
		public string Content { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public virtual SystemAccount Account { get; set; }
		public virtual NewsArticle NewsArticle { get; set; }
	}
}
