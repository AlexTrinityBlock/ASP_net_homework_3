using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication5.Models
{
    public class Item
    {
        public string id { get; set; }      //集合物件 Item 內 編號
        public string name { get; set; }    //集合物件 Item 內 名稱
        public int number { get; set; }     //集合物件 Item 內 數量
        public Item()
        {
            id = string.Empty;              //集合物件 Item 內 項目初始值
            name = string.Empty;
            number = 0;
        }
        public Item(string _id, string _name, int _number)
        {
            id = _id;
            name = _name;
            number = _number;
        }
        public override string ToString()
        {
            return $"編號:{id}, 名稱:{name}, 數量:{number}.";
        }



    }
}