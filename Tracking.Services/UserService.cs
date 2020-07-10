using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Tracking.Common.Extensions;
using Tracking.Common.Models;
using Tracking.Common.ViewModels;
using Tracking.DataAccess.Patterns.Factory;
using Tracking.DataAccess.Patterns.Uow;
using Tracking.Entities.Models;
using Tracking.Services.Interfaces;

namespace Tracking.Services
{
    public class UserService:IUserService
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public UserService(ILogger<UserService> logger, IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _logger = logger;
        }

        // single seed user
        private readonly List<User> _users = new List<User>
        {
            new User { Name = "Test User", Username = "test", Password = "p@s5w0rD" }
        };

        public async Task Seed()
        {
            using IUnitOfWork uow = _unitOfWorkFactory.Create();
            foreach (User item in _users)
            {
                User user = await uow.GetEntityRepository<User>().FindByAsync(q => q.Username.Equals(item.Username));
                if (user == null)
                {
                    item.Password = CryptoExtensions.Encrypt(item.Password);
                    uow.GetEntityRepository<User>().Insert(item);
                }
            }
            await uow.CommitAsync();
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model, string appSecret)
        {
            // seed default
            Task.Run(async () => await Seed()).Wait();
            //var user = _users.SingleOrDefault(u => u.Username == model.Username && u.Password == decryptedPass);
            using IUnitOfWork uow = _unitOfWorkFactory.Create();
            User user = await uow.GetEntityRepository<User>().FindByAsync(q => q.Username.Equals(model.Username));
            if (user != null)
            {
                string decryptedPass = CryptoExtensions.Decrypt(user.Password);

                if (model.Password.Equals(decryptedPass))
                {
                    // authentication successful so generate jwt token
                    string token = GenerateJwtToken(user, appSecret);

                    return new AuthenticateResponse(user, token);
                }
            }

            // return null if user not found
            return null;
        }

        public async Task<IEnumerable<UserViewModel>> GetAllAsync()
        {
            try
            {
                IEnumerable<User> users = null;
                using IUnitOfWork uow = _unitOfWorkFactory.Create();
                users = await uow.GetEntityRepository<User>().AllAsync();

                if (users == null)
                    return null;
                UserViewModel user = new UserViewModel();
                return users.Select(e => user.Map(e))
                    .OrderByDescending(o => o.Name);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                _logger.LogTrace(e.StackTrace);
            }

            return Enumerable.Empty<UserViewModel>();
        }

        // helper methods

        private string GenerateJwtToken(User user, string appSecret)
        {
            // generate token that is valid for 7 days
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(appSecret);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
