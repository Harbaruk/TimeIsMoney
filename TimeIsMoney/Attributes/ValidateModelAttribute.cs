using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using TimeIsMoney.Api.Extensions;
using TimeIsMoney.Common;

namespace TimeIsMoney.Api.Attributes
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        private readonly DomainTaskStatus _taskStatus;

        public ValidateModelAttribute(DomainTaskStatus taskStatus)
        {
            _taskStatus = taskStatus;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            foreach (var item in context.ModelState)
            {
                if (item.Value.Errors.FirstOrDefault() != null)
                {
                    _taskStatus.AddError(item.Key.FirstLetterToLower(), item.Value.Errors.FirstOrDefault().ErrorMessage);
                }
            }
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(_taskStatus.ErrorCollection);
            }
        }
    }
}