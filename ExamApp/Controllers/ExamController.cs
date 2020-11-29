using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Business.Abstract;
using Entity.Entities;
using ExamApp.Models;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ExamApp.Controllers
{
    public class ExamController : Controller
    {
        string url = "https://www.wired.com/";

        private readonly IExamService _examService;
        private readonly IExamTypeService _examTypeService;
        private readonly IQuestionService _questionService;

        public ExamController(IExamService examService, IQuestionService questionService)
        {
            _examService = examService;
            _questionService = questionService;
        }

        public async Task<IActionResult> Index()
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(url);
            var pageContents = await response.Content.ReadAsStringAsync();

            HtmlDocument pageDocument = new HtmlDocument();
            pageDocument.LoadHtml(pageContents);

            var htmlNodes = pageDocument.DocumentNode.SelectNodes("//li[contains(@class,'card-component__description card-component__description')]//h2").ToArray();


            var hrefNodes = pageDocument.DocumentNode.SelectNodes("//li[contains(@class,'card-component__description card-component__description')]//a[2]").ToArray();

            List<TitleAddressViewModel> titleAddressViewModels = new List<TitleAddressViewModel>();
            for (int j = 0; j < 4; j++)
            {
                TitleAddressViewModel titleAddressViewModel = new TitleAddressViewModel();

                titleAddressViewModel.Title = htmlNodes[j].InnerText;
                titleAddressViewModel.Link = hrefNodes[j].Attributes["href"].Value;

                titleAddressViewModels.Add(titleAddressViewModel);
            }

            List<string> titleList = new List<string>();
            List<string> options = new List<string>()
            {
                "A","B","C","D"
            };

            int i = 0;

            foreach (var node in htmlNodes)
            {
                if (i == 5)
                    break;

                titleList.Add(node.InnerText);
                i++;
            }

            ViewBag.titleList = new SelectList(titleAddressViewModels, "Link", "Title");
            ViewBag.Options = new SelectList(options);

            return View(new ExamQuestionModel());
        }

        [HttpPost]
        public IActionResult CreateExam(ExamQuestionModel model)
        {
            if (ModelState.IsValid)
            {
                Exam exam = new Exam();
                exam.Content = model.Exam.Content;
                exam.CreatedDate = DateTime.Now;
                exam.Title = model.Exam.Title;
                exam.Questions = model.Questions;

                _examService.Save(exam);

               
                ExamType examType = new ExamType();

                if (examType.Id == 0)
                { examType.Id = 1; }
                else
                { examType.Id = examType.Id + 1; }
                examType.Content = model.Exam.Content;
                examType.CreatedDate = DateTime.Now;
                examType.Title = model.Exam.Title;
                examType.Questions = model.Questions;

                _examTypeService.Save(examType);

            }
           

            return RedirectToAction("GetExamList");
        }

        public IActionResult StartExam(StartTheExamModel model)
        {
            if (ModelState.IsValid)
            {
                Exam exam = new Exam();
                exam.Id = model.Id;
                exam.Content = model.Content;
                exam.CreatedDate = DateTime.Now;
                exam.Title = model.Title;
                exam.Questions = model.Questions;

                _examService.GetById(exam.Id);
            }

            return RedirectToAction("StartTheExam");
        }

        public async Task<IActionResult> GetContent(string href)
        {
            string contentUrl = "https://www.wired.com" + href;

            HttpClient client = new HttpClient();
            var response = await client.GetAsync(contentUrl);
            var pageContents = await response.Content.ReadAsStringAsync();

            HtmlDocument pageDocument = new HtmlDocument();
            pageDocument.LoadHtml(pageContents);

            var htmlNode = pageDocument.DocumentNode.SelectSingleNode("//div[contains(@class,'grid--item body body__container article__body grid-layout__content')]//p");

            string content = htmlNode.InnerText;

            var result = new JsonResult(content);
            return result;
        }

        public IActionResult GetExamList()
        {
            var examList = _examService.GetAll();

            List<ExamListModel> examModelList = new List<ExamListModel>();

            foreach (var exam in examList)
            {
                ExamListModel examListModel = new ExamListModel();

                examListModel.ExamId = exam.Id;
                examListModel.Title = exam.Title;
                examListModel.CreatedDate = exam.CreatedDate;

                examModelList.Add(examListModel);
            }

            return View(examModelList);
        }

        public IActionResult GetExamListByCandidate()
        {
            var examList = _examService.GetAll();

            List<ExamListModel> examModelList = new List<ExamListModel>();

            foreach (var exam in examList)
            {
                ExamListModel examListModel = new ExamListModel();

                examListModel.ExamId = exam.Id;
                examListModel.Title = exam.Title;
                examListModel.CreatedDate = exam.CreatedDate;

                examModelList.Add(examListModel);
            }

            return View(examModelList);
        }


        public IActionResult Delete(int id)
        {
            var exam = _examService.GetById(id);

            _examService.Delete(exam);

            return RedirectToAction("GetExamList");
        }

        public IActionResult GetAll(int id)
        {
            var exam = _examService.GetById(id);
            _examService.GetById(id);
            return RedirectToAction("StartExam");
        }
    }
}
