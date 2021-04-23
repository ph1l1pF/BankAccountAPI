using System;
using System.Collections.Generic;

namespace BankAccountAPI.Services
{
    public interface IStatementService
    {
         IEnumerable<Statement> getStatements(DateTime startDate, DateTime endDate, string bankIds);
    }
}