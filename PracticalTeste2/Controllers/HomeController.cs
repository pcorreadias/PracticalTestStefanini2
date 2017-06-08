using PracticalTeste2.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using static PracticalTeste2.Models.CustomerModel;

namespace MovieApp.Controllers
{
    public class HomeController : Controller
    {
        private DatabaseEntities _db = new DatabaseEntities();
        private CustomerSearchModel _searchModel;

        public ActionResult Index()
        {
            if (Session["IdLoggedUser"] != null)
            {
                if (Session["RoleLoggedUser"].Equals("Administrator"))
                    return RedirectToAction("CustomerListAdmin");
                else
                    return RedirectToAction("CustomerListSeller");
            }
            else
                return RedirectToAction("Login");
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserSys user)
        {
            if (ModelState.IsValid)
            {
                using (DatabaseEntities dc = new DatabaseEntities())
                {
                    var v = dc.UserSys.Where(a => a.Email.Equals(user.Email) && a.Password.Equals(user.Password)).FirstOrDefault();

                    if (v != null)
                    {
                        Session["IdLoggedUser"] = v.Id.ToString();
                        Session["NameLoggedUser"] = v.Login.ToString();
                        Session["EmailLoggedUser"] = v.Email.ToString();
                        Session["RoleLoggedUser"] = v.UserRole.Name.ToString().Trim();
                        ViewBag.ErrorClass = "";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Message = "The e-mail and/or password entered is invalid. Please try again";
                        ViewBag.ErrorClass = "formValidateError";
                    }
                }
            }

            return View(user);
        }

        public ActionResult CustomerListAdmin(CustomerSearchModel searchModel)
        {
            if (Session["IdLoggedUser"] != null)
            {
                _searchModel = searchModel;

                var business = new CustomerModel();
                var customers = business.GetCustomers(_searchModel);

                ViewBag.ListOfGender = _db.Gender.ToList();
                ViewBag.ListOfCity = _db.City.ToList();
                ViewBag.ListOfRegion = _db.Region.ToList();
                ViewBag.ListOfClassification = _db.Classification.ToList();
                ViewBag.ListOfSeller = _db.UserSys.ToList();

                return View(customers);
            }
            else
                return RedirectToAction("Login");
        }

        public ActionResult CustomerListSeller(CustomerSearchModel searchModel, string sortOrder)
        {
            if (Session["IdLoggedUser"] != null)
            {
                _searchModel = searchModel;

                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

                searchModel.Seller = int.Parse(Session["IdLoggedUser"].ToString());
                var business = new CustomerModel();
                var customers = business.GetCustomers(_searchModel);

                ViewBag.ListOfGender = _db.Gender.ToList();
                ViewBag.ListOfCity = _db.City.ToList();
                ViewBag.ListOfRegion = _db.Region.ToList();
                ViewBag.ListOfClassification = _db.Classification.ToList();

                return View(customers);
            }
            else
                return RedirectToAction("Login");
        }
    }
}