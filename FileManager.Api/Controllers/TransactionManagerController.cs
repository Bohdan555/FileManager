using System.Threading.Tasks;
using TransactionManager.Components.Models;
using TransactionManager.Components.Models.Requests;
using TransactionManager.Components.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TransactionManager.Components.Domain.Enums;
using System;

namespace TransactionManager.Controllers
{
    [ApiController]
    [Route("api/transaction")]
    public class TransactionManagerController : ControllerBase
    {
        private readonly ILogger<TransactionManagerController> _logger;
        private readonly ITransactionManagerService _transactionManagerService;

        public TransactionManagerController(ILogger<TransactionManagerController> logger, ITransactionManagerService transactionManagerService)
        {
            _logger = logger;
            _transactionManagerService = transactionManagerService;
        }

        [HttpPost]
        public async Task<IActionResult> SaveFile([FromForm] UploadFileRequest request)
        {
            await _transactionManagerService.ImportTransactions(request);
            return Ok();
        }

        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetTransactions(Status status)
        {
            var result = await _transactionManagerService.GetTransactions(status);
            return Ok(result);
        }

        [HttpGet("date")]
        public async Task<IActionResult> GetTransactions([FromQuery] string startDate, [FromQuery] string endDate)
        {
            var result = await _transactionManagerService.GetTransactions(startDate, endDate);
            return Ok(result);
        }

        [HttpGet("currencyCode/{currencyCode}")]
        public async Task<IActionResult> GetTransactions(string currencyCode)
        {
            var result = await _transactionManagerService.GetTransactions(currencyCode);
            return Ok(result);
        }
    }
}
