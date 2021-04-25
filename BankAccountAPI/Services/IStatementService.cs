using System;
using System.Collections.Generic;

namespace BankAccountAPI.Services
{
    public interface IStatementService
    {
         IEnumerable<Statement> GetStatements(DateTime startDate, DateTime endDate, string bankIds);
    }
}