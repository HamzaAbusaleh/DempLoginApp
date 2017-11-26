using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoginAppDemo.Models.ViewModel
{
    public class AddressModel
    {
        public string Addressline1 { get; set; }
        public string Addressline2 { get; set; }
        public string ZIP { get; set; }
        public string Place { get; set; }
        public string CountryCode { get; set; }
    }
}