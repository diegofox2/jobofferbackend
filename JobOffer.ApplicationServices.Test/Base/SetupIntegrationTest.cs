using MongoDB.Driver;

namespace JobOffer.ApplicationServices.Test.Base
{
    public class SetupIntegrationTest
    {
        private readonly static MongoClient _mongoClient = new MongoClient();
        private const string TestDataBaseName = "JobOfferIntegrationTests";

        public static IMongoDatabase DatabaseTest
        {
            get
            {
                return _mongoClient.GetDatabase(TestDataBaseName);
            }
        }

        public static void OneTimeSetUp()
        {
            _mongoClient.GetDatabase(TestDataBaseName);
        }

        public static void OneTimeTearDown()
        {
            _mongoClient.DropDatabase(TestDataBaseName);
        }
    }
}
