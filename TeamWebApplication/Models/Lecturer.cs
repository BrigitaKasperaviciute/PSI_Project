﻿using TeamWebApplication.Models;

namespace TeamWebApplication.Models
{
    public class Lecturer : User
    {
        public Specialization Title { get; set; }
        public AcademicDegree AcademicDegree { get; set; }
        public Lecturer()
        { }
    }
    public enum Title
    {
        Professor,
        AssistantProfessor,
        AssociateProfessor,
        Lecturer
    }
}
