
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyBookingRoles.Models.PayPals;
using MyBookingRoles.Models.Store;
using PayPal.Api;
using Item = PayPal.Api.Item;

namespace MyBookingRoles.Controllers.PayPals
{
    public class PayPalController : Controller
    {
        // GET: PayPal
        public ActionResult Index()
        {
            return View();
        }

        //paying through PAYPAL
        public ActionResult PaymentWithPayPal(string cancel = null)
        {
            //geting apiContext
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
                //A resource representing a Payer that funds a payment Payment Method as paypal
                //Payer Id will be returned when payment proceeds or click to pay
                string payerId = Request.Params["PayerID"];

                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist
                    //it is returned by the create function call of the payment class

                    // Creating a payment
                    // baseURL is the url on which paypal sendsback the data.
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority +
                                "/PayPal/PaymentWithPayPal?";

                    //here we are generating guid for storing the paymentID received in session
                    //which will be used in the payment execution

                    var guid = Convert.ToString((new Random()).Next(100000));

                    //CreatePayment function gives us the payment approval url
                    //on which payer is redirected for paypal account payment

                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);

                    //get links returned from paypal in response to Create function call

                    var links = createdPayment.links.GetEnumerator();

                    string paypalRedirectUrl = null;

                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;

                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment
                            paypalRedirectUrl = lnk.href;
                        }
                    }

                    // saving the paymentID in the key guid
                    Session.Add(guid, createdPayment.id);

                    return Redirect(paypalRedirectUrl);
                }
                else
                {

                    // This function exectues after receving all parameters for the payment

                    var guid = Request.Params["guid"];

                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);

                    //If executed payment failed then we will show payment failure message to user
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("FailureView");
                    }
                }
            }
            catch (Exception ex)
            {
                return View("FailureView");
            }

            //on successful payment, show success page to user.
            return View("SuccessView");
        }


        private PayPal.Api.Payment payment;
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            this.payment = new Payment() { id = paymentId };
            return this.payment.Execute(apiContext, paymentExecution);
        }

        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            var model = (Models.Store.Item)Session["cart"];

            var order = new OrderDetails();
            //create itemlist and add item objects to it
            var itemList = new ItemList() { items = new List<Item>() };

            //Adding Item Details like name, currency, price etc
            itemList.items.Add(new Item()
            {

                currency = "ZAR",
                price = order.Price.ToString(),
                quantity = order.Quantity.ToString(),
                sku = order.ProdId.ToString(),
                name = order.Prod.ProductName

            });
            var dr = new MyBookingRoles.Models.Store.Order();
            var payer = new Payer() { payment_method = "paypal" };

            // Configure Redirect Urls here with RedirectUrls object
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };
           
            
            var oredr = new Models.Store.Order();
            // Adding Tax, shipping and Subtotal details
            var details = new Details()
            {
                
                shipping = oredr.CustomerAddress,
                subtotal = oredr.Total.ToString()

            };

            //Final amount with details
            var amount = new Amount()
            {
                currency = "ZAR",
                total = oredr.Total.ToString(),
 // Total must be equal to sum of tax, shipping and subtotal.
                details = details
            };

            var transactionList = new List<Transaction>();
            // Adding description about the transaction
            transactionList.Add(new Transaction()
            {
                description = "Transaction description",
                invoice_number = "gd-bvots-OrderNo" + order.OrderId.ToString(), //Generate an Invoice No
                amount = amount,
                item_list = itemList
            });


            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            return this.payment.Create(apiContext);
        }

        //
        //paying with card
        public ActionResult PaymentWithCard()
        {

            var model = new Models.Store.Item();
            var itemList = new ItemList() { items = new List<Item>() };

            //Adding Item Details like name, currency, price etc
            itemList.items.Add(new Item()

            {
                name = model.Pr.ProductName,
                price = model.Pr.Price.ToString(),
                quantity = model.Quantity.ToString(),
                sku = model.Pr.ToString()
            });

            //ADfress and biiling
            var order = new Models.Store.Order();
            Address pay = new Address();
            pay.city = order.City;
            pay.country_code = order.Country;
            pay.line1 = order.CustomerAddress;
           // card infor
            CreditCard bill = new CreditCard();
            bill.type = order.CreditCard;
            bill.cvv2 = order.CcType;
            bill.expire_month = order.Experation.Month;
            bill.expire_year = order.Experation.Year;
            bill.first_name = order.CustomerName;
            bill.last_name = order.LastName;
            bill.number = order.CreditCardNumber;
            bill.type = order.CcType;


            //total amount to pay
            var oe = new OrderDetails();
            Details details = new Details();
            details.shipping = order.CustomerAddress;
            details.subtotal = order.Total.ToString();

            details.tax = oe.Quantity.ToString();
            // oder Details
            Amount amount = new Amount();
            amount.total = order.Total.ToString();

            amount.details = details;

            //transaction Object
            Transaction trans = new Transaction();
            trans.amount = amount;
            trans.description = "Sale Order";
            trans.item_list = itemList;
            trans.invoice_number = "gd-bvots-OrderNo" + order.OrderId.ToString();//Generate an Invoice No

            List<Transaction> transactions = new List<Transaction>();
            transactions.Add(trans);

            FundingInstrument fundingInstrument = new FundingInstrument();
            fundingInstrument.credit_card = bill;
            List<FundingInstrument> fundings = new List<FundingInstrument>();
            fundings.Add(fundingInstrument);

            Payer payr = new Payer();
            payr.funding_instruments = fundings;
            payr.payment_method = "credit_card";

            Payment pymnt = new Payment();
            pymnt.intent = "sale";
            pymnt.payer = payr;
            pymnt.transactions = transactions;

            try
            {
                APIContext apiContext = PaypalConfiguration.GetAPIContext();
                Payment createdPayment = pymnt.Create(apiContext);
                if (createdPayment.state.ToLower() != "approved")
                {
                    return View("FailureView");
                }
            }
            catch (PayPal.PayPalException ex)
            {
                Models.Store.Logger.Log("Error:" + ex.Message);
                return View("FailureView");
            }
            return View("SuccessView");




        }
    }
}