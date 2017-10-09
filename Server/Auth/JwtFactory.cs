using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Server.Helpers;

namespace Server.Auth
{
    public class JwtFactory : IJwtFactory
    {
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly ILogger _logger;

        public JwtFactory(IOptions<JwtIssuerOptions> jwtOptions, ILogger<JwtFactory> logger)
        {
            _jwtOptions = jwtOptions.Value;
            ThrowIfInvalidOptions(_jwtOptions);
            _logger = logger;
        }

        public async Task<string> GenerateEncodedToken(string clientId, ClaimsIdentity identity)
        {
            var claims = new List<Claim>
            {
                 new Claim(JwtRegisteredClaimNames.NameId, clientId),
                 new Claim(JwtRegisteredClaimNames.Sub, clientId),
                 new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator()),
                 new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
                 identity.FindFirst(Helpers.Constants.Strings.JwtClaimIdentifiers.Id)
             };
            foreach (var roleClaim in identity.FindAll(ClaimTypes.Role))
            {
                claims.Add(roleClaim);
            }

            // Create the JWT security token and encode it.
            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims.ToArray(),
                notBefore: _jwtOptions.NotBefore,
                expires: _jwtOptions.Expiration,
                signingCredentials: _jwtOptions.SigningCredentials);

            var roles = jwt.Payload[ClaimTypes.Role] as List<object>;
            var roleString = "";
            if (roles != null)
            {
                roleString = roles.Aggregate(roleString, (current, role) => current + (role as string));
            }
            _logger.LogInformation(LoggerEvents.GENERATE_JWT, $"Generate JWT "
                    + $"sub: {(string)jwt.Payload["sub"]} "
                    + $"rol: {roleString} "
                    + $"nbf: {GetDateTimeFromUnix((long)jwt.Payload["nbf"])} "
                    + $"iat: {GetDateTimeFromUnix((long)jwt.Payload["iat"])} "
                    + $"exp: {GetDateTimeFromUnix((long)jwt.Payload["exp"])}");

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return encodedJwt;
        }

        private static DateTime GetDateTimeFromUnix(long unixtime)
        {
            return DateTimeOffset.FromUnixTimeSeconds(unixtime).UtcDateTime.ToLocalTime();
        }

        public ClaimsIdentity GenerateClaimsIdentity(string userName, string id, IList<string> roles)
        {
            var claims = new List<Claim> {new Claim(Constants.Strings.JwtClaimIdentifiers.Id, id)};
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return new ClaimsIdentity(new GenericIdentity(userName, "Token"), claims);
        }

        /// <returns>Date converted to seconds since Unix epoch (Jan 1, 1970, midnight UTC).</returns>
        private static long ToUnixEpochDate(DateTime date)
          => (long)Math.Round((date.ToUniversalTime() -
                               new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                              .TotalSeconds);

        private static void ThrowIfInvalidOptions(JwtIssuerOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            if (options.ValidFor <= TimeSpan.Zero)
            {
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwtIssuerOptions.ValidFor));
            }

            if (options.SigningCredentials == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.SigningCredentials));
            }

            if (options.JtiGenerator == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.JtiGenerator));
            }
        }
    }
}