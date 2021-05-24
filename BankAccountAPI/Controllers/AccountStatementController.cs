using System;
using System.Collections.Generic;
using BankAccountAPI.Models;
using BankAccountAPI.Services;
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
        public IEnumerable<Statement> Get(string startDate, string endDate, string bankIds)
        {
            (var start, var end) = GetDatesOrThrow(startDate, endDate);
            return _statementService.GetStatements(start, end, bankIds);
        }

        private static (DateTime, DateTime) GetDatesOrThrow(string startDateString, string endDateString) 
        {
            DateTime startDate;
            if(!DateTime.TryParse(startDateString, out startDate)) throw new Exception();

            // endDate can be null => Then take DateTime.Now
            DateTime endDate = DateTime.Now;
            if(!string.IsNullOrWhiteSpace(endDateString) && 
                !DateTime.TryParse(endDateString, out endDate)) throw new Exception();
            
            return (startDate, endDate);
        }
        
        [HttpPost]
        [Route("StartCollecting")]
        public string StartCollecting([FromBody] BankParams[] bankParams)
        {
            if (!isCollecting)
            {
                isCollecting = true;
                _statementsDownloadService.StartDownloadTimer(bankParams);
                return "Started collecting.";
            }
            return "Already collecting.";
        }

        [HttpGet]
        [Route("IsCollecting")]
        public bool IsCollecting()
        {
            return isCollecting;
        }

    }
}
