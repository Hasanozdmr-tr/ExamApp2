﻿using DataAccess.Abstract;
using Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Concrete
{
   public class ExamRepository: GenericRepository<Exam>, IExamDal
    {
    }
}
