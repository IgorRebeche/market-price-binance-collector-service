using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events
{
    public interface ITickerCollected
    {
        public string Price { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
