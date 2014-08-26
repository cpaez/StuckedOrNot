using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Http;
using Stucked.Infrastructure;
using System.Net.Http;
using System.Net;

namespace Stucked.API.Infrastructure
{
    public class ExceptionHandlingAttribute : ExceptionFilterAttribute
    {
        private static readonly ILogger _logger = LogManager.GetLogger(typeof(CustomLogger));

        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is ApplicationException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(context.Exception.Message),
                    ReasonPhrase = "Exception"
                });
            }

            // log error
            _logger.Error("Controller exception.", context.Exception);

            throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent("An error occurred, please try again or contact the administrator."),
                ReasonPhrase = "Critical Exception"
            });
        }
    }
}