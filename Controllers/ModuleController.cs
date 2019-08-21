using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using cairn.Accounts.Auth;
using cairn.Constant;
using cairn.Helper.Token;
using cairn.Modules.Datas;
using lug.Helper.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using SerilogTimings;

namespace cairn.Controllers.Modules
{
    /// <summary>Controller to manage the accounts</summary>
    public class ModuleController : CairnController
    {

        /// <summary>default constructor</summary>
        /// <param name="configuration">Used to get informations in the configuration files</param>
        /// <param name="logger">Application logger</param>
        public ModuleController(ILogger<ModuleController> logger, IConfiguration configuration) : base(logger, configuration)
        {
        }

        /// <summary>Search from the modules</summary>
        /// <returns>IActionResult : Ok with the list of modules</returns>
        public IActionResult SearchModules([FromBody] ListModulesRequest searchCriterias)
        {
                string correlationId = Guid.NewGuid().ToString();

                UserAuthDTO userAuth = this.HttpContext.Session.Get<UserAuthDTO>(SessionConstant.JWT);

                // Validation of the access token, to see if we have to regenerate it
                JwtSecurityToken token = TokenHelper.ValidateUserAuth(this.HttpContext.Session, configuration, userAuth, logger);
                if (token == null)
                {
                    throw new UnauthorizedAccessException("Le jeton d'accès est nul. Merci de vous authentifier.");
                }
                userAuth = this.HttpContext.Session.Get<UserAuthDTO>(SessionConstant.JWT);

                RestClient client = new RestClient(configuration["API:Host"]);
                RestRequest request = HttpHelper.CreateBaseRequest(new HttpCriterias{
                    Uri = configuration["API:Urls:Module:List"],
                    Method = Method.POST,
                    AccessToken = userAuth.AccessToken,
                    CorrelationId = correlationId
                });
                request.AddJsonBody(searchCriterias);

                IList<Module> modules = new List<Module>();
                using (Operation.Time("Liste des modules")) 
                {
                    IRestResponse response = client.Execute<List<Module>>(request);
                    if (!response.StatusCode.Equals(HttpStatusCode.OK))
                    {
                        return HttpError(correlationId, request.Method, client.BuildUri(request).ToString(), (int) response.StatusCode, response.Content);
                    } else 
                    {
                        modules = JsonConvert.DeserializeObject<List<Module>>(response.Content);
                    }
                }
           
            return Ok(modules);
        }

    }
}