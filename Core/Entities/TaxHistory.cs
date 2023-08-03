using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public enum ActionStatus
    {
        UnderReview, Approved,Rejected
    }
    public class TaxHistory
    {
        public int Id { get; set; }


        public ActionStatus Status { get; set; } 
        public DateTime Timestamp { get; set; }

        public virtual TaxReturn TaxReturn { get; set; }
        public int TaxReturnID { get; set; }
    }
}
