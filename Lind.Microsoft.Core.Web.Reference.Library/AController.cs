using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Lind.Microsoft.Core.Web.Reference.Library
{
    public class AController : Controller
    {
        public IActionResult ToThePower(int bse, int exponent)
        {
            return View(Math.Pow(bse, exponent));
        }
    }
}