using Business.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FUNewsAssignment1PRN222.Controllers.Admin
{
    public class TagController : Controller
    {
        private readonly ITagService _tagService;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        // GET: Danh sách tất cả các tag
        public IActionResult Index()
        {
            var tags = _tagService.GetAllTags();
            return View(tags);
        }

        // GET: Tạo tag mới
        public IActionResult Create()
        {
            return View();
        }

        // POST: Thêm tag mới
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Tag tag)
        {
            if (ModelState.IsValid)
            {
                _tagService.AddTag(tag);
                return RedirectToAction(nameof(Index));
            }
            return View(tag);
        }

        // GET: Chỉnh sửa tag
        public IActionResult Edit(int id)
        {
            var tag = _tagService.GetTagById(id);
            if (tag == null)
            {
                return NotFound();
            }
            return View(tag);
        }

        // POST: Cập nhật tag
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Tag tag)
        {
            if (ModelState.IsValid)
            {
                _tagService.UpdateTag(tag);
                return RedirectToAction(nameof(Index));
            }
            return View(tag);
        }

        // POST: Xóa tag
        [HttpPost]
        public IActionResult Delete(int id)
        {
            _tagService.DeleteTag(id);
            return RedirectToAction(nameof(Index));
        }
    }

}
