using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MyBookingRoles.Models;
using MyBookingRoles.Models.Store;

namespace MyBookingRoles.Controllers.Store
{
    public class ShoppingCartController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();

        // GET: ShoppingCart
        public ActionResult Cart()
        {
            ViewBag.User = User.Identity.GetUserName().ToString();
            return View();
        }

        // GET: ShoppingCart/Order Now
        public ActionResult OrderNow(int id)
        {
            //Fix Error On Cart Loading With This Expr101
            if(Session["cart"] == null)
            {
                List<Item> cart = new List<Item>();
                cart.Add(new Item(context.Products.Find(id),1));
                Session["cart"] = cart;

                //
                ViewBag.ListCart = cart.Count();
                Session["count"] = ViewBag.ListCart;
            }
            else
            {
                List<Item> cart = (List<Item>)Session["cart"];
                int index = isExisting(id);
                if (index == -1)
                    cart.Add(new Item(context.Products.Find(id), 1));
                else
                    cart[index].Quantity++;

                Session["cart"] = cart;
                ViewBag.ListCart = cart.Count();
                Session["count"] = ViewBag.ListCart;
            }
            
            return RedirectToAction("Cart");
        }

        //Is Existing
        private int isExisting(int id)
        {
            List<Item> cart = (List<Item>)Session["cart"];
            for (int i = 0; i < cart.Count; i++)
                if (cart[i].Pr.ProductID == id)
                    return i;
            return -1;
        }

        //GET: Update
        public ActionResult Update(FormCollection fc)
        {
            string[] quantity = fc.GetValues("quantity");
            List<Item> cart = (List<Item>)Session["cart"];
            for (int i = 0; i < cart.Count; i++)
                cart[i].Quantity = Convert.ToInt32(quantity[i]);
            Session["cart"] = cart;
            return RedirectToAction("Cart");
        }

        // GET: Delete
        public ActionResult Delete(int id)
        {
            //get current Id, and Cart Session
            int index = isExisting(id);

            List<Item> cart = (List<Item>)Session["cart"];
            cart.RemoveAt(index);

            Session["cart"] = cart;

            ViewBag.ListCart = cart.Count();
            Session["count"] = ViewBag.ListCart;
            return RedirectToAction("Cart");
        }

        [Authorize(Roles = "Customer")]
        public ActionResult CheckOut()
        {
            ViewBag.User = User.Identity.GetUserName().ToString();
            return View();
        }

        //[HttpPost]
        //Authorize to add the orders form Correctly
        [Authorize(Roles = "Customer")]
        public ActionResult ProcessOrder(FormCollection frc)
        {

            //Create a order model & Order Details Model
            List<Item> lstcart = (List<Item>)Session["cart"];

            //
            Order order = new Order()
            {
                PaymentAmount = System.Convert.ToDouble(frc["TotalAmount"]),
                CustomerName = frc["custName"],
                CustomerPhone = frc["custPhone"],
                CustomerEmail = frc["custEmail"],
                CustomerAddress = frc["custAddress"],
                OrderDate = DateTime.Now,
                PaymentType = "Cash", //Change Later
                Status = "Processing",
                OrderName = frc["custName"] + "-" + DateTime.Now + "-" + System.Convert.ToDouble(frc["TotalAmount"]),
            };
            order.SendMail();
            context.Orders.Add(order);
            context.SaveChanges();
            
            ///
            foreach (Item cart1 in lstcart)
            {
                OrderDetails item1 = new OrderDetails()
                {
                    OrderId = order.OrderId,
                    ProdId = cart1.Pr.ProductID,
                    Quantity = cart1.Quantity,
                    Price = cart1.Pr.Price,
                    
                 };

                context.OrderDetails.Add(item1);

                //Write Statement to Update product quantity on purchase
                Product prd = context.Products.Find(cart1.Pr.ProductID);
                prd.InStoreQuantity -= cart1.Quantity;
                context.Entry(prd).State = EntityState.Modified;

                context.SaveChanges();

                //Write Statement to Update product quantity on purchase
            }

            Session.Remove("cart");
            Session.Remove("count");
            return View("OrderSuccess",new { ord = order.OrderName});
            //Create a Add Statement for the modes
        }

        [Authorize(Roles = "Customer")]
        public ActionResult OrderSuccess()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();

            }
            base.Dispose(disposing);
        }
    }
}