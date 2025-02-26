using Business.Interfaces;
using Domain.Models;
using FUNewsAssignment1PRN222.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FUNewsAssignment1PRN222.Controllers.Admin
{
    [AuthorizeRole("3")]
    public class UserController : Controller
    {
        private readonly ISystemAccountService _systemAccountService;
        public UserController(ISystemAccountService systemAccountService)
        {
            _systemAccountService = systemAccountService;
        }
        // GET: UserController
        public ActionResult Index()
        {
            var users = _systemAccountService.GetAllUsers();
            return View("~/Views/Admin/User/Index.cshtml", users);
        }

        // GET: UserController/Details/5
        public ActionResult Details(short id)
        {
            var user = _systemAccountService.GetUserById(id);
            return View("~/Views/Admin/User/Details.cshtml", user);
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            ViewBag.RoleList = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Staff" },
                new SelectListItem { Value = "2", Text = "Lecture" }
            };
            return View("~/Views/Admin/User/Create.cshtml");
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SystemAccount user)
        {
            if (_systemAccountService.GetUserByEmail(user.AccountEmail) != null)
            {
                ViewBag.ErrorMessage = "Email already existed";
                ViewBag.RoleList = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Staff" },
                new SelectListItem { Value = "2", Text = "Lecture" }
            };
                return View("~/Views/Admin/User/Create.cshtml", user);
            }
            if (ModelState.IsValid)
            {
                _systemAccountService.RegisterUser(user);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.RoleList = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Staff" },
                new SelectListItem { Value = "2", Text = "Lecture" }
            };
            return View("~/Views/Admin/User/Create.cshtml", user);
        }

        // GET: UserController/Edit/5
        public ActionResult Edit(short id)
        {
            var user = _systemAccountService.GetUserById(id);
            ViewBag.RoleList = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Staff" },
                new SelectListItem { Value = "2", Text = "Lecture" }
            };
            ViewBag.StatusList = new List<SelectListItem>
            {
                new SelectListItem { Value = "0", Text = "Inactive" },
                new SelectListItem { Value = "1", Text = "Active" }
            };
            return View("~/Views/Admin/User/Edit.cshtml", user);
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(short id, SystemAccount user)
        {
            var dbUser = _systemAccountService.GetUserById(id);
            if (ModelState.IsValid)
            {
                dbUser.AccountRole = user.AccountRole;
                dbUser.AccountName = user.AccountName;
                dbUser.Status = user.Status;
                _systemAccountService.UpdateUser(dbUser);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.RoleList = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Staff" },
                new SelectListItem { Value = "2", Text = "Lecture" }
            };
            ViewBag.StatusList = new List<SelectListItem>
            {
                new SelectListItem { Value = "0", Text = "Inactive" },
                new SelectListItem { Value = "1", Text = "Active" }
            };
            return View("~/Views/Admin/User/Edit.cshtml", user);
        }

        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
