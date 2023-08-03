using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class TaxPayer
    {
        public int Id { get; set; }


        public virtual User User { get; set; }
        public string UserID { get; set; }
        public virtual ICollection<TaxReturn> TaxReturns { get; set; }
    }
}
