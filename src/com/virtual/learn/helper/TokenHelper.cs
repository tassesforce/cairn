using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using cairn.Accounts.Auth;
using cairn.Constant;
using cairn.Models.Auth.Token;
using lug.Handler.Token;
using lug.Helper.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using SerilogTimings;

namespace cairn.Helper.Token
{
    /// <summary>Helper to manage the use of tokens</summary>
    internal class TokenHelper
    {
        /// <summary>Validate the access token, with refresh if needed</summary>
        /// <param name="userAuth">Container of the tokens</param>
        /// <returns>JwtSecurityToken</returns>
        public static JwtSecurityToken ValidateUserAuth(ISession session, IConfiguration configuration, UserAuthDTO userAuth, ILogger logger)
        {
            if (userAuth != null)
            {
                TokenHandler handler = new TokenHandler(logger);
                JwtSecurityToken token = handler.ReadToken(userAuth.AccessToken);
                Claim expClaim = token.Payload.Claims.Single(c => c.Type == ClaimsConstant.EXPIRE);
                Claim userIdClaim = token.Payload.Claims.Single(c => c.Type == ClaimsConstant.USER_ID);

                TimeSpan t = DateTime.UtcNow - SessionConstant.EPOCH;

                if ((int)t.TotalSeconds >= int.Parse(expClaim.Value))
                {
                    RestClient client = new RestClient(configuration["API:Host"]);
                    RestRequest request = HttpHelper.CreateBaseRequest(new HttpCriterias{
                        Uri = configuration["API:Urls:Garda:Token"],
                        Method = Method.POST
                    });
                    request.AddJsonBody(CreateTokenRequest(configuration, userAuth.RefreshToken, userAuth.AccessToken, userIdClaim.Value));
                    
                    using (Operation.Time("Refresh du token")) 
                    {
                        IRestResponse response = client.Execute<TokenResponse>(request);
                        if (response.StatusCode.Equals(HttpStatusCode.OK))
                        {
                            TokenResponse tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(response.Content);
                            UserAuthDTO refreshedUserAuth = new UserAuthDTO(userAuth);
                            refreshedUserAuth.AccessToken = tokenResponse.AccessToken;
                            session.Set(SessionConstant.JWT, refreshedUserAuth);
                        } else {
                            throw new UnauthorizedAccessException("Le jeton de raffraichissement demandé est révoqué");
                        }
                    }
                }
                return token;
            }
            return null;
        }

        /// <summary>Create the request to refresh the access token</summary>
        /// <param name="refreshToken">refreshToken to use</param>
        /// <param name="userId">Identifier of the account</param>
        /// <returns>TokenRequest</returns>
        private static TokenRequest CreateTokenRequest(IConfiguration configuration, string refreshToken, string accessToken, string userId)
        {
            var tokenRequest = new TokenRequest{
                RefreshToken = refreshToken,
                AccessToken = accessToken,
                GrantType = configuration["JWT:GrantTypes:RefreshToken"],
                UserId = userId,
                ClientId = configuration["JWT:ClientId"],
                ClientSecret = configuration["JWT:ClientSecret"]
            };
            return tokenRequest;
        }
    }
}