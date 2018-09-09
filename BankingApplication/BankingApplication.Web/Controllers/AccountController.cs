using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using BankingApplication.Infrastructure.Service;
using Microsoft.AspNetCore.Http;
using BankingApplication.Infrastructure.Entity;
using BankingApplication.Infrastructure.Dto;
using BankingApplication.Web.Utility;

namespace BankingApplication.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public IActionResult Index()
        {
            var signedInUserEmail = HttpContext.Session.GetString("SignedInUserEmail");
            if (string.IsNullOrEmpty(signedInUserEmail)) return Redirect("/Home/Index");
            var signedInAcc = _accountService.GetAccountByEmail(signedInUserEmail);
            return View(signedInAcc);
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = _accountService.GetAccount(id.GetValueOrDefault());

            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        [HttpGet]
        public IActionResult Edit()
        {
            var signedInUserEmail = HttpContext.Session.GetString("SignedInUserEmail");
            if (string.IsNullOrWhiteSpace(signedInUserEmail)) return Redirect("SignIn");
            var account = _accountService.GetAccountByEmail(signedInUserEmail);
            if (account == null) return NotFound();

            return View(account);
        }

        //Handle concurency update for Account model
        [HttpPost]
        public IActionResult Edit(int? id, byte[] rowVersion)
        {
            if (string.IsNullOrWhiteSpace(HttpContext.Session.GetString("SignedInUserEmail"))) return Redirect("SignIn");

            if (id == null)
            {
                return NotFound();
            }
            string signedInUserEmail = HttpContext.Session.GetString("SignedInUserEmail");

            var accountToUpdate = new Account { ID = id.GetValueOrDefault(), RowVersion = rowVersion };
            TryUpdateModelAsync(accountToUpdate);

            AccountDto accDto = _accountService.UpdateAccount(accountToUpdate);

            foreach (var e in accDto.Errors)
            {
                ModelState.AddModelError(string.Empty, "The information you try to edit "
                        + "was modified by another user after you got the original value. The "
                        + "edited operation was canceled and the current values in the database "
                        + "have been displayed. If you still want to edit this information, please "
                        + "refresh your browser to reload the latest informationi. Otherwise click the Back to Home hyperlink.");
                ModelState.AddModelError(e.Key, e.Value);

            }
            if (accDto.Errors.Any())
            {
                return View(accountToUpdate);
            }
            return View("Index", accDto.Account);
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignIn(string username, string password)
        {
            string hashedPassword = !string.IsNullOrEmpty(password) ? Helper.GenerateHashedPassword(password) : string.Empty;
            Account acc = _accountService.GetAccount(username, hashedPassword);
            if (acc != null)
            {
                HttpContext.Session.SetString("SignedInUserEmail", username.ToLower());
                return View("Index", acc);
            }

            return Redirect("/Home/Index");

        }

        public IActionResult SignOut()
        {
            HttpContext.Session.Remove("SignedInUserEmail");
            HttpContext.Session.Clear();
            return Redirect("/Home/Index");
        }

        [HttpPost]
        public IActionResult SignUp(string email, string name, string password)
        {
            string number = Helper.GenerateNumber(email, name);
            string hashedPassword = !string.IsNullOrEmpty(password) ? Helper.GenerateHashedPassword(password) : string.Empty;

            Account acc = new Account
            {
                Email = email,
                FullName = name,
                Balance = 0M,
                CreatedDate = DateTime.Now,
                Number = number,
                Password = hashedPassword
            };
            AccountDto accDto = _accountService.CreateAccount(acc);

            HttpContext.Session.SetString("SignedInUserEmail", email.ToLower());
            return View("Index", acc);
        }

        public IActionResult Deposit()
        {
            if (string.IsNullOrWhiteSpace(HttpContext.Session.GetString("SignedInUserEmail"))) return Redirect("SignIn");
            return View();
        }

        public IActionResult Withdraw()
        {
            if (string.IsNullOrWhiteSpace(HttpContext.Session.GetString("SignedInUserEmail"))) return Redirect("SignIn");
            return View();
        }

        public IActionResult Transfer()
        {
            if (string.IsNullOrWhiteSpace(HttpContext.Session.GetString("SignedInUserEmail"))) return Redirect("SignIn");
            return View();
        }

        [HttpPost]
        public IActionResult Deposit(Transaction transaction)
        {
            string signedInUserEmail = HttpContext.Session.GetString("SignedInUserEmail");
            if (string.IsNullOrWhiteSpace(signedInUserEmail)) return Redirect("SignIn");
            Account signedInUser = _accountService.GetAccountByEmail(signedInUserEmail);
            transaction.Type = TransactionType.Deposit;
            transaction.CreatedDate = DateTime.Now;
            transaction.ToAccount = signedInUser;

            TransactionDto transDto = _accountService.ExecuteTransaction(transaction);
            foreach (var e in transDto.Errors)
            {
                ModelState.AddModelError(e.Key, e.Value);
            }
            return View("SuccessTransaction");
        }

        [HttpPost]
        public IActionResult Withdraw(Transaction transaction)
        {
            string signedInUserEmail = HttpContext.Session.GetString("SignedInUserEmail");
            if (string.IsNullOrWhiteSpace(signedInUserEmail)) return Redirect("SignIn");
            Account signedInUser = _accountService.GetAccountByEmail(signedInUserEmail);
            transaction.Type = TransactionType.Withdraw;
            transaction.CreatedDate = DateTime.Now;
            transaction.FromAccount = signedInUser;

            TransactionDto transDto = _accountService.ExecuteTransaction(transaction);
            if (transDto.Errors.Any())
            {
                foreach (var e in transDto.Errors)
                {
                    ModelState.AddModelError(e.Key, e.Value);
                }
                return View();
            }

            return View("SuccessTransaction");
        }

        [HttpPost]
        public IActionResult Transfer(Transaction transaction)
        {
            string signedInUserEmail = HttpContext.Session.GetString("SignedInUserEmail");
            if (string.IsNullOrWhiteSpace(signedInUserEmail)) return Redirect("SignIn");
            Account signedInUser = _accountService.GetAccountByEmail(signedInUserEmail);
            Account toAccount = _accountService.GetAccountByNumber(transaction.ToAccount.Number.Trim());
            transaction.Type = TransactionType.Transfer;
            transaction.CreatedDate = DateTime.Now;
            transaction.FromAccount = signedInUser;
            transaction.ToAccount = toAccount;

            TransactionDto transDto = _accountService.ExecuteTransaction(transaction);

            if (transDto.Errors.Any())
            {
                foreach (var e in transDto.Errors)
                {
                    ModelState.AddModelError(e.Key, e.Value);
                }
                return View();
            }

            return View("SuccessTransaction");
        }
    }
}