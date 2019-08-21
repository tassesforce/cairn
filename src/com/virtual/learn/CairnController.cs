using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RestSharp;

namespace cairn.Controllers
{
    /// <summary>global controller to handle the solution</summary>
    public abstract class CairnController : Controller
    {

        /// <summary>Logger to use</summary>
        protected ILogger logger;
        /// <summary>Used to get informations in the configuration files</summary>
        protected IConfiguration configuration;

        /// <summary>Default constructor</summary>
        public CairnController(ILogger logger) : this(logger, null)
        {
            
        }

        /// <summary>Default constructor</summary>
        public CairnController(ILogger logger, IConfiguration configuration)
        {
            this.logger = logger;
            this.configuration = configuration;
        }

        /// <summary>Generate the log after an error during the HTTP request</summary>
        /// <param name="correlationId">CorrelationId of the request</param>
        /// <param name="message">HTTP response message</param>
        /// <param name="httpStatus">HTTP response status</param>
        /// <param name="requestUrl">requestUrl</param>
        protected string GenerateLogHttpError(string correlationId, Method method, string requestUrl, int httpStatus, string message)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("(").Append(correlationId).Append(") ");
            builder.Append(method.ToString()).Append(" ");
            builder.Append(requestUrl);
            builder.Append(" : ");
            builder.Append(httpStatus);
            builder.Append(" ");
            builder.Append(message);

            return builder.ToString();
        }

        /// <summary>Return the IActionResult corresponding to the statusCode in the parameters</summary>
        /// <param name="correlationId">CorrelationId of the request</param>
        /// <param name="message">HTTP response message</param>
        /// <param name="httpStatus">HTTP response status</param>
        /// <param name="requestUrl">requestUrl</param>
        protected IActionResult HttpError(string correlationId, Method method, string requestUrl, int httpStatus, string message)
        {
            logger.LogError(GenerateLogHttpError(correlationId, method, requestUrl, httpStatus, message));
            if (HttpStatusCode.BadRequest.Equals(httpStatus))
            {
                return BadRequest();
            } else if (HttpStatusCode.Forbidden.Equals(httpStatus) || HttpStatusCode.Unauthorized.Equals(httpStatus))
            {
                return Forbid();
            } else if (HttpStatusCode.NotFound.Equals(httpStatus))
            {
                return NotFound();
            } else 
            {
                return StatusCode(httpStatus);
            }
        }

    } 
}