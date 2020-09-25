using JobOfferBackend.ApplicationServices;
using JobOfferBackend.DataAccess;
using JobOfferBackend.Domain.Entities;
using JobOfferBackend.Doman.Security.Entities;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace InitialDataCreator
{
    internal class Program
    {
        private static void Main(string[] args)
        {
           
            var mongoClient = new MongoClient();
            mongoClient.DropDatabase("JobOfferDatabase");
            var database = mongoClient.GetDatabase("JobOfferDatabase");

            var cSharp = new Skill() { Name = "C#", Id = Guid.NewGuid().ToString() };
            var javascript = new Skill() { Name = "Javascript", Id = Guid.NewGuid().ToString() };
            var react = new Skill() { Name = "React", Id = Guid.NewGuid().ToString() };
            var docker = new Skill() { Name = "Docker", Id = Guid.NewGuid().ToString() };
            var java = new Skill() { Name = "Java", Id = Guid.NewGuid().ToString() };

            var skillRepository = new SkillRepository(database);

            Task.Run(async () => await skillRepository.UpsertAsync(cSharp));
            Task.Run(async () => await skillRepository.UpsertAsync(javascript));
            Task.Run(async () => await skillRepository.UpsertAsync(react));
            Task.Run(async () => await skillRepository.UpsertAsync(docker));
            Task.Run(async () => await skillRepository.UpsertAsync(java));

            var companyRepository = new CompanyRepository(database);
            var recruiterRepository = new RecruiterRepository(database);
            var jobOfferRepository = new JobOfferRepository(database);
            var personRepository = new PersonRepository(database);
            var accountRepository = new AccountRepository(database);

            var recruiterService = new RecruiterService(companyRepository, recruiterRepository, jobOfferRepository, personRepository, accountRepository);

            var recruiter = new Recruiter();
            recruiter.AddClient(new Company("Acme", "Software"));

            recruiter.IdentityCard = "28.999.999";
            recruiter.FirstName = "Patricia";
            recruiter.LastName = "Maidana";
            recruiter.SetStudy(new Study("UBA", "Lic.RRHH", StudyStatus.Completed));
            recruiter.SetPreviousJob(new Job("Coto", "HR Analyst", DateTime.Now.AddYears(-6), true));

            recruiter.SetAbility(new Ability(javascript, 9));

            var jobOffer = new JobOffer() { Date = DateTime.Now.Date, Title = "Analista programador", Company = new Company("Acme", "software"), Zone = "Palermo" };

            jobOffer.Description = "Para importante empresa ubicada en San Telmo, estamos en búsqueda de desarrollador fullstack con " +
                "al menos 3 años de experiencia utilizando React y NodeJs.Quien se incorpore estará participando dentro " +
                "de un proyecto de inteligencia artifical";

            jobOffer.AddSkillRequired(new SkillRequired(javascript, 5, true));
            jobOffer.AddSkillRequired(new SkillRequired(react, 3));

            jobOffer.Language = "Ingles";
            jobOffer.LanguageLevel = LanguageLevel.Advance;
            jobOffer.IsLanguageMandatory = true;

            jobOffer.ContractInformation = new ContractCondition()
            {
                StartingFrom = "Inmediata",
                KindOfContract = "Relación de dependencia",
                WorkingDays = "Lunes a viernes 9 a 18"
            };

            recruiterService.CreateRecruiterAsync(recruiter).Wait();

            recruiterService.CreateJobOfferAsync(jobOffer, recruiter.Id).Wait();

            //

            var jobOffer2 = new JobOffer() { Date = DateTime.Now.Date, Title = "JAVA Full Stack Developer", Company = new Company("KaizenRH", "software"), Zone = "Las Cañitas" };

            jobOffer2.Description = "En KaizenRH buscamos Python Developer Junior para trabajar en interesantes proyectos dentro de Startup en expansión LATAM dedicada a la automatización de procesos IT y negocios.";

            jobOffer2.AddSkillRequired(new SkillRequired(javascript, 5, true));
            jobOffer2.AddSkillRequired(new SkillRequired(react, 3));
            jobOffer2.AddSkillRequired(new SkillRequired(java, 6, true));

            jobOffer2.Language = "Ingles";
            jobOffer2.LanguageLevel = LanguageLevel.Intermediate;

            jobOffer2.ContractInformation = new ContractCondition()
            {
                StartingFrom = "Inmediata",
                KindOfContract = "Relación de dependencia",
                WorkingDays = "Lunes a viernes 9 a 18"
            };

            recruiterService.CreateJobOfferAsync(jobOffer2, recruiter.Id).Wait();

            //

            var jobOffer3 = new JobOffer() { Date = DateTime.Now.Date, Title = "Sr. C# Backend Developer/Engineer", Company = new Company("ADN Recursos Humanos", "Seleccion de personal"), Zone = "Microcentro" };

            jobOffer3.Description = "ADN - Recursos Humanos estamos en la búsqueda de un Sr. Python Backend Developer/Engineer, para Importante Empresa de Tecnología";

            jobOffer3.AddSkillRequired(new SkillRequired(cSharp, 5));
            jobOffer3.AddSkillRequired(new SkillRequired(javascript, 2, true));

            jobOffer3.ContractInformation = new ContractCondition()
            {
                StartingFrom = "Inmediata",
                KindOfContract = "Relación de dependencia",
                WorkingDays = "Lunes a viernes 9 a 18"
            };

            recruiterService.CreateJobOfferAsync(jobOffer3, recruiter.Id).Wait();

            
            recruiterService.PublishJobOffer(jobOffer2).Wait();
            recruiterService.PublishJobOffer(jobOffer3).Wait();
            recruiterService.FinishJobOffer(jobOffer3).Wait();

            var pRepo = new RecruiterRepository(database);
            var aRepo = new AccountRepository(database);

            var person = pRepo.GetByIdentityCardAsync("28.999.999").Result;

            var account = new Account() { Id = Guid.NewGuid().ToString(), PersonId = person.Id, Email = "a@b.com", Password = "password", IsRecruiter = true };

            aRepo.UpsertAsync(account).Wait();

            Console.WriteLine("Agregado correctamente!");
            Console.ReadKey();
        }
    }
}