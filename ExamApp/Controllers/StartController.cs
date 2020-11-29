using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Business.Abstract;
using Entity.Entities;
using ExamApp.Models;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ExamApp.Controllers
{
    [Authorize]
    public class StartController : Controller
    {
        private readonly IExamService _examService;
        private readonly IQuestionService _questionService;
        public StartController(IQuestionService questionService, IExamService examService)
        {
            _questionService = questionService;
            _examService = examService;
        }
        //public IActionResult GetTheQuestions()
        //{
        //    var QuestionList = _questionService.GetAll();

        //    List<StartTheExamModel> StartTheExamModel = new List<StartTheExamModel>();

        //    foreach (var question in QuestionList)
        //    {
        //        StartTheExamModel startTheExamModel = new StartTheExamModel();

        //        startTheExamModel.Id = question.Id;
        //        startTheExamModel.Title = question.Title;
        //        startTheExamModel.Content = question.Content;

        //        StartTheExamModel.Add(startTheExamModel);
        //    }

        //    return View(StartTheExamModel);
        //}


        //public  IActionResult ()
        //{
        //    StartExams exams = new StartExams();



        //    return View(new StartTheExamModel());
        //}
    }
}