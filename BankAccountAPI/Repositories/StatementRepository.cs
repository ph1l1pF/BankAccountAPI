using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace BankAccountAPI.Services
{
    public class StatementRepository : IStatementRepository
    {

        private readonly IMongoCollection<Statement> _statements;
        private readonly ILogger<StatementRepository> _logger;

        public StatementRepository(IStatementsDatabaseSettings statementsDatabaseSettings, ILogger<StatementRepository> logger)
        {
            var client = new MongoClient(statementsDatabaseSettings.ConnectionString);
            var database = client.GetDatabase(statementsDatabaseSettings.DatabaseName);
            _statements = database.GetCollection<Statement>(statementsDatabaseSettings.StatementsCollectionName);
            _logger = logger;
        }
        public IEnumerable<Statement> GetStatements()
        {
            try {
                return _statements.Find(s => true).ToList();
            } catch(MongoException e) {
                _logger.LogError(e, e.Message);
                return new List<Statement>();
            }
        }

        public void StoreStatements(IEnumerable<Statement> statements)
        {
            try {
                _statements.InsertMany(statements);
            } catch(MongoException e) {
                _logger.LogError(e, e.Message);
            }
        }
    }
}