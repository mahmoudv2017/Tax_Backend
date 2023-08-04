using Core.Entities;

namespace Api.Dtos
{
    public class TaxHistoryDtoReponse
    {
        public int Id { get; set; }
        public ActionStatus Status { get; set; }
        public DateTime Timestamp { get; set; }

        
    }
}
