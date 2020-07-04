using JobOfferBackend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobOfferBackend.ApplicationServices.DTO
{
    public class JobOfferListDto
    {
        public JobOffer JobOffer { get; set; }

        public bool AlreadyApplied { get; set; }
    }
}
