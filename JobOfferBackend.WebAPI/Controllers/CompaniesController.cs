using JobOfferBackend.ApplicationServices;
using JobOfferBackend.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobOfferBackend.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly CompaniesService _companiesService;

        public CompaniesController(CompaniesService companiesService)
        {
            _companiesService = companiesService;
        }

        // GET api/companies
        [HttpGet()]
        public async Task<IEnumerable<Company>> GetAll()
        {
            return await _companiesService.GetAllCompanies();
        }
    }
}
