using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace PoliceWebApplication.Models
{
    public class CustomUserValidator : IUserValidator<User>
    {
        public interface IUserValidator<TUser> where TUser : class
        {
            Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user);
        }

        public Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user)
        {
            List<IdentityError> errors = new List<IdentityError>();

            if (!(user.Email.ToLower().EndsWith("@police.gov.ua")))
            {
                errors.Add(new IdentityError
                {
                    Description = "Будь ласка, введіть пошту домену @police.gov.ua"
                });
            }
            return Task.FromResult(errors.Count == 0 ?
                IdentityResult.Success : IdentityResult.Failed(errors.ToArray()));
        }
    }
}
