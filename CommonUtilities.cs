using MessingSystem.Domain;
using MessingSystem.Enums;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MessingSystem
{
    public static class CommonUtilities
    {
        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public static string GenereateJsonWebToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Startup.StaticConfig["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Sid, user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Email),
            };

            var token = new JwtSecurityToken(
                issuer: Startup.StaticConfig["Jwt:Issuer"],
                audience: Startup.StaticConfig["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddHours(24),
                signingCredentials: credentials
            );

            var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodedToken;
        }

        public static bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)); // Create hash using password salt.
                for (int i = 0; i < computedHash.Length; i++)
                { // Loop through the byte array
                    if (computedHash[i] != passwordHash[i]) return false; // if mismatch
                }
            }
            return true; //if no mismatches.
        }

        public static bool IsMealStatusChangeValid(DateTime date, int mealType)
        {
            var mealDate = new DateTime(date.Year, date.Month, date.Day, 0, 0, 1);

            if (mealType == (int)MealTypes.BreakFast)
            {
                var breakFastTimeOnDate = mealDate.AddHours(8);
                var timeSpan = breakFastTimeOnDate.Subtract(DateTime.Now);

                return timeSpan.TotalMinutes > (8 * 60);
            }
            else if (mealType == (int)MealTypes.Lunch)
            {
                var breakFastTimeOnDate = mealDate.AddHours(14);
                var timeSpan = breakFastTimeOnDate.Subtract(DateTime.Now);

                return timeSpan.TotalMinutes > (6 * 60);
            }
            else if (mealType == (int)MealTypes.TeaBreak)
            {
                var breakFastTimeOnDate = mealDate.AddHours(18);
                var timeSpan = breakFastTimeOnDate.Subtract(DateTime.Now);

                return timeSpan.TotalMinutes > (10 * 60);
            }
            else
            {
                var breakFastTimeOnDate = mealDate.AddHours(20);
                var timeSpan = breakFastTimeOnDate.Subtract(DateTime.Now);

                return timeSpan.TotalMinutes > (6 * 60);
            }
        }

    }
}
