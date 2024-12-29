using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigBrother_V2.ApiCloud
{
    public class BalanceData
    {
        public double balance { get; set; }
        public double bonus_balance { get; set; }
        public int days_left { get; set; }
        public List<Detalization> detalization { get; set; }
        public double hourly_cost { get; set; }
        public int hours_left { get; set; }
        public double monthly_cost { get; set; }
    }
}
