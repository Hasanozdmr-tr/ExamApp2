﻿using Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.Entities
{
    public class ExamType : ITable
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<Question> Questions { get; set; }
    }
}
