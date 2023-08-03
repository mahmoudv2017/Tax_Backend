using Core.Entities;

namespace Api.Dtos
{
    public class TaxReturnDto
    {
        public decimal Income { get; set; }
        public decimal AdjustedGrossIncome { get; set; }
        public decimal TaxableIncome { get; set; }
        public decimal TotalTax { get; set; }
        public string ForMonth { get; set; }
        public decimal TaxWithheld { get; set; }

    }

    public class TaxReturnDtoReponse
    {
        public decimal Income { get; set; }
        public DateTime FilingDate { get; set; }
        public decimal AdjustedGrossIncome { get; set; }
        public decimal TaxableIncome { get; set; }
        public decimal TotalTax { get; set; }
        public string ForMonth { get; set; }

        public ActionStatus Status { get; set; }
        public decimal TaxWithheld { get; set; }
        public int Id { get; set; }

    }
}
