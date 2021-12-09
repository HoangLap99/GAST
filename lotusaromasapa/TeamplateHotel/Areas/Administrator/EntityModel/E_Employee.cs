using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeamplateHotel.Areas.Administrator.EntityModel
{
    public class E_Employee
    {
        public int ID { get; set; }

        public string FullName { get; set; }

        public string  Position { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public int? Index { get; set; }
    }
}