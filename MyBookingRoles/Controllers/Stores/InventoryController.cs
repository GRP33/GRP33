using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBookingRoles.Controllers.Stores
{
    public class InventoryController : Controller
    {
        // GET: Inventory
        public ActionResult ListIndex()
        {
            return View();
        }
    }
}