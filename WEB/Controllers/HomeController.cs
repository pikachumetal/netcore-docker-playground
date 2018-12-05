using BIZ;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using WEB.Models;

namespace WEB.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var client = new RestClient("http://localhost:5000/api/messages");
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            var response = client.Execute(request);
            var messages = JsonConvert.DeserializeObject<IEnumerable<Message>>(response.Content);
            return View(messages);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Message message)
        {
            if (!ModelState.IsValid) return BadRequest();

            var client = new RestClient("http://localhost:5000/api/messages");
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("undefined", "{ 'text': '" + message.Text + "'}", ParameterType.RequestBody);
            var response = client.Execute(request);

            return response.StatusCode == HttpStatusCode.Created ? (ActionResult)RedirectToAction("Index") : BadRequest(response.ErrorMessage);
        }

        public IActionResult Delete(long? messageID)
        {
            if (messageID == null) return BadRequest();
            if (messageID < 1) return BadRequest();

            var client = new RestClient($"http://localhost:5000/api/messages/{messageID}");
            var request = new RestRequest(Method.DELETE);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Content-Type", "application/json");
            var response = client.Execute(request);
            return response.StatusCode == HttpStatusCode.NoContent ? (ActionResult)RedirectToAction("Index") : BadRequest(response.ErrorMessage);
        }
    }
}
