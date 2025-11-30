using Quizdom.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Quizdom.Controllers
{
    public class AccountController : Controller
    {
        QuizApp4Entities1 db = new QuizApp4Entities1();

        // =============== LOGIN ===============
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            var user = db.Users.FirstOrDefault(u =>
                u.Username == username && u.Password == password);

            if (user != null)
            {
                // Lưu phiên đăng nhập
                Session["UserID"] = user.UserID;
                Session["Username"] = user.Username;

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Sai Username hoặc Password!";
            return View();
        }

        // =============== REGISTER ===============
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User user)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Vui lòng nhập đầy đủ!";
                return View();
            }

            var exists = db.Users.Any(u => u.Username == user.Username);
            if (exists)
            {
                ViewBag.Error = "Username đã tồn tại!";
                return View();
            }

            user.CreatedAt = DateTime.Now;

            db.Users.Add(user);
            db.SaveChanges();

            // Tự động login
            Session["UserID"] = user.UserID;
            Session["Username"] = user.Username;

            return RedirectToAction("Index", "Home");
        }

        // =============== LOGOUT ===============
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }

        // =============== PROFILE ===============
        public ActionResult UserProfile()
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login");

            int id = (int)Session["UserID"];
            var user = db.Users.FirstOrDefault(u => u.UserID == id);

            return View(user);
        }

        public ActionResult EditProfile()
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login");

            int id = (int)Session["UserID"];
            var user = db.Users.FirstOrDefault(u => u.UserID == id);

            return View(user);
        }

        [HttpPost]
        public ActionResult EditProfile(User model)
        {
            var user = db.Users.Find(model.UserID);

            if (user == null)
                return HttpNotFound();

            user.FullName = model.FullName;
            user.Email = model.Email;

            db.SaveChanges();

            return RedirectToAction("UserProfile");
        }

    }
}
