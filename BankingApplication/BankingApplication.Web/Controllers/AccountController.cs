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
            var signedInUserLoginName = HttpContext.Session.GetString("SignedInUserLoginName");
            if (string.IsNullOrEmpty(signedInUserLoginName)) return Redirect("/Home/Index");
            var signedInAcc = _accountService.GetAccountByLoginName(signedInUserLoginName);
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
            var signedInUserLoginName = HttpContext.Session.GetString("SignedInUserLoginName");
            if (string.IsNullOrWhiteSpace(signedInUserLoginName)) return Redirect("SignIn");
            var account = _accountService.GetAccountByLoginName(signedInUserLoginName);
            if (account == null) return NotFound();

            return View(account);
        }

        //Handle concurency update for Account model
        [HttpPost]
        public IActionResult Edit(int? id, byte[] rowVersion)
        {
            if (string.IsNullOrWhiteSpace(HttpContext.Session.GetString("SignedInUserLoginName"))) return Redirect("SignIn");

            if (id == null)
            {
                return NotFound();
            }

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
                HttpContext.Session.SetString("SignedInUserLoginName", username.ToLower());
                return View("Index", acc);
            }

            return Redirect("/Home/Index");

        }

        public IActionResult SignOut()
        {
            HttpContext.Session.Remove("SignedInUserLoginName");
            HttpContext.Session.Clear();
            return Redirect("/Home/Index");
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(string userName, string password)
        {
            var checkUniqueAcc = _accountService.GetAccountByLoginName(userName);
            if (checkUniqueAcc != null)
            {
                ModelState.AddModelError(string.Empty, "The login name has been taken, please choose another one.");

                return View(checkUniqueAcc);
            }

            string number = Helper.GenerateAccountNumber(userName);

            string hashedPassword = !string.IsNullOrEmpty(password) ? Helper.GenerateHashedPassword(password) : string.Empty;

            Account acc = new Account
            {
                LoginName = userName,
                Balance = 0M,
                CreatedDate = DateTime.Now,
                AccountNumber = number,
                Password = hashedPassword
            };
            AccountDto accDto = _accountService.CreateAccount(acc);

            return Redirect("SignIn");
        }

        public IActionResult Deposit()
        {
            if (string.IsNullOrWhiteSpace(HttpContext.Session.GetString("SignedInUserLoginName"))) return Redirect("SignIn");
            return View();
        }

        public IActionResult Withdraw()
        {
            if (string.IsNullOrWhiteSpace(HttpContext.Session.GetString("SignedInUserLoginName"))) return Redirect("SignIn");
            return View();
        }

        public IActionResult Transfer()
        {
            if (string.IsNullOrWhiteSpace(HttpContext.Session.GetString("SignedInUserLoginName"))) return Redirect("SignIn");
            return View();
        }

        [HttpPost]
        public IActionResult Deposit(Transaction transaction)
        {
            string signedInUserLoginName = HttpContext.Session.GetString("SignedInUserLoginName");
            if (string.IsNullOrWhiteSpace(signedInUserLoginName)) return Redirect("SignIn");
            Account signedInUser = _accountService.GetAccountByLoginName(signedInUserLoginName);
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
            string signedInUserLoginName = HttpContext.Session.GetString("SignedInUserLoginName");
            if (string.IsNullOrWhiteSpace(signedInUserLoginName)) return Redirect("SignIn");
            Account signedInUser = _accountService.GetAccountByLoginName(signedInUserLoginName);

            if (signedInUser.Balance <= 0)
            {
                ModelState.AddModelError(string.Empty, "The money is out, you can not withdraw any more.");
                return View(signedInUser);
            }

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
            string signedInUserLoginName = HttpContext.Session.GetString("SignedInUserLoginName");
            if (string.IsNullOrWhiteSpace(signedInUserLoginName)) return Redirect("SignIn");
            Account signedInUser = _accountService.GetAccountByLoginName(signedInUserLoginName);
            if (signedInUser.Balance <= 0)
            {
                ModelState.AddModelError(string.Empty, "The money is out, you can not transfer to another account any more.");
                return View(signedInUser);
            }
            Account toAccount = _accountService.GetAccountByAccountNumber(transaction.ToAccount.AccountNumber.Trim());
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