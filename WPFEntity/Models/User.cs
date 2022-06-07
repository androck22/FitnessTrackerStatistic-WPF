using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFEntity.Models
{
    public class User
    {
        public int Rank { get; set; }
        public string Name { get; set; }
        public int Day { get; set; }
        public int Steps { get; set; }
        public string Status { get; set; }
    }
}
