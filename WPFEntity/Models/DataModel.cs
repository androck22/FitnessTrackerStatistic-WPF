using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFEntity.Models
{
    public class DataModel
    {
        public DataModel(double value, int label)
        {
            this.Value = value;
            this.Label = label;
        }

        public double Value { get; set; }
        public int Label { get; set; }
    }
}
