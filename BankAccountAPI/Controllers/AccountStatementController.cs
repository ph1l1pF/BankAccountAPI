using System;
using System.Collections.Generic;
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
            return _statementService.GetStatements(DateTime.Parse(startDate), DateTime.Parse(endDate), bankIds);
        }

        [HttpGet]
        [Route("HealthCheck")]
        public string HealthCheck()
        {
            return "healthy";
        }

        /*
            [
                {
                    "bankNumber": "",
                    "accountNumber": "",
                    "userId": "",
                    "pin": "",
                    "httpsEndpoint": "https://fints.comdirect.de/fints",
                    "bankId": "Comdirect"
                },
                {
                    "bankNumber": "",
                    "accountNumber": "",
                    "userId": "",
                    "pin": "",
                    "httpsEndpoint": "https://banking-wl1.s-fints-pt-wl.de/fints30",
                    "bankId": "Sparkasse"
                }
            ]          
        */
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
