using System;
using System.Collections.Generic;
using System.Linq;
using BankAccountAPI.Services;
using Microsoft.Extensions.Logging;

namespace BankAccountAPI
{

    public class BankParams
    {
        public string BankNumber { get; }
        public string AccountNumber { get; }
        public string UserId { get; }
        public string Pin { get; }
        public string HttpsEndpoint { get; }

        public string BankId {get;}

        public BankParams(string bankNumber, string accountNumber, string userId, string pin, string httpsEndpoint, string bankId)
        {
            BankNumber = bankNumber;
            AccountNumber = accountNumber;
            UserId = userId;
            Pin = pin;
            HttpsEndpoint = httpsEndpoint;
            BankId = bankId;
        }
    }
    public class StatementService : IStatementService
    {

        private readonly IStatementRepository _statementRepository;
        private readonly ILogger<StatementDownloadService> _logger;

        public StatementService(IStatementRepository statementRepository, ILogger<StatementDownloadService> logger)
        {
            _statementRepository = statementRepository;
            _logger = logger;
        }

        public IEnumerable<Statement> GetStatements(DateTime startDate, DateTime endDate, string bankIds)
        {
            var bankIdsList = bankIds.Split(",");
            var allStatements = _statementRepository.GetStatements();
            var desiredStatements = allStatements.Where(s => startDate <= s.Date && s.Date<=endDate && bankIdsList.Contains(s.BankId));
            
            var countStatements = desiredStatements.Count();
            _logger.LogInformation("Returning {countStatements} statements for {startDate} to {endDate}", countStatements, startDate, endDate);
            
            return desiredStatements.OrderBy(s => s.Date);
        }

        

        
    }
}