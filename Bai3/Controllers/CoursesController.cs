using Bai3.Models;
using Bai3.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bai3.Controllers
{
    public class CoursesController : Controller
    {
        // mat khau dang nhap tai khoan :Abc123.
        private readonly ApplicationDbContext _dbContext;
        public CoursesController()
        {
            _dbContext = new ApplicationDbContext();
        }
        // GET: Courses
        [Authorize]
        public ActionResult Create()
        {
            var viewModel = new CourseViewModel
            {
                Categories = _dbContext.Categories.ToList()
            };
            return View(viewModel);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CourseViewModel model)
        {
            if(!ModelState.IsValid)
            {
                model.Categories= _dbContext.Categories.ToList();
                return View(model);
            }
            var course = new Course
            {
                LectureId = User.Identity.GetUserId(),
                DateTime = model.GetDateTime(),
                CategoryId = model.Category,
                Place = model.Place
            };
            _dbContext.Courses.Add(course);
            _dbContext.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}