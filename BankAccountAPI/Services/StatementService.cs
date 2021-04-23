using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using BankAccountAPI.Services;

namespace BankAccountAPI
{
    public enum BankId
    {
        [Description("Comdirect")]
        Comdirect,

        [Description("Sparkasse")]
        Sparkasse
    }

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

        public IEnumerable<Statement> getStatements(DateTime startDate, DateTime endDate, string bankIds)
        {
            var statements = new List<Statement>();
            var bankIdsList = new List<string>();
            foreach (var bank in bankIds.Split(","))
            {
                if (Enum.TryParse(typeof(BankId), bank, true, out var bankId)) {
                    //statements.AddRange(getStatementsForOneBank(startDate, endDate, (BankId)bankId));
                    bankIdsList.Add(bank);
                }
            }

            //_statementRepository.StoreStatements(statements.OrderBy(s => s.Date));

            var allStatements = _statementRepository.GetStatements();
            var desiredStatements = allStatements.Where(s => startDate <= s.Date && s.Date<=endDate && bankIdsList.Contains(s.BankId));
            return desiredStatements.OrderBy(s => s.Date);
        }

        

        
    }
}