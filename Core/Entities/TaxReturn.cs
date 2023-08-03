namespace Core.Entities
{
    public class TaxReturn
    {
        public int Id { get; set; }

        public int TaxPayerId { get; set; } // Foreign key to TaxPayer
        public virtual TaxPayer TaxPayer { get; set; } // Navigation property to TaxPayer


        public ActionStatus Status { get; set; }    

        public DateTime FilingDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }

        // Additional properties related to tax return
        public decimal Income { get; set; }
        public decimal AdjustedGrossIncome { get; set; }
        public decimal TaxableIncome { get; set; }
        public decimal TotalTax { get; set; }
        public decimal TaxWithheld { get; set; }

        public string ForMonth { get; set; }
        public virtual List<TaxHistory> taxHistories { get; set; }
    }
}