using JobOfferBackend.Domain.Entities;

namespace JobOfferBackend.ApplicationServices.DTO
{
    public class JobOfferListDto
    {
        public JobOffer JobOffer { get; set; }

        public bool AlreadyApplied { get; set; }
    }
}
