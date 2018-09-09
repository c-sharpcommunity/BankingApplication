using Microsoft.AspNetCore.Mvc;
using BankingApplication.Infrastructure.Service;
using Microsoft.AspNetCore.Http;

namespace BankingApplication.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAccountService _accountService;

        public HomeController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public IActionResult Index()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("SignedInUserEmail")))
            {
                return Redirect("/Account/Index");
            }
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult ViewTransaction()
        {
            string signedInUserEmail = HttpContext.Session.GetString("SignedInUserEmail");
            if (string.IsNullOrWhiteSpace(signedInUserEmail)) return Redirect("SignIn");
            var account = _accountService.GetAccountByEmail(signedInUserEmail);
            var transactions = _accountService.GetTheirOwnTransactions(account.ID);
            
            return View(transactions);
        }
    }
}
