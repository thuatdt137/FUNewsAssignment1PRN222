using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public partial class NewsArticle
{
	public string NewsArticleId { get; set; } = null!;

	[Required(ErrorMessage = "News title is required")]
	[StringLength(200, ErrorMessage = "News title cannot exceed 200 characters")]
	public string? NewsTitle { get; set; }

	[Required(ErrorMessage = "Headline is required")]
	[StringLength(500, ErrorMessage = "Headline cannot exceed 500 characters")]
	public string Headline { get; set; } = null!;
	[DataType(DataType.DateTime)]
	public DateTime? CreatedDate { get; set; } = DateTime.Now;
	[Required(ErrorMessage = "Content is required")]
	public string? NewsContent { get; set; }
	[StringLength(300, ErrorMessage = "News source cannot exceed 300 characters")]
	public string? NewsSource { get; set; }
	public short? CategoryId { get; set; }
	[Required]
	public bool? NewsStatus { get; set; }
	public short? CreatedById { get; set; }
	public short? UpdatedById { get; set; }
	public DateTime? ModifiedDate { get; set; }

	public virtual Category? Category { get; set; }

	public virtual SystemAccount? CreatedBy { get; set; }

	public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
	public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
