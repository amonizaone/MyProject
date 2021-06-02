using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MyApi.Core
{
    public class MimeType : Attribute, IResourceFilter
    {
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            context.HttpContext.Request.Headers["Content-Type"] = "application/json";
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
        }
    }
}
