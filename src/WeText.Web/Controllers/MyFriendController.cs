using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web.Mvc;
using WeText.Web.Models;
using Newtonsoft.Json;
using System;

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

                var getSentInvitationsResult = await proxy.GetAsync($"api/social/invitation/{userId}/sent");
                getSentInvitationsResult.EnsureSuccessStatusCode();
                dynamic sentInvitations = JsonConvert.DeserializeObject(await getSentInvitationsResult.Content.ReadAsStringAsync());
                foreach(var inv in sentInvitations)
                {
                    var statusName = string.Empty;
                    bool isCompleted = false;
                    switch((int)inv.InvitationEndReason)
                    {
                        case 1:
                            statusName = "Waiting For Approval";
                            break;
                        case 2:
                            statusName = "Accepted";
                            isCompleted = true;
                            break;
                        case 3:
                            statusName = "Rejected";
                            isCompleted = true;
                            break;
                    }
                    vm.SentInvitations.Add(new InvitationViewModel
                    {
                        InvitationId = Guid.Parse((string)inv.InvitationId),
                        UserDisplayName = inv.TargetUserName,
                        SentDate = Convert.ToDateTime(inv.InvitationStartDate),
                        Status = statusName,
                        IsCompleted = isCompleted,
                        CompleteDate = Convert.ToDateTime(inv.InvitationEndDate)
                    });
                }

                var getReceivedInvitationsResult = await proxy.GetAsync($"api/social/invitation/{userId}/received");
                getReceivedInvitationsResult.EnsureSuccessStatusCode();
                dynamic receivedInvitations = JsonConvert.DeserializeObject(await getReceivedInvitationsResult.Content.ReadAsStringAsync());
                foreach (var inv in receivedInvitations)
                {
                    var statusName = string.Empty;
                    bool isCompleted = false;
                    switch ((int)inv.InvitationEndReason)
                    {
                        case 1:
                            statusName = "Waiting For Approval";
                            break;
                        case 2:
                            statusName = "Accepted";
                            isCompleted = true;
                            break;
                        case 3:
                            statusName = "Rejected";
                            isCompleted = true;
                            break;
                    }
                    vm.ReceivedInvitations.Add(new InvitationViewModel
                    {
                        InvitationId = Guid.Parse((string)inv.InvitationId),
                        UserDisplayName = inv.OriginatorName,
                        SentDate = Convert.ToDateTime(inv.InvitationStartDate),
                        Status = statusName,
                        IsCompleted = isCompleted,
                        CompleteDate = Convert.ToDateTime(inv.InvitationEndDate)
                    });
                }
                return View(vm);
            }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Invite(MyFriendViewModel model)
        {
            var userId = User.Identity.GetUserId();
            var targetUserId = model.SelectedUserId;
            var invitationLetter = model.InvitationLetter;
            using (var proxy = new ServiceProxy(baseAddress))
            {
                var result = await proxy.PostAsJsonAsync("api/social/invitation/send", new
                {
                    OriginatorId = userId,
                    TargetUserId = targetUserId,
                    InvitationLetter = invitationLetter
                });
                result.EnsureSuccessStatusCode();
                return RedirectToAction("Info", "Home", new
                {
                    MessageTitle = "Success!",
                    MessageText = "Invitation has been sent successfully.",
                    ReturnAction = "Index",
                    ReturnController = "MyFriend"
                });
            }
        }

        public async Task<ActionResult> Accept(Guid invitationId)
        {
            var userId = User.Identity.GetUserId();
            using (var proxy = new ServiceProxy(baseAddress))
            {
                var result = await proxy.PostAsJsonAsync("api/social/invitation/accept", new
                {
                    CurrentUserId = userId,
                    InvitationId = invitationId
                });
                return RedirectToAction("Info", "Home", new
                {
                    MessageTitle = "Success!",
                    MessageText = "Approval has been sent successfully.",
                    ReturnAction = "Index",
                    ReturnController = "MyFriend"
                });
            }
        }
    }
}