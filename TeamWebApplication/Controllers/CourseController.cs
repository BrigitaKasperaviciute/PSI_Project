﻿using Microsoft.AspNetCore.Mvc;
using TeamWebApplication.Data;
using TeamWebApplication.Models;

namespace TeamWebApplication.Controllers
{
    public class CourseController : Controller
    {
        private readonly IUserContainer _userContainer;
        private readonly ICourseContainer _courseContainer;
        private readonly IRelationContainer _relationContainer;

        public CourseController(IUserContainer userContainer, ICourseContainer courseContainer, IRelationContainer relationContainer, IPostContainer postContainer)
        {
            _userContainer = userContainer;
            _courseContainer = courseContainer;
            _relationContainer = relationContainer;
        }


        public IActionResult Index()
        {
			_userContainer.CurrentCourseId = 0;
			IEnumerable<Course> coursesTaken = (
                from user in _userContainer.userList
                where user.UserId == _userContainer.LoggedInUserId
                from courseId in user.CoursesUserTakesId
                join course in _courseContainer.CourseList on courseId equals course.Id
                select course
            ).ToList();
            
            User currentUser = _userContainer.GetUser(_userContainer.LoggedInUserId);

            var viewModel = new CourseViewModel
            {
                Courses = coursesTaken,
                User = currentUser
            };

            return View(viewModel);
        }

        public IActionResult Create()
        {
            Course course = new Course();
            return View(course);
        }

        [HttpPost]
        public IActionResult Create(Course course)
        {
            int createdCourseId = _courseContainer.CreateCourse(course, _userContainer.LoggedInUserId);
            _userContainer.AddRelation(_userContainer.LoggedInUserId, createdCourseId);
            _relationContainer.AddRelationData(createdCourseId, _userContainer.LoggedInUserId);
            _courseContainer.WriteCourses();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int courseId)
        {
            Course? course = _courseContainer.GetCourse(courseId);
            return View(course);
        }

        [HttpPost]
        public IActionResult Edit(Course course)
        {
            Course? originalCourse = _courseContainer.GetCourse(course.Id);
            originalCourse.Name = course.Name;
            originalCourse.IsVisible = course.IsVisible;
            originalCourse.IsPublic = course.IsPublic;
            originalCourse.Description = course.Description;
            _courseContainer.WriteCourses();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int courseId)
        {
            Course course = _courseContainer.CourseList.SingleOrDefault(course => course.Id == courseId);
            return View(course);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteCourse(int courseId)
        {
            Course course = _courseContainer.CourseList.SingleOrDefault(course => course.Id == courseId);
            if (course == null)
            {
                return NotFound();
            }
            _courseContainer.DeleteCourse(course);
            _userContainer.DeleteRelation(_userContainer.LoggedInUserId, courseId);
            _relationContainer.DeleteRelationData(courseId, _userContainer.LoggedInUserId);
            _courseContainer.WriteCourses();
            return RedirectToAction("Index");
        }

        public IActionResult AddUser(int courseId)
        {
            _userContainer.CurrentCourseId = courseId;
            return View();
        }

        [HttpPost]
        public IActionResult AddUser(String userIdString)
        {
            String[] userIdList = userIdString.Split(';');
            Course? currentCourse = _courseContainer.GetCourse(_userContainer.CurrentCourseId);
            foreach (var word in userIdList)
            {
                if (Int32.TryParse(word, out int userId) != false && currentCourse != null)
                {
                    User? user;
                    if ((user = _userContainer.GetUser(userId)) != null && userId != _userContainer.LoggedInUserId)
                    {
                        _relationContainer.AddRelationData(_userContainer.CurrentCourseId, userId);
                        currentCourse.UsersInCourseId.Add(userId);
                        user.CoursesUserTakesId.Add(_userContainer.CurrentCourseId);
                    }
                }
            }
            return RedirectToAction("Index");
        }

        public IActionResult RemoveUser(int courseId)
        {
            _userContainer.CurrentCourseId = courseId;
            return View();
        }

        [HttpPost]
        public IActionResult RemoveUser(String userIdString)
        {
            String[] userIdList = userIdString.Split(';');
            Course? currentCourse = _courseContainer.GetCourse(_userContainer.CurrentCourseId);
            foreach (var word in userIdList)
            {
                if (Int32.TryParse(word, out int userId) != false && currentCourse != null)
                {
                    User? user;
                    if ((user = _userContainer.GetUser(userId)) != null && userId != _userContainer.LoggedInUserId)
                    {
                        _relationContainer.RemoveRelationData(_userContainer.CurrentCourseId, userId);
                        currentCourse.UsersInCourseId.Remove(userId);
                        user.CoursesUserTakesId.Remove(_userContainer.CurrentCourseId);
                    }
                }
            }
            return RedirectToAction("Index");
        }

        public IActionResult CheckUsers(int courseId)
        {
            _userContainer.CurrentCourseId = courseId;
            Course? currentCourse = _courseContainer.GetCourse(_userContainer.CurrentCourseId);
            ICollection<User> userInCourseList = (
                from user in _userContainer.userList
                where currentCourse.UsersInCourseId.Contains(user.UserId)
                select user
            ).ToList();
            return View(userInCourseList);
        }
    }
}