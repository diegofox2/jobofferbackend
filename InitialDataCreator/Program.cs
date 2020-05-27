using JobOfferBackend.ApplicationServices;
using JobOfferBackend.DataAccess;
using JobOfferBackend.Domain.Entities;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace InitialDataCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Menu:");
            Console.WriteLine("1 - Crear skills de tecnología");
            Console.WriteLine("2 - Crear ofertas de trabajo y 1 recruiter (patricia maidana, password 123)");
            var option = int.Parse(Console.ReadLine());

            var mongoClient = new MongoClient();
            var database = mongoClient.GetDatabase("JobOfferDatabase");

            var cSharp = new Skill() { Description = "Lenguaje de programación C#", Name = "C#" };
            var javascript = new Skill() { Description = "Lenguaje de programación C#", Name = "Javascript" };
            var react = new Skill() { Description = "Lenguaje de programación C#", Name = "React" };
            var docker = new Skill() { Description = "Lenguaje de programación C#", Name = "Docker" };

            if (option == 1)
            {

                var skillRepository = new SkillRepository(database);

                Task.Run(async () => await skillRepository.UpsertAsync(cSharp));
                Task.Run(async () => await skillRepository.UpsertAsync(javascript));
                Task.Run(async () => await skillRepository.UpsertAsync(react));
                Task.Run(async () => await skillRepository.UpsertAsync(docker));

                Console.WriteLine("Agregado correctamente!");
                Console.ReadKey();
            }
            else
            {
                var companyRepository = new CompanyRepository(database);
                var recruiterRepository = new RecruiterRepository(database);
                var jobOfferRepository = new JobOfferRepository(database);

                var recruiterService = new RecruiterService(companyRepository, recruiterRepository, jobOfferRepository);

                var recruiter = new Recruiter();
                recruiter.AddClientCompany(new Company("Acme", "Software"));

                recruiter.IdentityCard = "28.999.999";
                recruiter.FirstName = "Patricia";
                recruiter.LastName = "Maidana";
                recruiter.SetStudy(new Study("UBA", "Lic.RRHH", StudyStatus.Completed));
                recruiter.SetPreviousJob(new Job("Coto", "HR Analyst", DateTime.Now.AddYears(-6), true));

                recruiterService.CreateRecruiterAsync(recruiter);


                var jobOffer = new JobOffer() { Date = DateTime.Now, Title = "Analista programador",Company = new Company("Acme", "software")};

                jobOffer.Description = "Para importante empresa ubicada en San Telmo, estamos en búsqueda de desarrollador fullstack con " +
                    "al menos 3 años de experiencia utilizando React y NodeJs.Quien se incorpore estará participando dentro " +
                    "de un proyecto de inteligencia artifical";

                jobOffer.AddSkillRequired(new SkillRequired(javascript, 5, true));
                jobOffer.AddSkillRequired(new SkillRequired(react, 3));

                recruiterService.CreateJobOfferAsync(jobOffer, recruiter.Id);

                Console.WriteLine("Agregado correctamente!");
                Console.ReadKey();
            }
        }
    }
}
