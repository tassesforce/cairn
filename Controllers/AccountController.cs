using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using cairn.Accounts.Auth;
using cairn.Accounts.Collaborator;
using cairn.Constant;
using cairn.Enum;
using cairn.Exceptions;
using cairn.Helper.Token;
using cairn.Models.Accounts;
using cairn.Models.Auth;
using FluentEmail.Core;
using lug.Enum;
using lug.Handler.Token;
using lug.Helper.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using SerilogTimings;

namespace cairn.Controllers.Accounts
{
    /// <summary>Controller to manage the accounts</summary>
    public class AccountController : CairnController
    {

        /// <summary>Used to notify by mail</summary>
        private readonly IFluentEmail email;

        /// <summary>default constructor</summary>
        /// <param name="configuration">Used to get informations in the configuration files</param>
        /// <param name="email">Used to notify by mail</param>
        /// <param name="logger">Application logger</param>
        public AccountController(ILogger<AccountController> logger, IConfiguration configuration, IFluentEmail email) : base(logger, configuration)
        {
            this.email = email;
        }

        /// <summary>Add an account on Garda and store it in the session</summary>
        /// <param name="client">Http client</param>
        /// <param name="correlationId">CorrelationId of the process</param>
        /// <param name="userForm">Content of the form filled by the user</param>
        /// <returns>IActionResult : In case of an error, returns the IActionResult corresponding, null if alright</returns>
        private IActionResult AddUserAuth(RestClient client, string correlationId, AddUserAuthForm userForm)
        {
            logger.LogInformation("uri : " + configuration["API:Urls:Garda:Auth"]);
            logger.LogInformation("base url : " + client.BaseUrl);
            RestRequest requestAuth = HttpHelper.CreateBaseRequest(new HttpCriterias{
                Uri = configuration["API:Urls:Garda:Auth"],
                Method = Method.POST,
                CorrelationId = correlationId
            });

            AuthenticateUserCreation auth = new AuthenticateUserCreation();
            auth.ClientId = configuration["JWT:ClientId"];
            auth.ClientSecret = configuration["JWT:ClientSecret"];
            auth.AccountType = userForm.AccountType;
            auth.Contact = new ContactUserAuth {
                MailAdress = userForm.MailAdress,
                PhoneNumber = userForm.PhoneNumber
            };
            auth.Login = userForm.Auth.Login;
            auth.Password = userForm.Auth.Password;
            auth.Roles = configuration["Roles"];
            requestAuth.AddJsonBody(auth);
            
            using (Operation.Time("Ajout du compte {id} dans Garda", auth.Login)) 
            {
                IRestResponse response = client.Execute<UserAuthDTO>(requestAuth);
                if (!response.StatusCode.Equals(HttpStatusCode.Created))
                {
                    return HttpError(correlationId, requestAuth.Method, client.BuildUri(requestAuth).ToString(), (int) response.StatusCode, response.Content);
                }
                UserAuthDTO userAuth = JsonConvert.DeserializeObject<UserAuthDTO>(response.Content);
                this.HttpContext.Session.Set(SessionConstant.JWT, userAuth);
            }
            // nothing unexpected arrived, go on
            return null;
        }


        /// <summary>Add an account on Garda</summary>
        /// <param name="client">Http client</param>
        /// <param name="correlationId">CorrelationId of the process</param>
        /// <param name="userForm">Content of the form filled by the user</param>
        /// <returns>IActionResult : In case of an error, returns the IActionResult corresponding, null if alright</returns>
        private UserAuthDTO AddThirdPartyUserAuth(RestClient client, string correlationId, AddUserAuthForm userForm)
        {

            RestRequest requestAuth = HttpHelper.CreateBaseRequest(new HttpCriterias{
                Uri = configuration["API:Urls:Garda:Auth"],
                Method = Method.POST,
                CorrelationId = correlationId
            });

            AuthenticateUserCreation auth = new AuthenticateUserCreation();
            auth.ClientId = configuration["JWT:ClientId"];
            auth.ClientSecret = configuration["JWT:ClientSecret"];
            auth.AccountType = userForm.AccountType;
            auth.Contact = new ContactUserAuth {
                MailAdress = userForm.MailAdress,
                PhoneNumber = userForm.PhoneNumber
            };
            auth.Login = userForm.Auth.Login;
            auth.Password = userForm.Auth.Password;
            auth.Roles = configuration["Roles"];
            auth.AccountType = AccountTypesEnum.COLLABORATOR.Value;
            requestAuth.AddJsonBody(auth);
            
            using (Operation.Time("Ajout du compte {id} dans Garda", auth.Login)) 
            {
                IRestResponse response = client.Execute<UserAuthDTO>(requestAuth);
                if (!response.StatusCode.Equals(HttpStatusCode.Created))
                {
                    HttpError(correlationId, requestAuth.Method, client.BuildUri(requestAuth).ToString(), (int) response.StatusCode, response.Content);
                }
                return JsonConvert.DeserializeObject<UserAuthDTO>(response.Content);
            }
        }


        /// <summary> Create an Agency account (moral)</summary>
        /// <param name="client">HTTP connection client</param>
        /// <param name="request">HTTP request</param>
        /// <param name="userAuth">Associated account on Garda</param>
        /// <param name="correlationId">Process correlationId</param>
        /// <returns>IRestResponse : HTTP response</returns>
        public IActionResult CreateAgencyAccount([FromBody] AddUserAuthAgencyForm userForm)
        {
            
            string correlationId = Guid.NewGuid().ToString();
            RestClient client = new RestClient(configuration["API:Host"]);
            IActionResult result = AddUserAuth(client, correlationId, userForm);
            // In case of an error during the request, AddUserAuth returns informations about the error, so we transmit it to the caller
            if (result != null)
            {
                return result;
            }
            UserAuthDTO userAuth = this.HttpContext.Session.Get<UserAuthDTO>(SessionConstant.JWT);

            AddUserAuthAgencyForm agencyForm = new AddUserAuthAgencyForm(userForm);
            RestRequest request = HttpHelper.CreateBaseRequest(new HttpCriterias{
                    Uri = configuration["API:Urls:Compte:Agency"],
                    Method = Method.POST,
                    AccessToken = userAuth.AccessToken,
                    CorrelationId = correlationId
                });
            request.AddUrlSegment(configuration["API:Urls:Parameters:Id"], userForm.Auth.Login);

            var handler = new TokenHandler(logger);
            var token = handler.ReadToken(userAuth.AccessToken);
            var userIdClaim = token.Payload.Claims.Single(c => c.Type == ClaimsConstant.USER_ID);
            agencyForm.Model.UserId = userIdClaim.Value;
            StringEnum preferredContactEnum = new PreferredContactEnum();
            // TODO add the preferred contact type
            request.AddJsonBody(agencyForm.Model);
            // request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            using (Operation.Time("Creation du compte Agence {id}", userForm.Auth.Login)) 
            {
                IRestResponse response = client.Execute(request);
                if (response.StatusCode.Equals(HttpStatusCode.Created))
                {
                    NotifyLIVE();
                    this.HttpContext.Session.Set(SessionConstant.ACCOUNT, JsonConvert.DeserializeObject<AgencyAccountModel>(response.Content));
                    return Ok();
                } else 
                {
                    DeleteUserAuth(userAuth, correlationId);
                    return HttpError(correlationId, request.Method, client.BuildUri(request).ToString(), (int) response.StatusCode, response.Content);
                }
            }
        }

        /// <summary>Create a company account (moral)</summary>
        /// <param name="client">HTTP Connection client</param>
        /// <param name="request">HTTP request</param>
        /// <param name="userAuth">Associated Garda account</param>
        /// <param name="correlationId">Process correlationId</param>
        /// <returns>IRestResponse : HTTP response</returns>
        public IActionResult CreateCompanyAccount([FromBody] AddUserAuthCompanyForm userForm)
        {

            string correlationId = Guid.NewGuid().ToString();
            RestClient client = new RestClient(configuration["API:Host"]);
            IActionResult result = AddUserAuth(client, correlationId, userForm);
            // In case of an error during the request, AddUserAuth returns informations about the error, so we transmit it to the caller
            if (result != null)
            {
                return result;
            }
            UserAuthDTO userAuth = this.HttpContext.Session.Get<UserAuthDTO>(SessionConstant.JWT);

            AddUserAuthCompanyForm companyForm = new AddUserAuthCompanyForm(userForm);
            IRestRequest request = HttpHelper.CreateBaseRequest(new HttpCriterias{
                    Uri = configuration["API:Urls:Compte:Company"],
                    Method = Method.POST,
                    AccessToken = userAuth.AccessToken,
                    CorrelationId = correlationId
                });
            request.AddUrlSegment(configuration["API:Urls:Parameters:Id"], userForm.Auth.Login);
            var handler = new TokenHandler(logger);
            var token = handler.ReadToken(userAuth.AccessToken);
            var userIdClaim = token.Payload.Claims.Single(c => c.Type == ClaimsConstant.USER_ID);
            companyForm.Model.UserId = userIdClaim.Value;
            StringEnum preferredContactEnum = new PreferredContactEnum();
            // TODO add the preferred contact type
            companyForm.Model.Contact = new Contact();
            companyForm.Model.Contact.MailAdress = userForm.MailAdress;
            companyForm.Model.Contact.PhoneNumber = userForm.PhoneNumber;
            request.AddJsonBody(companyForm.Model);
            // request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            using (Operation.Time("Creation du compte Entreprise {id}", userForm.Auth.Login)) 
            {
                IRestResponse response = client.Execute(request);
                if (response.StatusCode.Equals(HttpStatusCode.Created))
                {
                    NotifyLIVE();
                    this.HttpContext.Session.Set(SessionConstant.ACCOUNT, JsonConvert.DeserializeObject<CompanyAccountModel>(response.Content));
                    return Ok();
                } else 
                {
                    DeleteUserAuth(userAuth, correlationId);
                    return HttpError(correlationId, request.Method, client.BuildUri(request).ToString(), (int) response.StatusCode, response.Content);
                }
            }
        }

        /// <summary>Create a candidate account</summary>
        /// <param name="client">HTTP connection client</param>
        /// <param name="request">HTTP request</param>
        /// <param name="userAuth">Associated Garda account</param>
        /// <param name="correlationId">Process correlationId</param>
        /// <returns>IRestResponse : HTTP response</returns>
        public IActionResult CreateCandidateAccount([FromBody] AddUserAuthCandidateForm userForm)
        {

            string correlationId = Guid.NewGuid().ToString();
            RestClient client = new RestClient(configuration["API:Host"]);
            
            IActionResult result = AddUserAuth(client, correlationId, userForm);
            // In case of an error during the request, AddUserAuth returns informations about the error, so we transmit it to the caller
            if (result != null)
            {
                return result;
            }
            UserAuthDTO userAuth = this.HttpContext.Session.Get<UserAuthDTO>(SessionConstant.JWT);

            AddUserAuthCandidateForm candidateForm = new AddUserAuthCandidateForm(userForm);
            IRestRequest request = HttpHelper.CreateBaseRequest(new HttpCriterias{
                Uri = configuration["API:Urls:Compte:Candidate"],
                Method = Method.POST,
                AccessToken = userAuth.AccessToken,
                CorrelationId = correlationId
            });
            request.AddUrlSegment(configuration["API:Urls:Parameters:Id"], userForm.Auth.Login);
            var handler = new TokenHandler(logger);
            var token = handler.ReadToken(userAuth.AccessToken);
            var userIdClaim = token.Payload.Claims.Single(c => c.Type == ClaimsConstant.USER_ID);
            candidateForm.Model.UserId = userIdClaim.Value;
            StringEnum preferredContactEnum = new PreferredContactEnum();
            // TODO add the preferred contact type
            // ((AddUserAuthCandidateForm) userForm).Model.Contact.PreferredContact = (PreferredContactEnum) preferredContactEnum.ValueOf(AgencyContactType);
            request.AddJsonBody(candidateForm.Model);
            // request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            using (Operation.Time("Creation du compte Candidat {id}", userForm.Auth.Login)) 
            {
                IRestResponse response = client.Execute(request);
                if (response.StatusCode.Equals(HttpStatusCode.Created))
                {
                    this.HttpContext.Session.Set(SessionConstant.ACCOUNT, JsonConvert.DeserializeObject<CandidateAccountModel>(response.Content));
                    return Ok();
                } else 
                {
                    DeleteUserAuth(userAuth, correlationId);
                    return HttpError(correlationId, request.Method, client.BuildUri(request).ToString(), (int) response.StatusCode, response.Content);
                }
            }
        }

        /// <summary>Create a candidate account</summary>
        /// <param name="client">HTTP connection client</param>
        /// <param name="request">HTTP request</param>
        /// <param name="userAuth">Associated Garda account</param>
        /// <param name="correlationId">Process correlationId</param>
        /// <returns>IRestResponse : HTTP response</returns>
        public IActionResult CreateCollaboratorAccount([FromBody] AddUserAuthCollaboratorForm userForm)
        {

            string correlationId = Guid.NewGuid().ToString();
            RestClient client = new RestClient(configuration["API:Host"]);
            
            UserAuthDTO thirdPartyUserAuth = AddThirdPartyUserAuth(client, correlationId, userForm);
            UserAuthDTO userAuth = this.HttpContext.Session.Get<UserAuthDTO>(SessionConstant.JWT);

             // Validation of the access token, to see if we have to regenerate it
            JwtSecurityToken token = TokenHelper.ValidateUserAuth(this.HttpContext.Session, configuration, userAuth, logger);
            if (token == null)
            {
                throw new UnauthorizedAccessException("Le jeton d'accès est nul. Merci de vous authentifier.");
            }
            userAuth = this.HttpContext.Session.Get<UserAuthDTO>(SessionConstant.JWT);
            
            AddUserAuthCollaboratorForm collaboratorForm = new AddUserAuthCollaboratorForm(userForm);
            IRestRequest request = HttpHelper.CreateBaseRequest(new HttpCriterias{
                Uri = configuration["API:Urls:Compte:Collaborator"],
                Method = Method.POST,
                AccessToken = userAuth.AccessToken,
                CorrelationId = correlationId
            });
            request.AddUrlSegment(configuration["API:Urls:Parameters:Id"], userForm.Auth.Login);
            TokenHandler handler = new TokenHandler(logger);
            JwtSecurityToken thirdPartyToken = handler.ReadToken(thirdPartyUserAuth.AccessToken);
            Claim thirdPartyUserIdClaim = thirdPartyToken.Payload.Claims.Single(c => c.Type == ClaimsConstant.USER_ID);
            token = handler.ReadToken(userAuth.AccessToken);
            Claim userIdClaim = token.Payload.Claims.Single(c => c.Type == ClaimsConstant.USER_ID);
            Claim accountTypeClaim = token.Payload.Claims.Single(c => c.Type == ClaimsConstant.ACCOUNT_TYPE);
            collaboratorForm.Model.UserId = thirdPartyUserIdClaim.Value;
            StringEnum preferredContactEnum = new PreferredContactEnum();
            // TODO add the preferred contact type
            // ((AddUserAuthCandidateForm) userForm).Model.Contact.PreferredContact = (PreferredContactEnum) preferredContactEnum.ValueOf(AgencyContactType);
            ReferentAccount referent = new ReferentAccount() {
                AccountType = accountTypeClaim.Value,
                UserId = userIdClaim.Value
            };
            collaboratorForm.Model.Referents = new List<ReferentAccount>() {referent};
            request.AddJsonBody(collaboratorForm.Model);
            // request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            using (Operation.Time("Creation du compte Candidat {id}", userForm.Auth.Login)) 
            {
                IRestResponse response = client.Execute(request);
                if (response.StatusCode.Equals(HttpStatusCode.Created))
                {
                    return Ok();
                } else 
                {
                    DeleteUserAuth(thirdPartyUserAuth, correlationId);
                    return HttpError(correlationId, request.Method, client.BuildUri(request).ToString(), (int) response.StatusCode, response.Content);
                }
            }
        }

        /// <summary>Send a mail notification to LIVE</summary>
        private void NotifyLIVE()
        {
            // Using Smtp Sender package (or set using AddSmtpSender in services)
            email.To("florian.c@learn-virtual.com")
    	        .Subject("Ajout d'un compte")
    	        .Body("Un nouveau compte a été ajouté")
		        .SendAsync();
        }

        /// <summary>Delete a third party Garda account (collaborator account)</summary>
        /// <param name="correlationId">Process correlationId</param>
        /// <param name="userId">UserId of the account to delete</param>
        private void DeleteThirdPartyUserAuth(string userId, string correlationId)
        {
            using (Operation.Time("Suppression des donnees du compte {id}", userId)) 
            {
                RestClient client = new RestClient(configuration["API:Host"]);

                RestRequest request = HttpHelper.CreateBaseRequest(new HttpCriterias{
                    Uri = configuration["API:Urls:Garda:Auth"],
                    Method = Method.DELETE,
                    CorrelationId = correlationId
                });
                DeleteUserRequest bodyRequest = new DeleteUserRequest{
                    ClientId = configuration["JWT:ClientId"],
                    ClientSecret = configuration["JWT:ClientSecret"]
                };
                request.AddQueryParameter("userId", userId);
                request.AddJsonBody(bodyRequest);
                IRestResponse response = client.Execute(request);
                if (!response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    //TODO gestion des erreurs
                    logger.LogInformation(response.ToString());
                }

            }
        }

        /// <summary>Delete a Garda account</summary>
        /// <param name="correlationId">Process correlationId</param>
        /// <param name="userAuth">UserAuth to delete</param>
        private void DeleteUserAuth(UserAuthDTO userAuth, string correlationId)
        {
            TokenHandler handler = new TokenHandler(logger);
            JwtSecurityToken token = handler.ReadToken(userAuth.AccessToken);
            Claim loginClaim = token.Payload.Claims.Single(c => c.Type == ClaimsConstant.LOGIN);
            using (Operation.Time("Suppression des donnees du compte {id}", loginClaim.Value)) 
            {
                RestClient client = new RestClient(configuration["API:Host"]);

                RestRequest request = HttpHelper.CreateBaseRequest(new HttpCriterias{
                    Uri = configuration["API:Urls:Garda:Auth"] + loginClaim.Value,
                    Method = Method.DELETE,
                    CorrelationId = correlationId
                });
                DeleteUserRequest bodyRequest = new DeleteUserRequest{
                    ClientId = configuration["JWT:ClientId"],
                    ClientSecret = configuration["JWT:ClientSecret"]
                };
                request.AddQueryParameter("login", loginClaim.Value);
                request.AddJsonBody(bodyRequest);
                IRestResponse response = client.Execute(request);
                if (!response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    //TODO gestion des erreurs
                    logger.LogInformation(response.ToString());
                }

            }
            this.HttpContext.Session.Remove(SessionConstant.JWT);
        }

        /// <summary>Get an existing account</summary>
        /// <param name="userId">UserId of the account to get</param>
        /// <returns>IActionResult : Ok if we got the account</returns>
        public IActionResult GetAccount([FromQuery] string userId)
        {
            UserAuthDTO userAuth = this.HttpContext.Session.Get<UserAuthDTO>(SessionConstant.JWT);

            // Validation of the access token, to see if we have to regenerate it
            JwtSecurityToken token = TokenHelper.ValidateUserAuth(this.HttpContext.Session, configuration, userAuth, logger);
            if (token == null)
            {
                throw new UnauthorizedAccessException("Le jeton d'accès est nul. Merci de vous authentifier.");
            }

            // Get the UserAuth back from the session because it could have been updated in ValidateUserAuth
            userAuth = this.HttpContext.Session.Get<UserAuthDTO>(SessionConstant.JWT);
            Claim accountTypeClaim = token.Payload.Claims.Single(c => c.Type == ClaimsConstant.ACCOUNT_TYPE);

            if(AccountTypesEnum.AGENCY.Value.Equals(accountTypeClaim.Value))
            {
                return Ok(GetAgencyAccount(userAuth, token, userId));
            } else if(AccountTypesEnum.CANDIDATE.Value.Equals(accountTypeClaim.Value))
            {
                return Ok(GetCandidateAccount(userAuth, token, userId));
            } else if(AccountTypesEnum.COMPANY.Value.Equals(accountTypeClaim.Value))
            {
                return Ok(GetCompanyAccount(userAuth, token, userId));
            }
            // if none of the methods is called, then we face a bad request because the accountType doesn't match one of the accepted types
            return BadRequest();
        }

        /// <summary>Get the account for the agency type</summary>
        /// <param name="token">The token from Garda</param>
        /// <param name="userAuth">The account from Garda</param>
        /// <param name="userId">The id of the account</param>
        private AccountDTO GetAgencyAccount(UserAuthDTO userAuth, JwtSecurityToken token, string userId)
        {
            AccountDTO accountDTO = new AccountDTO();
            Account account = this.HttpContext.Session.Get<AgencyAccountModelWithCollaborator>(SessionConstant.ACCOUNT);
            this.HttpContext.Session.Remove(SessionConstant.ACCOUNT);
            
            // the account may be null. Not null in case of an account creation
            if (account == null)
            {
                string correlationId = Guid.NewGuid().ToString();

                Claim userIdClaim = token.Payload.Claims.Single(c => c.Type == ClaimsConstant.USER_ID);
                string userIdAccount = userIdClaim.Value;
                if (!string.IsNullOrEmpty(userId)) 
                {
                    userIdAccount = userId;
                }

                RestClient client = new RestClient(configuration["API:Host"]);
                RestRequest request = HttpHelper.CreateBaseRequest(new HttpCriterias{
                    Uri = configuration["API:Urls:Compte:Agency"],
                    Method = Method.GET,
                    AccessToken = userAuth.AccessToken,
                    CorrelationId = correlationId
                });
                request.AddUrlSegment(configuration["API:Urls:Parameters:Id"], userIdAccount);

                using (Operation.Time("Recuperation des donnees du compte {id}", userIdAccount)) 
                {
                    IRestResponse response = client.Execute<AgencyAccountModel>(request);
                    if (!response.StatusCode.Equals(HttpStatusCode.OK))
                    {
                        // return HttpError(correlationId, request.Method, client.BuildUri(request).ToString(), (int) response.StatusCode, response.Content);
                    } else 
                    {
                        account = JsonConvert.DeserializeObject<AgencyAccountModelWithCollaborator>(response.Content);
                    }
                }
            }
            
            Claim loginClaim = token.Payload.Claims.Single(c => c.Type == ClaimsConstant.LOGIN);
            AuthForm auth = new AuthForm {
                Login = loginClaim.Value
            };
            accountDTO.Auth = auth;
            accountDTO.AccountType = AccountTypesEnum.AGENCY.Value;
            accountDTO.Account = account;
            return accountDTO;
        }
        
        /// <summary>Get the account for the candidate type</summary>
        /// <param name="token">The token from Garda</param>
        /// <param name="userAuth">The account from Garda</param>
        /// <param name="userId">The id of the account</param>
        private AccountDTO GetCandidateAccount(UserAuthDTO userAuth, JwtSecurityToken token, string userId)
        {
            AccountDTO accountDTO = new AccountDTO();
            Account account = this.HttpContext.Session.Get<CandidateAccountModel>(SessionConstant.ACCOUNT);
            this.HttpContext.Session.Remove(SessionConstant.ACCOUNT);
            if (account == null)
            {
                string correlationId = Guid.NewGuid().ToString();

                Claim userIdClaim = token.Payload.Claims.Single(c => c.Type == ClaimsConstant.USER_ID);
                string userIdAccount = userIdClaim.Value;
                if (!string.IsNullOrEmpty(userId)) 
                {
                    userIdAccount = userId;
                }

                RestClient client = new RestClient(configuration["API:Host"]);
                RestRequest request = HttpHelper.CreateBaseRequest(new HttpCriterias{
                    Uri = configuration["API:Urls:Compte:Candidate"],
                    Method = Method.GET,
                    AccessToken = userAuth.AccessToken,
                    CorrelationId = correlationId
                });
                request.AddUrlSegment(configuration["API:Urls:Parameters:Id"], userIdAccount);

                using (Operation.Time("Recuperation des donnees du compte {id}", userIdAccount)) 
                {
                    IRestResponse response = client.Execute<CandidateAccountModel>(request);
                    if (!response.StatusCode.Equals(HttpStatusCode.OK))
                    {
                        // return HttpError(correlationId, request.Method, client.BuildUri(request).ToString(), (int) response.StatusCode, response.Content);
                    } else 
                    {
                        account = JsonConvert.DeserializeObject<CandidateAccountModel>(response.Content);
                    }
                }
            }
            Claim loginClaim = token.Payload.Claims.Single(c => c.Type == ClaimsConstant.LOGIN);
            AuthForm auth = new AuthForm {
                Login = loginClaim.Value
            };
            accountDTO.Auth = auth;
            accountDTO.AccountType = AccountTypesEnum.CANDIDATE.Value;
            accountDTO.Account = account;
            return accountDTO;
        }

        /// <summary>Get the account for the company type</summary>
        /// <param name="token">The token from Garda</param>
        /// <param name="userAuth">The account from Garda</param>
        /// <param name="userId">The id of the account</param>
        private AccountDTO GetCompanyAccount(UserAuthDTO userAuth, JwtSecurityToken token, string userId)
        {
            AccountDTO accountDTO = new AccountDTO();
            Account account = this.HttpContext.Session.Get<CompanyAccountModel>(SessionConstant.ACCOUNT);
            this.HttpContext.Session.Remove(SessionConstant.ACCOUNT);

            if (account == null)
            {
                string correlationId = Guid.NewGuid().ToString();

                Claim userIdClaim = token.Payload.Claims.Single(c => c.Type == ClaimsConstant.USER_ID);
                string userIdAccount = userIdClaim.Value;
                if (!string.IsNullOrEmpty(userId)) 
                {
                    userIdAccount = userId;
                }

                RestClient client = new RestClient(configuration["API:Host"]);
                RestRequest request = HttpHelper.CreateBaseRequest(new HttpCriterias{
                    Uri = configuration["API:Urls:Compte:Company"],
                    Method = Method.GET,
                    AccessToken = userAuth.AccessToken,
                    CorrelationId = correlationId
                });
                request.AddUrlSegment(configuration["API:Urls:Parameters:Id"], userIdAccount);

                using (Operation.Time("Recuperation des donnees du compte {id}", userIdAccount)) 
                {
                    IRestResponse response = client.Execute<CompanyAccountModel>(request);
                    if (!response.StatusCode.Equals(HttpStatusCode.OK))
                    {
                        // return HttpError(correlationId, request.Method, client.BuildUri(request).ToString(), (int) response.StatusCode, response.Content);
                    } else 
                    {
                        account = JsonConvert.DeserializeObject<CompanyAccountModel>(response.Content);
                    }
                }
            }
            Claim loginClaim = token.Payload.Claims.Single(c => c.Type == ClaimsConstant.LOGIN);
            AuthForm auth = new AuthForm {
                Login = loginClaim.Value
            };
            accountDTO.Auth = auth;
            accountDTO.AccountType = AccountTypesEnum.COMPANY.Value;
            accountDTO.Account = account;
            return accountDTO;
        }

        /// <summary>Ajax method to get a collaborator account</summary>
        /// <param name="userId">The id of the account</param>
        public IActionResult GetCollaboratorAccount([FromQuery] string userId)
        {
            return Ok(GetCollaborator(userId));
        }

        /// <summary>Get the account for the company type</summary>
        /// <param name="userId">The id of the account</param>
        public AccountDTO GetCollaborator(string userId)
        {
            UserAuthDTO userAuth = this.HttpContext.Session.Get<UserAuthDTO>(SessionConstant.JWT);

            // Validation of the access token, to see if we have to regenerate it
            JwtSecurityToken token = TokenHelper.ValidateUserAuth(this.HttpContext.Session, configuration, userAuth, logger);
            if (token == null)
            {
                throw new UnauthorizedAccessException("Le jeton d'accès est nul. Merci de vous authentifier.");
            }
            userAuth = this.HttpContext.Session.Get<UserAuthDTO>(SessionConstant.JWT);

            CollaboratorAccountDTO accountDTO = new CollaboratorAccountDTO();
            string correlationId = Guid.NewGuid().ToString();

            Claim userIdClaim = token.Payload.Claims.Single(c => c.Type == ClaimsConstant.USER_ID);
            string userIdAccount = userIdClaim.Value;
            if (!string.IsNullOrEmpty(userId)) 
            {
                userIdAccount = userId;
            }

            RestClient client = new RestClient(configuration["API:Host"]);
            RestRequest request = HttpHelper.CreateBaseRequest(new HttpCriterias{
                Uri = configuration["API:Urls:Compte:Collaborator"],
                Method = Method.GET,
                AccessToken = userAuth.AccessToken,
                CorrelationId = correlationId
            });
            request.AddUrlSegment(configuration["API:Urls:Parameters:Id"], userIdAccount);

            Account account = null;
            using (Operation.Time("Recuperation des donnees du compte {id}", userIdAccount)) 
            {
                IRestResponse response = client.Execute<CollaboratorAccountModel>(request);
                if (!response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    throw new UnknownAccountException("Ce compte collaborateur ne semble pas faire partie de notre référentiel.");
                    // return HttpError(correlationId, request.Method, client.BuildUri(request).ToString(), (int) response.StatusCode, response.Content);
                } else 
                {
                    account = JsonConvert.DeserializeObject<CollaboratorAccountModel>(response.Content);
                }
            }

            Claim loginClaim = token.Payload.Claims.Single(c => c.Type == ClaimsConstant.LOGIN);
            AuthForm auth = new AuthForm {
                Login = loginClaim.Value
            };
            accountDTO.Auth = auth;
            accountDTO.AccountType = AccountTypesEnum.COLLABORATOR.Value;
            accountDTO.Account = account;

            List<ReferentAccount> referents = ((CollaboratorAccountModel) account).Referents;
            AccountDTO referentAccount = GetAgencyAccount(userAuth, token, referents[0].UserId);
            if (referentAccount == null) {
                throw new UnknownAccountException("Impossible de trouver le compte référent pour ce collaborateur.");
            }
            accountDTO.Name = ((AgencyAccountModel) referentAccount.Account).Name;
            accountDTO.Adress = ((AgencyAccountModel) referentAccount.Account).Adress;
            return accountDTO;
        }

        /// <summary> Create an Agency account (moral)</summary>
        /// <param name="client">HTTP connection client</param>
        /// <param name="request">HTTP request</param>
        /// <param name="userAuth">Associated account on Garda</param>
        /// <param name="correlationId">Process correlationId</param>
        /// <returns>IRestResponse : HTTP response</returns>
        public IActionResult UpdateAgencyAccount([FromBody] AddUserAuthAgencyForm userForm)
        {
            
            string correlationId = Guid.NewGuid().ToString();
            UserAuthDTO userAuth = this.HttpContext.Session.Get<UserAuthDTO>(SessionConstant.JWT);


            // Validation of the access token, to see if we have to regenerate it
            JwtSecurityToken token = TokenHelper.ValidateUserAuth(this.HttpContext.Session, configuration, userAuth, logger);
            if (token == null)
            {
                throw new UnauthorizedAccessException("Le jeton d'accès est nul. Merci de vous authentifier.");
            }

            // Get the UserAuth back from the session because it could have been updated in ValidateUserAuth
            userAuth = this.HttpContext.Session.Get<UserAuthDTO>(SessionConstant.JWT);

            AddUserAuthAgencyForm agencyForm = new AddUserAuthAgencyForm(userForm);
            RestClient client = new RestClient(configuration["API:Host"]);
            RestRequest request = HttpHelper.CreateBaseRequest(new HttpCriterias{
                    Uri = configuration["API:Urls:Compte:Agency"],
                    Method = Method.PUT,
                    AccessToken = userAuth.AccessToken,
                    CorrelationId = correlationId
                });
            
            var userIdClaim = token.Payload.Claims.Single(c => c.Type == ClaimsConstant.USER_ID);
            request.AddUrlSegment(configuration["API:Urls:Parameters:Id"], userIdClaim.Value);

            StringEnum preferredContactEnum = new PreferredContactEnum();
            // TODO add the preferred contact type
            request.AddJsonBody(agencyForm.Model);
            // request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            using (Operation.Time("Creation du compte Agence {id}", userForm.Auth.Login)) 
            {
                IRestResponse response = client.Execute(request);
                if (response.StatusCode.Equals(HttpStatusCode.Created))
                {
                    this.HttpContext.Session.Set(SessionConstant.ACCOUNT, JsonConvert.DeserializeObject<AgencyAccountModel>(response.Content));
                    return Ok();
                } else 
                {
                    return HttpError(correlationId, request.Method, client.BuildUri(request).ToString(), (int) response.StatusCode, response.Content);
                }
            }
        }

        /// <summary>Create a company account (moral)</summary>
        /// <param name="client">HTTP Connection client</param>
        /// <param name="request">HTTP request</param>
        /// <param name="userAuth">Associated Garda account</param>
        /// <param name="correlationId">Process correlationId</param>
        /// <returns>IRestResponse : HTTP response</returns>
        public IActionResult UpdateCompanyAccount([FromBody] AddUserAuthCompanyForm userForm)
        {

            string correlationId = Guid.NewGuid().ToString();
            UserAuthDTO userAuth = this.HttpContext.Session.Get<UserAuthDTO>(SessionConstant.JWT);


            // Validation of the access token, to see if we have to regenerate it
            JwtSecurityToken token = TokenHelper.ValidateUserAuth(this.HttpContext.Session, configuration, userAuth, logger);
            if (token == null)
            {
                throw new UnauthorizedAccessException("Le jeton d'accès est nul. Merci de vous authentifier.");
            }

            // Get the UserAuth back from the session because it could have been updated in ValidateUserAuth
            userAuth = this.HttpContext.Session.Get<UserAuthDTO>(SessionConstant.JWT);
            RestClient client = new RestClient(configuration["API:Host"]);

            AddUserAuthCompanyForm companyForm = new AddUserAuthCompanyForm(userForm);
            IRestRequest request = HttpHelper.CreateBaseRequest(new HttpCriterias{
                    Uri = configuration["API:Urls:Compte:Company"],
                    Method = Method.PUT,
                    AccessToken = userAuth.AccessToken,
                    CorrelationId = correlationId
                });
                
            var userIdClaim = token.Payload.Claims.Single(c => c.Type == ClaimsConstant.USER_ID);
            request.AddUrlSegment(configuration["API:Urls:Parameters:Id"], userIdClaim.Value);
            request.AddJsonBody(companyForm.Model);
            // request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            using (Operation.Time("Creation du compte Entreprise {id}", userForm.Auth.Login)) 
            {
                IRestResponse response = client.Execute(request);
                if (response.StatusCode.Equals(HttpStatusCode.Created))
                {
                    this.HttpContext.Session.Set(SessionConstant.ACCOUNT, JsonConvert.DeserializeObject<CompanyAccountModel>(response.Content));
                    return Ok();
                } else 
                {
                    return HttpError(correlationId, request.Method, client.BuildUri(request).ToString(), (int) response.StatusCode, response.Content);
                }
            }
        }

        /// <summary>Create a candidate account</summary>
        /// <param name="client">HTTP connection client</param>
        /// <param name="request">HTTP request</param>
        /// <param name="userAuth">Associated Garda account</param>
        /// <param name="correlationId">Process correlationId</param>
        /// <returns>IRestResponse : HTTP response</returns>
        public IActionResult UpdateCandidateAccount([FromBody] AddUserAuthCandidateForm userForm)
        {

            string correlationId = Guid.NewGuid().ToString();
            UserAuthDTO userAuth = this.HttpContext.Session.Get<UserAuthDTO>(SessionConstant.JWT);


            // Validation of the access token, to see if we have to regenerate it
            JwtSecurityToken token = TokenHelper.ValidateUserAuth(this.HttpContext.Session, configuration, userAuth, logger);
            if (token == null)
            {
                throw new UnauthorizedAccessException("Le jeton d'accès est nul. Merci de vous authentifier.");
            }

            // Get the UserAuth back from the session because it could have been updated in ValidateUserAuth
            userAuth = this.HttpContext.Session.Get<UserAuthDTO>(SessionConstant.JWT);
            RestClient client = new RestClient(configuration["API:Host"]);
            AddUserAuthCandidateForm candidateForm = new AddUserAuthCandidateForm(userForm);
            IRestRequest request = HttpHelper.CreateBaseRequest(new HttpCriterias{
                Uri = configuration["API:Urls:Compte:Candidate"],
                Method = Method.PUT,
                AccessToken = userAuth.AccessToken,
                CorrelationId = correlationId
            });
            
            var userIdClaim = token.Payload.Claims.Single(c => c.Type == ClaimsConstant.USER_ID);
            request.AddUrlSegment(configuration["API:Urls:Parameters:Id"], userIdClaim.Value);
            
            StringEnum preferredContactEnum = new PreferredContactEnum();
            // TODO add the preferred contact type
            // ((AddUserAuthCandidateForm) userForm).Model.Contact.PreferredContact = (PreferredContactEnum) preferredContactEnum.ValueOf(AgencyContactType);
            request.AddJsonBody(candidateForm.Model);
            // request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            using (Operation.Time("Creation du compte Candidat {id}", userForm.Auth.Login)) 
            {
                IRestResponse response = client.Execute(request);
                if (response.StatusCode.Equals(HttpStatusCode.Created))
                {
                    this.HttpContext.Session.Set(SessionConstant.ACCOUNT, JsonConvert.DeserializeObject<CandidateAccountModel>(response.Content));
                    return Ok();
                } else 
                {
                    return HttpError(correlationId, request.Method, client.BuildUri(request).ToString(), (int) response.StatusCode, response.Content);
                }
            }
        }

        /// <summary>Update a candidate account</summary>
        /// <param name="client">HTTP connection client</param>
        /// <param name="request">HTTP request</param>
        /// <param name="userAuth">Associated Garda account</param>
        /// <param name="correlationId">Process correlationId</param>
        /// <returns>IRestResponse : HTTP response</returns>
        public IActionResult UpdateCollaboratorAccount([FromBody] AddUserAuthCollaboratorForm userForm)
        {

            string correlationId = Guid.NewGuid().ToString();
            RestClient client = new RestClient(configuration["API:Host"]);
            
            // UserAuthDTO thirdPartyUserAuth = AddThirdPartyUserAuth(client, correlationId, userForm);
            UserAuthDTO userAuth = this.HttpContext.Session.Get<UserAuthDTO>(SessionConstant.JWT);

             // Validation of the access token, to see if we have to regenerate it
            JwtSecurityToken token = TokenHelper.ValidateUserAuth(this.HttpContext.Session, configuration, userAuth, logger);
            if (token == null)
            {
                throw new UnauthorizedAccessException("Le jeton d'accès est nul. Merci de vous authentifier.");
            }
            userAuth = this.HttpContext.Session.Get<UserAuthDTO>(SessionConstant.JWT);
            
            AccountDTO accountDTO = GetCollaborator(userForm.Model.UserId);

            AddUserAuthCollaboratorForm collaboratorForm = new AddUserAuthCollaboratorForm(userForm);
            CollaboratorAccountModel formModel = collaboratorForm.Model;

            CollaboratorAccountModel modelToUpdate = (CollaboratorAccountModel) accountDTO.Account;

            modelToUpdate.Contact = formModel.Contact;
            modelToUpdate.FirstName = formModel.FirstName;
            modelToUpdate.LastName = formModel.LastName;
            modelToUpdate.Position = formModel.Position;

            IRestRequest request = HttpHelper.CreateBaseRequest(new HttpCriterias{
                Uri = configuration["API:Urls:Compte:Collaborator"],
                Method = Method.PUT,
                AccessToken = userAuth.AccessToken,
                CorrelationId = correlationId
            });
            request.AddUrlSegment(configuration["API:Urls:Parameters:Id"], modelToUpdate.UserId);
            TokenHandler handler = new TokenHandler(logger);
            StringEnum preferredContactEnum = new PreferredContactEnum();
            // TODO add the preferred contact type
            // ((AddUserAuthCandidateForm) userForm).Model.Contact.PreferredContact = (PreferredContactEnum) preferredContactEnum.ValueOf(AgencyContactType);
            request.AddJsonBody(modelToUpdate);
            // request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            using (Operation.Time("Creation du compte Candidat {id}", userForm.Auth.Login)) 
            {
                IRestResponse response = client.Execute(request);
                if (response.StatusCode.Equals(HttpStatusCode.Created))
                {
                    return Ok();
                } else 
                {
                    return HttpError(correlationId, request.Method, client.BuildUri(request).ToString(), (int) response.StatusCode, response.Content);
                }
            }
        }

        /// <summary>Delete a collaborator account</summary>
        /// <param name="userId">Id of the collaborator to delete</param>
        public IActionResult DeleteCollaboratorAccount([FromQuery] string userId)
        {

            string correlationId = Guid.NewGuid().ToString();
            RestClient client = new RestClient(configuration["API:Host"]);
            
            UserAuthDTO userAuth = this.HttpContext.Session.Get<UserAuthDTO>(SessionConstant.JWT);

             // Validation of the access token, to see if we have to regenerate it
            JwtSecurityToken token = TokenHelper.ValidateUserAuth(this.HttpContext.Session, configuration, userAuth, logger);
            if (token == null)
            {
                throw new UnauthorizedAccessException("Le jeton d'accès est nul. Merci de vous authentifier.");
            }
            userAuth = this.HttpContext.Session.Get<UserAuthDTO>(SessionConstant.JWT);
            
            IRestRequest request = HttpHelper.CreateBaseRequest(new HttpCriterias{
                Uri = configuration["API:Urls:Compte:Collaborator"],
                Method = Method.DELETE,
                AccessToken = userAuth.AccessToken,
                CorrelationId = correlationId
            });
            request.AddUrlSegment(configuration["API:Urls:Parameters:Id"], userId);
            using (Operation.Time("Suppression du collaborateur {id}", userId)) 
            {
                IRestResponse response = client.Execute(request);
                if (!response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    return HttpError(correlationId, request.Method, client.BuildUri(request).ToString(), (int) response.StatusCode, response.Content);
                }
            }

            // We deleted the account, so we delete 
            DeleteThirdPartyUserAuth(userId, correlationId);
            return Ok();
        }

    }
}