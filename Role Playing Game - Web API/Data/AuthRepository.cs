using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RolePlayingGameWebAPI.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RolePlayingGameWebAPI.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        public AuthRepository(DataContext context,IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<ServiceResponse<string>> Login(string userName, string password)
        {
            var response = new ServiceResponse<string>();
            var User = await _context.Users.FirstOrDefaultAsync(x => x.Name.ToLower().Equals(userName.ToLower()));
            if (User == null)
            {
                response.Success = false;
                response.Message = "User Not Found";
            }
            else if (!VerifyPassword(password, User.PasswordHash, User.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Wrong Password";
            }
            else
            {
                response.Data = CreateToken(User);
            }
            return response;
        }

        public async Task<ServiceResponse<int>> Register(User user, string password)
        {
            ServiceResponse<int> response = new ServiceResponse<int>();
            if(await UserExists(user.Name))
            {
                response.Success = false;
                response.Message = "User Already Exists";
                return response;
            }
            CreateUserPasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            _context.Add(user);
            await _context.SaveChangesAsync();
            response.Data = user.ID;
            return response;
        }

        public async Task<bool> UserExists(string userName)
        {
            if (await _context.Users.AnyAsync(x => x.Name.ToLower().Equals(userName.ToLower())))
            {
                return true;
            }
            return false;
        }
        private void CreateUserPasswordHash(string password,out byte[] passwordHash,out byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                //ComputeHash will Compute the HashValue of the specified Byte Array
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        private bool VerifyPassword(string password,byte[] passwordHash,byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var ComputedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for(int i = 0; i < ComputedHash.Length; i++)
                {
                    if (ComputedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        private string CreateToken(User user)
        {
            //Gets the User Object
            //Clear Some Claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.ID.ToString()),
                new Claim(ClaimTypes.Name,user.Name),
                new Claim(ClaimTypes.Role,user.Role)
            };
            //Returns the New Instance of SymmetricSecurityKey Provided in JSON File
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("JwtToken:Token").Value));
            //SigningCredentials will Checks the Signin Using Key and HashMap Algorithm
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            //Contains some information which used to create a security token.
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };
            //Creating new JWTTokenHandler Class
            var tokenHandler = new JwtSecurityTokenHandler();
            //Create the Actual Token Using CreateToken
            var token = tokenHandler.CreateToken(tokenDescriptor);
            //Write the Token in String
            return tokenHandler.WriteToken(token);
        }
    }
}
