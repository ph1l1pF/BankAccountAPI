using System;
using System.Threading;
using BankAccountAPI.Models;
using Microsoft.Extensions.Logging;

namespace BankAccountAPI.Services
{
    public class StatementDownloadService : IStatementDownloadService
    {
        
        private readonly IStatementService _statementService;
        private readonly IStatementRepository _statementRepository;


        private BankParams[] bankParamsList;
        public StatementDownloadService(IStatementService statementService, 
                                        IStatementRepository statementRepository)
        {
            _statementService = statementService;
            _statementRepository = statementRepository;
        }

        public void StartDownloadTimer(BankParams[] bankParams)
        {
            this.bankParamsList = bankParams;
            new Timer(DownloadLatestStatements, null, 0, 100000);
        }

        private void DownloadLatestStatements(Object o)
        {
            var latestStatements =_statementService.DownloadLatestStatements(bankParamsList);
        }
        
        
    }
}