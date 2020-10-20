using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Itenso.TimePeriod;

namespace MyBookingRoles.Models.Booking
{
    public class MyTimeBlockExtention:TimeBlock
    {


        public MyTimeBlockExtention(DateTime x, TimeSpan y)
               : base(x, y)
        {

        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.Start.ToShortTimeString());
            sb.Append(" to ");
            sb.Append(this.End.ToShortTimeString());
            return sb.ToString();


        }
    }
}