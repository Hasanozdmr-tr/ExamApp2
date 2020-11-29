using Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IExamTypeService : IGenericService<Exam>
    {
        void Save(ExamType examType);
    }
}
