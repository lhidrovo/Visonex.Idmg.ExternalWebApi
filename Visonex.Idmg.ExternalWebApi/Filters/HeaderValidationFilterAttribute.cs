using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using Visonex.Idmg.ExternalWebApi.Common;
using Visonex.Idmg.ExternalWebApi.Exceptions;

namespace Visonex.Idmg.ExternalWebApi.Filters
{
    public class HeaderValidationFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var dataBaseParameter = context.HttpContext.Request.Headers
                .SingleOrDefault(t => t.Key == CustomHeaders.OrganizationNameHeader).Value;

            if (string.IsNullOrEmpty(dataBaseParameter))
            {
                throw new HttpResponseException
                {
                    Status = (int)HttpStatusCode.NotFound,
                    Msg = $"The '{CustomHeaders.OrganizationNameHeader}' header parameter is required."
                };
            }

            base.OnActionExecuting(context);
        }
    }
}
