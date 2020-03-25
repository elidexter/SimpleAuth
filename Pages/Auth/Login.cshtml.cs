using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuthGo.Pages.Auth
{
    public class LoginModel : PageModel
    {
        public string ReturnURL { get; set; }
        public async Task OnGetAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ReturnURL = returnUrl;
        }
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            ReturnURL = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var properties = new AuthenticationProperties
                {
                    AllowRefresh = false,
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddSeconds(10)
                };
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier,"correo@gmail.com"),
                    new Claim(ClaimTypes.Name, "Name"),
                    new Claim(ClaimTypes.GivenName, "GivenName"),
                    new Claim(ClaimTypes.Surname, "Surname"),
                    new Claim(ClaimTypes.DateOfBirth, DateTime.Now.ToString("o"), ClaimValueTypes.DateTime)
                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(principal, properties);
                return LocalRedirect(ReturnURL);
            }            
            return Page();
        }        
    }
}