using System;
using System.Collections.Generic;
using BankAccountAPI.Models;
using BankAccountAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankAccountAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountStatementController
    {
        private readonly IStatementService _statementService;
        private readonly IStatementDownloadService _statementsDownloadService;

        private static bool isCollecting = false;
        public AccountStatementController(IStatementService statementService, IStatementDownloadService statementsDownloadService)
        {
            _statementService = statementService;
            _statementsDownloadService = statementsDownloadService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Statement>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Get(string startDate, string endDate, string bankIds)
        {
            try {
                (var start, var end) = GetDatesOrThrow(startDate, endDate);
                return new OkObjectResult(_statementService.GetStatements(start, end, bankIds));
            } catch (Exception) {
                return new BadRequestResult();
            }
        }

        private static (DateTime, DateTime) GetDatesOrThrow(string startDateString, string endDateString) 
        {
            if(!DateTime.TryParse(startDateString, out var startDate)) throw new Exception();

            // endDate can be null => Then take DateTime.Now
            DateTime endDate = DateTime.Now;
            if(!string.IsNullOrWhiteSpace(endDateString) && 
                !DateTime.TryParse(endDateString, out endDate)) throw new Exception();
            
            return (startDate, endDate);
        }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("StartCollecting")]
        public IActionResult StartCollecting([FromBody] BankParams[] bankParams)
        {
            if (!isCollecting)
            {
                isCollecting = true;
                _statementsDownloadService.StartDownloadTimer(bankParams);
                return new OkObjectResult("Started collecting statements.");
            }
            return new BadRequestResult();
        }

        [HttpGet]
        [Route("IsCollecting")]
        public bool IsCollecting()
        {
            return isCollecting;
        }

    }
}
