using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WeText.Web.Models;
using Newtonsoft.Json;

namespace WeText.Web.Controllers
{
    [Authorize]
    public class MyFriendController : Controller
    {
        const string baseAddress = "http://localhost:9023/";

        public async Task<ActionResult> Index()
        {
            var vm = new MyFriendViewModel();
            var userId = User.Identity.GetUserId();
            using (var proxy = new ServiceProxy(baseAddress))
            {
                var result = await proxy.GetAsync($"api/social/others/{userId}");
                result.EnsureSuccessStatusCode();
                dynamic userNames = JsonConvert.DeserializeObject(await result.Content.ReadAsStringAsync());
                List<SelectListItem> items = new List<SelectListItem>();
                foreach(var userName in userNames)
                {
                    items.Add(new SelectListItem { Text = userName.DisplayName, Value = userName.UserId });
                }
                vm.UserNames = items;
                return View(vm);
            }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Invite(MyFriendViewModel model)
        {
            return View(model);
        }
    }
}