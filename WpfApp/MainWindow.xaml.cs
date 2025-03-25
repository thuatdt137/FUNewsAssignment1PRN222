using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp
{
	public partial class MainWindow : Window
	{
		private readonly FunewsManagementContext _context;
		private List<Category> categories = new();

		public MainWindow()
		{
			InitializeComponent();
			_context = new FunewsManagementContext();
			LoadData();
		}

		private void btnAddToList_Click(object sender, RoutedEventArgs e)
		{
			string newsTitle = txtNewsTitle.Text;
			string headline = txtHeadline.Text;
			string newsContent = txtNewsContent.Text;
			short? categoryId = cmbCategory.SelectedValue as short?;

			if (string.IsNullOrWhiteSpace(newsTitle) || string.IsNullOrWhiteSpace(headline) || string.IsNullOrWhiteSpace(newsContent))
			{
				MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}
			var newsArticle = new NewsArticle
			{
				NewsArticleId = Guid.NewGuid().ToString(),
				NewsTitle = newsTitle,
				Headline = headline,
				NewsContent = newsContent,
				CategoryId = categoryId,
				Category = _context.Categories.Find(categoryId)
			};

			_context.NewsArticles.Add(newsArticle);
			_context.SaveChanges();
		}

		private void LoadData()
		{
			categories = _context.Categories.ToList();
			cmbCategory.ItemsSource = categories;
			cmbCategory.SelectedIndex = 0;

		}

		private void btnLoadData_Click(object sender, RoutedEventArgs e)
		{
			lstNewsArticle.Items.Clear();
			foreach (var news in _context.NewsArticles.Include(m => m.Category).Where(m => m.NewsTitle.StartsWith("U")).ToList())
			{
				lstNewsArticle.Items.Add($"{news.NewsTitle}  {news.CreatedDate.Value.ToString("MM/dd/yyyy" ?? "N/A")} {news.Category.CategoryName}");

			}

		}

		private void btnSendToServer_Click(object sender, RoutedEventArgs e)
		{

        }
    }
}