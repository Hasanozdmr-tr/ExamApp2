using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using ExamApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ExamApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public AuthController(IUserService userService, IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }

        public IActionResult Login()
        {
            return View(new UserLoginModel());
        }


        [HttpPost]
        public IActionResult Login(UserLoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _userService.GetUserByUsernameAndPassword(model.Username,model.Password);
                

                if (user == null)
                {
                    
                    return NotFound();
                    
                }
                if (user.Id == 2)
                {

                    return RedirectToAction("GetExamList", "Exam");

                }
                if (user.Id == 3)
                {

                    return  RedirectToAction("GetExamListByCandidate", "Exam");

                }
                ModelState.AddModelError("", "Kayıtlı Kullanıcı Bulunamamıştır, Lütfen Kayıt Olun !");
                
            }

            return View(model);
        }

        public IActionResult Register()
        {
            ViewBag.RoleList = new SelectList(_roleService.GetAll(), "Id", "RoleName");

            return View(new UserRegisterModel());
        }

        [HttpPost]
        public IActionResult Register2(UserRegisterModel model)
        {
            ViewBag.RoleList = new SelectList(_roleService.GetAll(), "Id", "RoleName");

            if (ModelState.IsValid)
            {
                var user = _userService.GetUserByUsernameAndPassword(model.Username, model.Password);

                if (user! == null )
                {
                    ModelState.AddModelError("", "Kullanıcı Mevcut");
                }
                else
                {
                    _userService.Save(new Entity.Entities.User()
                    {
                        Id = model.Id,
                        Username = model.Username,
                        Password = model.Password,
                        RoleId = model.RoleId
                    }); ;

                    return RedirectToAction("Login");
                }

            }

            return View(model);
        }
    }
}
