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

        public async Task<Account> SignInAsync(string email, string password)
        {
            return await _accountRepository.GetAccountAsync(email, password);
        }

        public async Task SignUpAsync(string email, string password, string confirmedPassword, bool isRecruiter)
        {
            if(password != confirmedPassword)
            {
                throw new InvalidOperationException(ServicesErrorMessages.PASSWORD_CONFIRMATION_DOES_NOT_MATCH_WITH_PASSWORD);
            }

            

            if(await _accountRepository.AccountEmailAlreadyExists(email))
            {
                throw new InvalidOperationException(ServicesErrorMessages.INVALID_USER_ACCOUNT);
            }

            var account = new Account() { Email = email, Password = password, IsRecruiter = isRecruiter };

            await _accountRepository.UpsertAsync(account);
        }
    }
}
