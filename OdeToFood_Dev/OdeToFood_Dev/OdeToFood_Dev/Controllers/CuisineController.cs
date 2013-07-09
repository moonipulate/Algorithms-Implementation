using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OdeToFood_Dev.Filters;

namespace OdeToFood_Dev.Controllers
{
    [Log] 
    public class CuisineController : Controller
    {
        //
        // GET: /Cuisine/

        //[Authorize]
        public ActionResult Search(string name)
        {
            throw new Exception("Something terrible has happened");

            var message = Server.HtmlEncode(name);
            return Content(message);
        }


    }
}
