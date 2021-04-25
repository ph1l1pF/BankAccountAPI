using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;

namespace BankAccountAPI.Services
{
    public class StatementRepository : IStatementRepository
    {

        private readonly IMongoCollection<Statement> _statements;

        public StatementRepository(IStatementsDatabaseSettings statementsDatabaseSettings)
        {
            var client = new MongoClient(statementsDatabaseSettings.ConnectionString);
            var database = client.GetDatabase(statementsDatabaseSettings.DatabaseName);
            _statements = database.GetCollection<Statement>(statementsDatabaseSettings.StatementsCollectionName);
        }
        public IEnumerable<Statement> GetStatements()
        {
            try {
                return _statements.Find(s => true).ToList();
            } catch(MongoException e) {
                Console.WriteLine(e.Message);
                return new List<Statement>();
            }
        }

        public void StoreStatements(IEnumerable<Statement> statements)
        {
            try {
                _statements.InsertMany(statements);
            } catch(MongoException e) {
                Console.WriteLine(e.Message);
            }
        }
    }
}