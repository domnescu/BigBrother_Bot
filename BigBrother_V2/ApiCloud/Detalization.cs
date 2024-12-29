using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigBrother_V2.ApiCloud
{
    public class Detalization
    {
        public List<object> linked { get; set; }
        public string name { get; set; }
        public string plan { get; set; }
        public string plan_name { get; set; }
        public string price { get; set; }
        public string price_month { get; set; }
        public string region_slug { get; set; }
        public int resource_id { get; set; }
        public string state { get; set; }
        public string type { get; set; }
        public string unit { get; set; }
    }
}
