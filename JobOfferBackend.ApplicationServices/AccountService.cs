using JobOfferBackend.ApplicationServices.Constants;
using JobOfferBackend.DataAccess;
using JobOfferBackend.Doman.Security.Entities;
using System;
using System.Threading.Tasks;

namespace JobOfferBackend.ApplicationServices
{
    public class AccountService
    {
        private readonly AccountRepository _accountRepository;

        public AccountService(AccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<string> SignInAsync(string email, string password)
        {
            var account = await _accountRepository.GetAccountAsync(email, password);

            return account?.Id;
        }

        public async Task SignUpAsync(string email, string password, string confirmedPassword)
        {
            if(password != confirmedPassword)
            {
                throw new InvalidOperationException(ServicesErrorMessages.PASSWORD_CONFIRMATION_DOES_NOT_MATCH_WITH_PASSWORD);
            }

            var account = new Account() { Email = email, Password = password };

            await _accountRepository.UpsertAsync(account);
        }
    }
}
