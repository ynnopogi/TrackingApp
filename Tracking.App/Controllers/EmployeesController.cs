using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Tracking.Common;
using Tracking.Common.ViewModels;

namespace Tracking.App.Controllers
{
    //[Authorize]
    public class EmployeesController : Controller
    {
        private readonly ILogger<EmployeesController> _logger;

        public EmployeesController(ILogger<EmployeesController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(Const.LocalBaseAddress);
            MediaTypeWithQualityHeaderValue contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("access_token"));
            HttpResponseMessage response = client.GetAsync("/api/employees").Result;
            string stringData = response.Content.ReadAsStringAsync().Result;
            List<EmployeeViewModel> data = JsonConvert.DeserializeObject<List<EmployeeViewModel>>(stringData);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                ViewBag.Message = "Unauthorized!";
            }

            return View(data); 
        }

        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(EmployeeViewModel model)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(Const.LocalBaseAddress);
            MediaTypeWithQualityHeaderValue contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("access_token"));
            string jsonObject = JsonConvert.SerializeObject(model);
            StringContent stringContent = new StringContent(jsonObject, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync("/api/employees", stringContent).Result;

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                ViewBag.Message = "Unauthorized!";
            }

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            EmployeeViewModel model = this.Get(id);
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(int id, EmployeeViewModel model)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(Const.LocalBaseAddress);
            MediaTypeWithQualityHeaderValue contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("access_token"));
            string jsonObject = JsonConvert.SerializeObject(model);
            StringContent stringContent = new StringContent(jsonObject, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PutAsync($"/api/employees/{id}", stringContent).Result;

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                ViewBag.Message = "Unauthorized!";
            }

            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            EmployeeViewModel model = this.Get(id);
            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(Const.LocalBaseAddress);
            MediaTypeWithQualityHeaderValue contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("access_token"));
            HttpResponseMessage response = client.DeleteAsync($"/api/employees/{id}").Result;

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                ViewBag.Message = "Unauthorized!";
            }

            return RedirectToAction("Index");
        }

        private EmployeeViewModel Get(int id)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(Const.LocalBaseAddress);
            MediaTypeWithQualityHeaderValue contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("access_token"));
            HttpResponseMessage response = client.GetAsync($"/api/employees/{id}").Result;
            string stringData = response.Content.ReadAsStringAsync().Result;
            EmployeeViewModel model = JsonConvert.DeserializeObject<EmployeeViewModel>(stringData);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                ViewBag.Message = "Unauthorized!";
            }

            return model;
        }
    }
}