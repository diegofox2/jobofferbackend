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
        private readonly TokenInformationRepository _tokenInformationRepository;

        public AccountService(AccountRepository accountRepository, TokenInformationRepository tokenInformationRepository)
        {
            _accountRepository = accountRepository;
            _tokenInformationRepository = tokenInformationRepository;
        }

        public async Task<string> SignInAsync(string email, string password)
        {
            var account = await _accountRepository.GetAccountAsync(email, password);

            if(account != null)
            {
                
                var token = new TokenInformation() { AccountId = account.Id };
                await _tokenInformationRepository.UpsertAsync(token);
                
                //It sends a token for future operations
                return token.Id;
            }

            throw new InvalidOperationException(ServicesErrorMessages.SIGN_IN_ERROR);
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
