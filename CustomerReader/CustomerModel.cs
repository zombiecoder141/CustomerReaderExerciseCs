using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerReader
{
    public class Address
    {
        public string streetAddress;
        public string city;
        public string state;
        public string zipCode;
    }

    public class Customer : Address
    {
        public string fn;
        public string ln;
        public string email;
        public string phone;
        public string GetFormattedName()
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(this.fn.ToLower()) + " " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(this.ln.ToLower()); 
        }
    }
}
