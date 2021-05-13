using System;
using System.Collections.Generic;
using System.Linq;
using BankAccountAPI.Models;
using BankAccountAPI.Services;
using Microsoft.Extensions.Logging;

namespace BankAccountAPI
{


    public class StatementService : IStatementService
    {

        private readonly IStatementRepository _statementRepository;
        private readonly IFinTsExecutor _finTsExecutor;
        private readonly ILogger<StatementDownloadService> _logger;

        public StatementService(IStatementRepository statementRepository, ILogger<StatementDownloadService> logger, IFinTsExecutor finTsExecutor)
        {
            _statementRepository = statementRepository;
            _logger = logger;
            _finTsExecutor = finTsExecutor;
        }

        public IEnumerable<Statement> GetStatements(DateTime startDate, DateTime endDate, string bankIds)
        {
            var bankIdsList = bankIds.Split(",");
            var allStatements = _statementRepository.GetStatements();
            var desiredStatements = allStatements.Where(s => startDate <= s.Date && s.Date <= endDate && bankIdsList.Contains(s.BankId));

            var countStatements = desiredStatements.Count();
            _logger?.LogInformation("Returning {countStatements} statements for {startDate} to {endDate}", countStatements, startDate, endDate);

            return desiredStatements.OrderBy(s => s.Date);
        }

        public IEnumerable<Statement> DownloadLatestStatements(BankParams[] bankParamsList)
        {
            try
            {
                var potentiallyNewStatements = new List<Statement>();
                var startDate = DateTime.Now - TimeSpan.FromDays(80);
                var endDate = DateTime.Now;
                bankParamsList.ToList().ForEach(bp => potentiallyNewStatements.AddRange(_finTsExecutor.Download(startDate, endDate, bp)));

                var existingStatements = _statementRepository.GetStatements();
                var newStatements = potentiallyNewStatements.Where(pns => !existingStatements.Contains(pns));

                if(newStatements.Any()) _statementRepository.StoreStatements(newStatements);

                var countNewStatements = newStatements.Count();
                _logger?.LogInformation("Stored {countNewStatements} new statements in the database.", countNewStatements);
                return newStatements;
            }
            catch (Exception e)
            {
                _logger?.LogError(e, e.Message);
                return new List<Statement>();
            }
        }



    }
}