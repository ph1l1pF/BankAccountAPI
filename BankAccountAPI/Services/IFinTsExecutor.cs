using System;
using System.Collections.Generic;
using BankAccountAPI.Models;

namespace BankAccountAPI.Services
{
    public interface IFinTsExecutor
    {
         IEnumerable<Statement> Download(DateTime start, DateTime end, BankParams bankParams);
    }
}