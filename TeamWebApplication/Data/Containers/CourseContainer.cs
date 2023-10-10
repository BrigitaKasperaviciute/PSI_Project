﻿using TeamWebApplication.Models;
using System.Globalization;
using TeamWebApplication.ExtensionMethods;

namespace TeamWebApplication.Data
{
    public sealed class CourseContainer : ICourseContainer
    {
        public string FetchingPath { get; }
        private int courseIdCounter;
        public ICollection<Course> courseList { get; }

        public CourseContainer(IRelationContainer relationContainer, string fetchingPath = "./TextData/CourseData.txt")
        {
            this.FetchingPath = fetchingPath;
            courseList = new List<Course>();
            FetchCourses(relationContainer);
        }


		public void FetchCourses(IRelationContainer relationContainer)
        {
            string? readString;
            string[]? splitString;
            using (StreamReader? reader = new StreamReader(FetchingPath))
            {
                if ((readString = reader.ReadLine()) != null)
                    courseIdCounter = Int32.Parse(readString);

                while ((readString = reader.ReadLine()) != null)
                {
                    splitString = readString.Split(';');
                    Course course = new Course(
                        id: Int32.Parse(splitString[0]),                                                             
                        name: splitString[1],                                                                         
                        creationDate: DateTime.ParseExact(splitString[2], "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture), 
                        description: splitString[3],                                                                        
                        IsVisible: Boolean.Parse(splitString[4]),                                                        
                        isPublic: Boolean.Parse(splitString[5])                                                           
                    );
                    foreach (Relation<int> relation in relationContainer.relationData)
                    {
                        if (Int32.Parse(splitString[0]) == relation.Value1) //Course
                            course.UsersInCourseId.Add(relation.Value2); //User
                    }
                    courseList.Add(course);
                }
            }
        }

        public Course? GetCourse(int courseId)
        {
            Course? course = courseList.SingleOrDefault(course => course.Id == courseId);
            return course;
        }

        public int CreateCourse(Course course, int loggedInUserId)
        {
            course.Id = courseIdCounter;
            courseIdCounter++;
            course.CreationDate = DateTime.Now;
            course.UsersInCourseId.Add(loggedInUserId);
            courseList.Add(course);
            return course.Id;
        }

        public void WriteCourses()
        {
            using (StreamWriter? writer = new StreamWriter(FetchingPath))
            {
                writer.WriteLine(courseIdCounter);
                foreach (var course in courseList)
                    writer.WriteLine(course.FormattedToString());
            }
        }

        public void PrintCourseList()
        {
            foreach (var course in courseList)
                System.Diagnostics.Debug.WriteLine(course.FormattedToString());
        }

        public void PrintRelation()
        {
            foreach (var course in courseList)
            {
                foreach (var relation in course.UsersInCourseId)
                    System.Diagnostics.Debug.WriteLine(relation);
            }
        }
        public int DeleteCourse(Course courseToRemove)
        {
                courseList.Remove(courseToRemove);
                WriteCourses(); 
            return courseToRemove.Id;
        }
    }
}