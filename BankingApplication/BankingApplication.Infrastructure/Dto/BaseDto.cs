using System.Collections.Generic;

namespace BankingApplication.Infrastructure.Dto
{
    public abstract class BaseDto
    {
        public BaseDto()
        {
            Errors = new Dictionary<string, string>();
        }

        public Dictionary<string, string> Errors { get; set; }
    }
}
