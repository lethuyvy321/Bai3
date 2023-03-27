using Bai3.Models;
using Bai3.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
                Categories = _dbContext.Categories.ToList(),
                Heading = "Add Course"
            };
            return View("CourseForm",viewModel);
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
        public ActionResult Attending()
        {
            var userId = User.Identity.GetUserId();

            var courses = _dbContext.Attendances
                .Where(a => a.AttendeeId == userId)
                .Select(a => a.Course)
                .Include(l => l.Lecture)
                .Include(l => l.Category)
                .ToList();

            var viewModel = new CoursesViewModel
            {
                UpcomingCourses = courses,
                ShowAction = User.Identity.IsAuthenticated
            };
            return View(viewModel);
        }
        [Authorize]
        public ActionResult Mine()
        {
            var userId = User.Identity.GetUserId();
            var courses = _dbContext.Courses
                .Where(c => c.LectureId== userId && c.DateTime > DateTime.Now)
                .Include(l => l.Category)
                .Include (l => l.Lecture)
                .ToList() ;
            return View(courses);
        }
        [Authorize]
        public ActionResult Edit (int id)
        {
            var userId = User.Identity?.GetUserId();
            var courses = _dbContext.Courses.Single(c => c.Id == id && c.LectureId == userId);

            var viewModel = new CourseViewModel
            {
                Categories = _dbContext.Categories.ToList(),
                Date = courses.DateTime.ToString("dd/MM/yyyy"),
                Time = courses.DateTime.ToString("HH:mm"),
                Category = courses.CategoryId,
                Place = courses.Place,
                Heading = "Edit Course",
                Id = courses.Id,
            };
            return View("CourseForm", viewModel);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(CourseViewModel viewModel)
        {
            if(!ModelState.IsValid)
            {
                viewModel.Categories = _dbContext.Categories.ToList();
                return View("CourseForm",viewModel);
            }
            var userId = User.Identity.GetUserId();
            var course = _dbContext.Courses.Single(c => c.Id == viewModel.Id&& c.LectureId == userId);
            
            course.Place = viewModel.Place;
            course.DateTime = viewModel.GetDateTime();
            course.CategoryId = viewModel.Category;

            _dbContext.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public ActionResult LectureIamGoing() 
        {
            var userId = User.Identity.GetUserId();
            // danh sach giang vien duoc theo doi boi toi
            var listFollowee = _dbContext.Followings
                .Where(p => p.FollowerId == userId).ToList();
            var listAttendances = _dbContext.Attendances
                .Where(p => p.AttendeeId== userId).ToList();
            var courses = new List<Course>();
            foreach(var item in listAttendances)
            {
                foreach(var item2 in listFollowee)
                {
                    if(item2.FolloweeId == item.Course.LectureId)
                    {
                        Course objCourse = item.Course;
                        courses.Add(objCourse);
                    }
                }
            }
            return View(courses);
        }
    }
}