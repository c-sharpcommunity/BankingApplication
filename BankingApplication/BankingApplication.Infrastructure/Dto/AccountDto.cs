using BankingApplication.Infrastructure.Entity;

namespace BankingApplication.Infrastructure.Dto
{
    public class AccountDto : BaseDto
    {
        public AccountDto() : base()
        {

        }

        public Account Account { get; set; }
    }
}
