using JobOfferBackend.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobOfferBackend.Doman.Security.Entities
{
    public class TokenInformation : BaseEntity<TokenInformation>
    {
        public string AccountId { get; set; }

        public override void Validate()
        {
            throw new NotImplementedException();
        }
    }
}
