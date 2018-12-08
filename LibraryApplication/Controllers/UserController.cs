//using LibraryApplication.Models;
//using LibraryApplication.Utility;
//using LibraryApplication.ViewModel;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Web;
//using System.Web.Mvc;

//namespace LibraryApplication.Controllers
//{
//    [Authorize(Roles = StaticDetails.AdminUserRole)]
//    public class UserController : Controller
//    {
//        private ApplicationDbContext db;
//        public UserController()
//        {
//            db = ApplicationDbContext.Create();
//        }
//        // GET: User
//        public ActionResult Index()
//        {
//            var user = from u in db.Users
//                       join m in db.Memberships on u.MembershipID equals m.ID
//                       select new UserViewModel
//                       {
//                           Id = u.Id,
//                           FirstName = u.FirstName,
//                           LastName = u.LastName,
//                           Email = u.Email,
//                           BirthDate = u.BirthDate,
//                           Phone = u.Phone,
//                           MembershipID = u.MembershipID,
//                           Memberships = (ICollection<Membership>)db.Memberships.ToList().Where(n => n.ID.Equals(u.MembershipID)),
//                           Disabled = u.Disable
//                       };
//            var usersList = user.ToList();
//            return View(usersList);
//        }
//        //GET Edit
//        public ActionResult Edit(string id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            ApplicationUser user = db.Users.Find(id);
//            if (user == null)
//            {
//                return HttpNotFound();
//            }
//            UserViewModel model = new UserViewModel()
//            {
//                FirstName = user.FirstName,
//                LastName = user.LastName,
//                Email = user.Email,
//                BirthDate = user.BirthDate,
//                Id = user.Id,
//                MembershipID = user.MembershipID,
//                Memberships = db.Memberships.ToList(),
//                Phone = user.Phone,
//                Disabled = user.Disable
//            };
//            return View(model);
//        }
//        //POST Method for EDIT Action
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit(UserViewModel user)
//        {
//            if (!ModelState.IsValid)
//            {
//                UserViewModel model = new UserViewModel()
//                {
//                    FirstName = user.FirstName,
//                    LastName = user.LastName,
//                    Email = user.Email,
//                    BirthDate = user.BirthDate,
//                    Id = user.Id,
//                    MembershipID = user.MembershipID,
//                    Memberships = db.Memberships.ToList(),
//                    Phone = user.Phone,
//                    Disabled = user.Disabled
//                };
//                return View(model);
//            }
//            else
//            {
//                var userInDb = db.Users.Single(u => u.Id == user.Id);
//                userInDb.FirstName = user.FirstName;
//                userInDb.LastName = user.LastName;
//                userInDb.BirthDate = user.BirthDate;
//                userInDb.Email = user.Email;
//                userInDb.MembershipID = user.MembershipID;
//                userInDb.Phone = user.Phone;
//                userInDb.Disable = user.Disabled;
//            }
//            db.SaveChanges();
//            return RedirectToAction("Index", "Users");
//        }
//        public ActionResult Details(string id)
//        {
//            if (id == null || id.Length == 0)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            ApplicationUser user = db.Users.Find(id);
//            UserViewModel model = new UserViewModel()
//            {
//                FirstName = user.FirstName,
//                LastName = user.LastName,
//                Email = user.Email,
//                BirthDate = user.BirthDate,
//                Id = user.Id,
//                MembershipID = user.MembershipID,
//                Memberships = db.Memberships.ToList(),
//                Phone = user.Phone,
//                Disabled = user.Disable
//            };
//            return View(model);
//        }
//        //DELETE Get Method
//        public ActionResult Delete(string id)
//        {
//            if (id == null || id.Length == 0)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            ApplicationUser user = db.Users.Find(id);
//            UserViewModel model = new UserViewModel()
//            {
//                FirstName = user.FirstName,
//                LastName = user.LastName,
//                Email = user.Email,
//                BirthDate = user.BirthDate,
//                Id = user.Id,
//                MembershipID = user.MembershipID,
//                Memberships = db.Memberships.ToList(),
//                Phone = user.Phone,
//                Disabled = user.Disable
//            };
//            return View(model);
//        }
//        //DELETE Post method
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult UsersDeletePost(string id)
//        {
//            var userInDb = db.Users.Find(id);
//            if (id == null || id.Length == 0)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            userInDb.Disable = true;
//            db.SaveChanges();
//            return View();
//        }
//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//            }
//        }
//    }
//}

using LibraryApplication.Models;
using LibraryApplication.Utility;
using LibraryApplication.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace LibraryApplication.Controllers
{
    [Authorize(Roles = StaticDetails.AdminUserRole)]
    public class UserController : Controller
    {
        private ApplicationDbContext db;


        public UserController()
        {
            db = ApplicationDbContext.Create();
        }

        // GET: User
        public ActionResult Index()
        {
            var user = from u in db.Users
                       join m in db.Memberships on u.MembershipID equals m.ID
                       select new UserViewModel
                       {
                           Id = u.Id,
                           FirstName = u.FirstName,
                           LastName = u.LastName,
                           Email = u.Email,
                           BirthDate = u.BirthDate,
                           Phone = u.Phone,
                           MembershipID = u.MembershipID,
                           Memberships = (ICollection<Membership>)db.Memberships.ToList().Where(n => n.ID.Equals(u.MembershipID)),
                           Disabled = u.Disable
                       };

            var usersList = user.ToList();

            return View(usersList);
        }

        //EDIT GET
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ApplicationUser user = db.Users.Find(id);

            if (user == null)
            {
                return HttpNotFound();
            }

            UserViewModel model = new UserViewModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                BirthDate = user.BirthDate,
                Id = user.Id,
                MembershipID = user.MembershipID,
                Memberships = db.Memberships.ToList(),
                Phone = user.Phone,
                Disabled = user.Disable
            };

            return View(model);
        }


        //EDIT POST ACTION
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserViewModel user)
        {
            if (!ModelState.IsValid)
            {
                UserViewModel model = new UserViewModel()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    BirthDate = user.BirthDate,
                    Id = user.Id,
                    MembershipID = user.MembershipID,
                    Memberships = db.Memberships.ToList(),
                    Phone = user.Phone,
                    Disabled = user.Disabled
                };
                return View(model);
            }
            else
            {
                var userInDb = db.Users.Single(u => u.Id == user.Id);
                userInDb.FirstName = user.FirstName;
                userInDb.LastName = user.LastName;
                userInDb.BirthDate = user.BirthDate;
                userInDb.Email = user.Email;
                userInDb.MembershipID = user.MembershipID;
                userInDb.Phone = user.Phone;
                userInDb.Disable = user.Disabled;
            }

            db.SaveChanges();

            return RedirectToAction("Index", "User");
        }

        //DETAILS ACTION METHOD
        public ActionResult Details(string id)
        {
            if (id == null || id.Length == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = db.Users.Find(id);
            UserViewModel model = new UserViewModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                BirthDate = user.BirthDate,
                Id = user.Id,
                MembershipID = user.MembershipID,
                Memberships = db.Memberships.ToList(),
                Phone = user.Phone,
                Disabled = user.Disable
            };
            return View(model);
        }

        //DELETE GET
        public ActionResult Delete(string id)
        {
            if (id == null || id.Length == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = db.Users.Find(id);
            UserViewModel model = new UserViewModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                BirthDate = user.BirthDate,
                Id = user.Id,
                MembershipID = user.MembershipID,
                Memberships = db.Memberships.ToList(),
                Phone = user.Phone,
                Disabled = user.Disable
            };
            return View(model);
        }

        //DELETE POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            var userInDb = db.Users.Find(id);
            if (id == null || id.Length == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            userInDb.Disable = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
        }
    }
}