using JobOfferBackend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobOfferBackend.WebAPI.Dto
{
    public class UpdateJobOfferDto
    {
        public JobOffer NewJobOffer { get; set; }

        public JobOffer PreviousJobOffer { get; set; }
    }
}
