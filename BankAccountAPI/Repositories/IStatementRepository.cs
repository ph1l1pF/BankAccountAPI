using System.Collections.Generic;

namespace BankAccountAPI.Services
{
    public interface IStatementRepository
    {
        IEnumerable<Statement> GetStatements();

        void StoreStatements(IEnumerable<Statement> statements);
    }
}