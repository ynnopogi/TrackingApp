using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Tracking.App.Extensions;
using Tracking.Common;
using Tracking.Common.Models;

namespace Tracking.App.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index(string returnUrl = null) => View();

        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Index(AuthenticateRequest model, string returnUrl = null)
        {
            if (model is null)
                throw new ArgumentNullException(nameof(model));

            ViewData["ReturnUrl"] = returnUrl;
            IActionResult response = Unauthorized();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(Const.LocalBaseAddress);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header
            client.Timeout = TimeSpan.FromMinutes(30);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/api/users/authenticate");
            string jsonObject = JsonConvert.SerializeObject(model);
            request.Content = new StringContent(jsonObject, Encoding.UTF8, "application/json");//CONTENT-TYPE head
            await client.SendAsync(request).ContinueWith(responseTask =>
            {
                Task jsonTask = responseTask.Result.Content.ReadAsJsonAsync<JObject>().ContinueWith(jsonResponse =>
                {
                    var authenticateResponse = jsonResponse.Result;
                    if (authenticateResponse["token"] != null 
                        && !string.IsNullOrWhiteSpace(authenticateResponse["token"]?.ToString()))
                    {
                        HttpContext.Session.SetString("username", authenticateResponse["username"]?.ToString());
                        HttpContext.Session.SetString("access_token", authenticateResponse["token"]?.ToString());
                        response = RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, authenticateResponse["message"]?.ToString());
                        response = View(model);
                    }
                });

                jsonTask.Wait();
            });

            return await Task.FromResult(response);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            return RedirectToAction(nameof(EmployeesController.Index), "Employees");
            //if (Url.IsLocalUrl(returnUrl))
            //{
            //    return Redirect(returnUrl);
            //}
            //else
            //{
            //    return RedirectToAction(nameof(HomeController.Index), "Home");
            //}
        }
    }
}