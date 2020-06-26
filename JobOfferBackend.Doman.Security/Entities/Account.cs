﻿using JobOfferBackend.Domain.Common;
using JobOfferBackend.Domain.Security.Constants;

namespace JobOfferBackend.Doman.Security.Entities
{
    public class Account : BaseEntity<Account>
    {
        public string Personid { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }


        public override void Validate()
        {
            if (string.IsNullOrEmpty(Email))
                _errors.Append(DomainErrorMessages.EMAIL_CANT_BE_EMPTY);

            if (string.IsNullOrEmpty(Password))
                _errors.Append(DomainErrorMessages.PASSWORD_CANT_BE_EMPTY);
        }
    }
}