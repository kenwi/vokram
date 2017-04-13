using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using vokram.Core;
using vokram;

namespace WebApplicationBasic.Controllers
{
    public class HomeController : Controller
    {
        VokramBot bot;

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
