using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using BankAccountAPI.Services;

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
        public StatementService(IStatementRepository statementRepository)
        {
            _statementRepository = statementRepository;
        }

        public IEnumerable<Statement> GetStatements(DateTime startDate, DateTime endDate, string bankIds)
        {
            var bankIdsList = bankIds.Split(",");
            var allStatements = _statementRepository.GetStatements();
            var desiredStatements = allStatements.Where(s => startDate <= s.Date && s.Date<=endDate && bankIdsList.Contains(s.BankId));
            return desiredStatements.OrderBy(s => s.Date);
        }

        

        
    }
}