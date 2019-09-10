using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using cairn.Accounts.Auth;
using cairn.Constant;
using FluentEmail.Core;
using lug.Handler.Token;
using lug.Helper.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using SerilogTimings;

namespace cairn.Controllers.Auth
{
    /// <summary></summary>
    public class AuthController : CairnController
    {

        /// <summary>Default constructor</summary>
        /// <param name="configuration">Used to get informations from the configuration files</param>
        /// <param name="logger">Application logger</param>
        public AuthController(ILogger<AuthController> logger, IConfiguration configuration) : base(logger, configuration)
        {
        }

        [HttpPost]
        /// <summary>Method to log in a user</summary>
        /// <param name="userRequest">Informations on the user to log in</param>
        /// 
        public IActionResult AuthenticateUser( [FromBody] UserAuthForm userRequest)
        {
            using (Operation.Time("Authentification de l'utilisateur {id}", userRequest.Login)) 
            {
                
                RestClient client = new RestClient(configuration["API:Host"]);

                string correlationId = Guid.NewGuid().ToString();
                var request = HttpHelper.CreateBaseRequest(new HttpCriterias{
                    Uri = configuration["API:Urls:Garda:AuthUser"],
                    Method = Method.POST
                    // CorrelationId = correlationId
                });
                AuthenticateUser auth = new AuthenticateUser();
                auth.ClientId = configuration["JWT:ClientId"];
                auth.ClientSecret = configuration["JWT:ClientSecret"];
                auth.GrantType = configuration["JWT:GrantTypes:Password"];
                auth.Password = userRequest.Password;
                request.AddUrlSegment(configuration["API:Urls:Parameters:Id"], userRequest.Login);
                request.AddJsonBody(auth);
                UserAuthDTO userAuth = null;
                using (Operation.Time("Demande d'authentification de l'utilisateur {id} auprès de Garda", userRequest.Login)) 
                {
                    var response = client.Execute<UserAuthDTO>(request);
                    if (!response.StatusCode.Equals(HttpStatusCode.OK))
                    {
                        return HttpError(correlationId, request.Method, client.BuildUri(request).ToString(), (int) response.StatusCode, response.Content);
                    }
                    userAuth = JsonConvert.DeserializeObject<UserAuthDTO>(response.Content);
                    HttpContext.Session.Set(SessionConstant.JWT, userAuth);
                }
                TokenHandler handler = new TokenHandler(logger);
                JwtSecurityToken token = handler.ReadToken(userAuth.AccessToken);
                Claim accounTypeClaim = token.Payload.Claims.Single(c => c.Type == ClaimsConstant.ACCOUNT_TYPE);
                
                return Ok(accounTypeClaim.Value);
            }
        }

        ///<summary>Method to log off the user</summary>
        ///<returns> OK if the log off succeeded</returns>
        public IActionResult LogOff()
        {
            UserAuthDTO userAuth = this.HttpContext.Session.Get<UserAuthDTO>(SessionConstant.JWT);
            this.HttpContext.Session.Remove(SessionConstant.JWT);
            TokenHandler handler = new TokenHandler(logger);
            JwtSecurityToken token = handler.ReadToken(userAuth.AccessToken);
            Claim loginClaim = token.Payload.Claims.Single(c => c.Type == ClaimsConstant.LOGIN);
            logger.LogInformation("Log off for the account {1}", loginClaim.Value);

            // We revocate the token so we can't connect with it anymore
            using (Operation.Time("Révocation du  refresh token")) 
            {
                
                RestClient client = new RestClient(configuration["API:Host"]);
                string correlationId = Guid.NewGuid().ToString();
                var request = HttpHelper.CreateBaseRequest(new HttpCriterias{
                    Uri = configuration["API:Urls:Garda:Token"],
                    Method = Method.DELETE,
                    CorrelationId = correlationId
                });
                TokenRevokeRequest requestRevoke = new TokenRevokeRequest();
                requestRevoke.ClientId = configuration["JWT:ClientId"];
                requestRevoke.ClientSecret = configuration["JWT:ClientSecret"];
                requestRevoke.RefreshToken = userAuth.RefreshToken;
                request.AddJsonBody(requestRevoke);
                var response = client.Execute(request);
                if (!response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    return HttpError(correlationId, request.Method, client.BuildUri(request).ToString(), (int) response.StatusCode, response.Content);
                }
            }
            this.HttpContext.Session.Clear();
            return Ok();
        }

    }
}