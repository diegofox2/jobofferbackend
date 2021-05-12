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

            var cSharp = new Skill() { Name = "C#"};
            var javascript = new Skill() { Name = "Javascript" };
            var react = new Skill() { Name = "React" };
            var docker = new Skill() { Name = "Docker" };
            var java = new Skill() { Name = "Java" };

            var skillRepository = new SkillRepository(database);

            Task.Run(async () => await skillRepository.UpsertAsync(cSharp)).Wait();
            Task.Run(async () => await skillRepository.UpsertAsync(javascript)).Wait();
            Task.Run(async () => await skillRepository.UpsertAsync(react)).Wait();
            Task.Run(async () => await skillRepository.UpsertAsync(docker)).Wait();
            Task.Run(async () => await skillRepository.UpsertAsync(java)).Wait();

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

            var company1 = new Company("Acme", "software");

            companyRepository.UpsertAsync(company1).Wait();

            var jobOffer = new JobOffer() { Date = DateTime.Now.Date, Title = "Analista programador", CompanyId = company1.Id, Zone = "Palermo" };

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

            recruiterService.SaveJobOfferAsync(jobOffer, recruiter.Id).Wait();

            //

            var company2 = new Company("KaizenRH", "software");

            companyRepository.UpsertAsync(company2).Wait();

            var jobOffer2 = new JobOffer() { Date = DateTime.Now.Date, Title = "JAVA Full Stack Developer", CompanyId = company2.Id, Zone = "Las Cañitas" };

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

            recruiterService.SaveJobOfferAsync(jobOffer2, recruiter.Id).Wait();

            //

            var company3 = new Company("ADN Recursos Humanos", "Seleccion de personal");

            companyRepository.UpsertAsync(company3).Wait();

            var jobOffer3 = new JobOffer() { Date = DateTime.Now.Date, Title = "Sr. C# Backend Developer/Engineer", CompanyId = company3.Id, Zone = "Microcentro" };

            jobOffer3.Description = "ADN - Recursos Humanos estamos en la búsqueda de un Sr. Python Backend Developer/Engineer, para Importante Empresa de Tecnología";

            jobOffer3.AddSkillRequired(new SkillRequired(cSharp, 5));
            jobOffer3.AddSkillRequired(new SkillRequired(javascript, 2, true));

            jobOffer3.ContractInformation = new ContractCondition()
            {
                StartingFrom = "Inmediata",
                KindOfContract = "Relación de dependencia",
                WorkingDays = "Lunes a viernes 9 a 18"
            };

            recruiterService.SaveJobOfferAsync(jobOffer3, recruiter.Id).Wait();

            
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