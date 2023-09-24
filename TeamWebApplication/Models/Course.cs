﻿using System.ComponentModel.DataAnnotations;
using System.Security.Policy;
using TeamWebApplication.Models;

namespace TeamWebApplication.Models
{
    public class Course
    {
        //These variables are fetched from files
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public string Description { get; set; }
        public bool IsVisible { get; set; }
        public bool IsPublic { get; set; }
        public ICollection<int>? UsersInCourseId { get; set; }
        //These variables are not yet implemented
        public ICollection<int>? PostsInCourseId { get; set; }

        public Course() {
            UsersInCourseId = new List<int>();
            PostsInCourseId = new List<int>();
        }

        public Course(int id, string name, DateTime creationDate, string description, bool isVisible, bool isPublic) {
            this.Id = id;
            this.Name = name;
            this.CreationDate = creationDate;
            this.Description = description;
            this.IsVisible = isVisible;
            this.IsPublic = IsPublic;
            UsersInCourseId = new List<int>();
            PostsInCourseId = new List<int>();
        }

        public override string ToString()
        {
            return
                Id.ToString() + ";" +
                Name + ";" +
                CreationDate.ToString() + ";" +
                Description + ";" +
                IsVisible + ";" +
                IsPublic;
        }
    }
}
