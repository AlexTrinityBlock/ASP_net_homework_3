using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication5.Models
{
    public class DeleteData
    {
        public string ID { get; set; }

        public DeleteData() {
            ID = String.Empty;
        }

        public DeleteData(string _ID)
        {
            ID=_ID;
        }
    }
}