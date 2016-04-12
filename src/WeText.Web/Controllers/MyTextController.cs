using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;
using WeText.Web.Models;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WeText.Web.Controllers
{
    [Authorize]
    public class MyTextController : Controller
    {
        const string baseAddress = "http://localhost:9023/";


        public async Task<ActionResult> Index()
        {
            var userId = User.Identity.GetUserId();
            using (var proxy = new ServiceProxy(baseAddress))
            {
                var result = await proxy.GetAsync($"api/texts/user/{userId}");
                result.EnsureSuccessStatusCode();
                var model = JsonConvert.DeserializeObject<IEnumerable<TextViewModel>>(await result.Content.ReadAsStringAsync());
                return View(model);
            }
        }

        public async Task<ActionResult> Text(string id)
        {
            using (var proxy = new ServiceProxy(baseAddress))
            {
                var result = await proxy.GetAsync($"api/texts/{id}");
                result.EnsureSuccessStatusCode();
                var model = JsonConvert.DeserializeObject<IEnumerable<TextViewModel>>(await result.Content.ReadAsStringAsync()).FirstOrDefault();
                return View(model);
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateTextViewModel model)
        {
            var userId = User.Identity.GetUserId();
            using (var proxy = new ServiceProxy(baseAddress))
            {
                var result = await proxy.PostAsJsonAsync("api/texts/create", new
                {
                    model.Title,
                    model.Content,
                    UserId = userId
                });
                result.EnsureSuccessStatusCode();
                return RedirectToAction("Info", "Home", new
                {
                    MessageTitle = "Success!",
                    MessageText = "Text was created successfully.",
                    ReturnAction = "Index",
                    ReturnController = "MyText"
                });
            }
        }
    }
}