using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeIsMoney.Api.Attributes;
using TimeIsMoney.Common;
using TimeIsMoney.Services.TransactionType;
using TimeIsMoney.Services.TransactionType.Models;

namespace TimeIsMoney.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/transaction_type")]
    [Authorize]
    public class TransactionTypeController : Controller
    {
        private readonly ITransactionTypeService _transactionTypeService;
        private readonly DomainTaskStatus _taskStatus;

        public TransactionTypeController(ITransactionTypeService transactionTypeService, DomainTaskStatus taskStatus)
        {
            _transactionTypeService = transactionTypeService;
            _taskStatus = taskStatus;
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(TransactionTypeModel), 200)]
        [ServiceFilter(typeof(ValidateModelAttribute))]
        public IActionResult CreateType([FromBody] CreateTransactionTypeModel typeModel)
        {
            var type = _transactionTypeService.AddType(typeModel);
            if (!_taskStatus.HasErrors)
            {
                return Ok(type);
            }
            return BadRequest(_taskStatus.ErrorCollection);
        }

        [HttpGet]
        [Route("autocomplete")]
        [ProducesResponseType(typeof(List<TransactionTypeModel>), 200)]
        public IActionResult Autocomplete([FromQuery] string query)
        {
            var types = _transactionTypeService.Autocomplete(query);
            if (!_taskStatus.HasErrors)
            {
                return Ok(types);
            }
            return BadRequest(_taskStatus.ErrorCollection);
        }
    }
}