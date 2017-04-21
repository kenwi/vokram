using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Vokram.Bot;
using Vokram.Core;


namespace WebApplicationBasic.Controllers
{
    public class HomeController : Controller
    {
        readonly VokramBot bot;

        public HomeController()
        {
            bot = new VokramBot("chat.freenode.net", "vokram")
            {
                ClientRegistered = (s, e) => bot.Join("#hadamard")
            };
        }

        public IActionResult Index()
        {
            bot.Connect("chat.freenode.net");
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
