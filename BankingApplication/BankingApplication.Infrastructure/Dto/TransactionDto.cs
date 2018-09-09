using BankingApplication.Infrastructure.Entity;

namespace BankingApplication.Infrastructure.Dto
{
    public class TransactionDto : BaseDto
    {
        public TransactionDto() : base()
        {

        }
        public Transaction Transaction { get; set; }
    }
}
