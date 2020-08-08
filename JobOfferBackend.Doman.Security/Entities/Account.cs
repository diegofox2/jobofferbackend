using JobOfferBackend.Domain.Common;
using JobOfferBackend.Domain.Security.Constants;

namespace JobOfferBackend.Doman.Security.Entities
{
    public class Account : BaseEntity<Account>
    {
        public string PersonId { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }


        public override void Validate()
        {
            if (string.IsNullOrEmpty(Email))
                _errorLines.AppendLine(DomainErrorMessages.EMAIL_CANT_BE_EMPTY);

            if (string.IsNullOrEmpty(Password))
                _errorLines.AppendLine(DomainErrorMessages.PASSWORD_CANT_BE_EMPTY);

            ThrowExceptionIfErrors();
        }
    }
}
