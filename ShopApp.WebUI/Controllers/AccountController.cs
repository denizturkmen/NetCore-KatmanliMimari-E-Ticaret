using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using ShopApp.BusinessLayer.Abstract;
using ShopApp.WebUI.Extensions;
using ShopApp.WebUI.Identity;
using ShopApp.WebUI.Models;


namespace ShopApp.WebUI.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class AccountController : Controller
    {
        //Kullanıcı oluşturmak için
        private UserManager<ApplicationUser> _userManager;

        //Cookie oluyor login logout için
        private SignInManager<ApplicationUser> _signInManager;

        //SendGrid email göndermek 
        private IEmailSender _emailSender;

        //Cart Alişveriş sepeti ile ilgili işlemler
        private ICartService _cartService;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender, ICartService cartService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _cartService = cartService;
        }
        public IActionResult Register()
        {
            //eğer viewde model kullanmışsan modeli gette yolluycaksın yoksa nullexception alırsın
            return View(new RegisterModel());
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email,
                FullName = model.FullName
            };
            //password burda yapmamaız nedeni hashleyip veritabanı atmak için
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // generate token
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                // callback url ile confirmEmail action parametreleri gönderdik
                // kullanıcı onay maili
                var callbackUrl = Url.Action("ConfirmEmail", "Account", new
                {
                    //gidecek paramtreler 
                    userId = user.Id,
                    token = code
                });

                // send email
                // calbackurl link olarak kullanıcıya göndermek için
                await _emailSender.SendEmailAsync(model.Email, "Hesabınızı Onaylayınız.", $"Lütfen email hesabınızı onaylamak için linke <a href='http://localhost:49308{callbackUrl}'>tıklayınız.</a>");

                //Kullancı hatalari için ekranda bilgi
                TempData.Put("message", new ResultMessage()
                {
                    Title = "Hesap Onayı",
                    Message = "Eposta adrenize gelen link ile hesabınızı onaylayınız",
                    Css = "warning"
                });

                return RedirectToAction("Login", "Account");
            }

            //Modelstate olmadığunda genel hata için
            ModelState.AddModelError("", "Bilinmeyen hata oluştu lütfen tekrar deneyiniz.");
            return View(model);
        }


        public IActionResult Login(string ReturnUrl = null)
        {
            return View(new LoginModel()
            {
                //Model içinden aldık
                ReturnUrl = ReturnUrl
            });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Bu email ile daha önce hesap oluşturulmamış.");
                return View(model);
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError("", "Lütfen hesabınızı email ile onaylayınız.");
                return View(model);
            }

            //Ispersist tarayıcı kapandığında cookie kalsın mı kalmasın mı ayrıca startup.cs kalacağı süreyi belirtik
            // chechbox semimi aktifleştir
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, false);

            if (result.Succeeded)
            {
                return Redirect(model.ReturnUrl ?? "~/");
            }

            ModelState.AddModelError("", "Email veya parola yanlış");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            TempData.Put("message", new ResultMessage()
            {
                Title = "Oturum Kapatıldı.",
                Message = "Hesabınız güvenli bir şekilde sonlandırıldı.",
                Css = "warning"
            });

            return Redirect("~/");
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                TempData.Put("message", new ResultMessage()
                {
                    Title = "Hesap Onayı",
                    Message = "Hesap onayı için bilgileriniz yanlış",
                    Css = "danger"
                });

                return Redirect("~/");
            }

            // parametre olarak gelen userıd aldık
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                //maili onaylama bilgisi için
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {

                    // create cart object
                    //maili onaylayan kişinin cart table da bir userId sahip olmasını sağladık
                    _cartService.InitializeCart(user.Id);


                    TempData.Put("message", new ResultMessage()
                    {
                        Title = "Hesap Onayı",
                        Message = "Hesabınız başarıyla onaylanmıştır.",
                        Css = "success"
                    });

                    return RedirectToAction("Login");
                }


            }

            TempData.Put("message", new ResultMessage()
            {
                Title = "Hesap Onayı",
                Message = "Hesabınız onaylanamadı.",
                Css = "danger"
            });
            return View();
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string Email)
        {
            if (string.IsNullOrEmpty(Email))
            {
                TempData.Put("message", new ResultMessage()
                {
                    Title = "Forgot Password",
                    Message = "Bilgileriniz Hatalı",
                    Css = "danger"
                });

                return View();
            }

            var user = await _userManager.FindByEmailAsync(Email);

            if (user == null)
            {
                TempData.Put("message", new ResultMessage()
                {
                    Title = "Forgot Password",
                    Message = "Eposta adresi ile bir kullanıcı bulunamadı",
                    Css = "danger"
                });

                return View();
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);

            var callbackUrl = Url.Action("ResetPassword", "Account", new
            {
                token = code
            });

            // send email
            await _emailSender.SendEmailAsync(Email, "Reset Password", $"Parolanızı yenilemek için linke <a href='http://localhost:49308{callbackUrl}'>tıklayınız.</a>");

            TempData.Put("message", new ResultMessage()
            {
                Title = "Forgot Password",
                Message = "Parola yenilemek için hesabınıza mail gönderildi.",
                Css = "warning"
            });

            return RedirectToAction("Login", "Account");
        }


        public IActionResult ResetPassword(string token)
        {
            if (token == null)
            {
                return RedirectToAction("Home", "Index");
            }
            var model = new ResetPasswordModel { Token = token };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return RedirectToAction("Home", "Index");
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("Login", "Account");
            }
            return View(model);
        }

        public IActionResult Accessdenied()
        {
            return View();
        }
    }
}
