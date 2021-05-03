using System;
using System.Collections.Generic;
using BankAccountAPI.Models;

namespace BankAccountAPI.Services
{
    public interface IStatementService
    {
         IEnumerable<Statement> GetStatements(DateTime startDate, DateTime endDate, string bankIds);
         IEnumerable<Statement> DownloadLatestStatements(BankParams[] bankParamsList);
    }
}