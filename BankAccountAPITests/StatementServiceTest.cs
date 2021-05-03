using System;
using System.Collections.Generic;
using BankAccountAPI.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using FluentAssertions;
using System.Linq;
using BankAccountAPI.Models;

namespace BankAccountAPI.Test
{
    [TestClass]
    public class StatementServiceTest
    {
        private IStatementService _sut;
        private IStatementRepository _statementRepo;
        private IFinTsExecutor _finTsExecutor;

        [TestInitialize]
        public void Init()
        {
            _statementRepo = Substitute.For<IStatementRepository>();
            _finTsExecutor = Substitute.For<IFinTsExecutor>();
        }

        [TestMethod]
        public void Test_GetStatements_DateRange()
        {
            _statementRepo.GetStatements().ReturnsForAnyArgs(new List<Statement>{
                new Statement(new DateTime(2021,5,1),"sender","subject",100,"bankId"),
                new Statement(new DateTime(2021,5,2),"sender","subject",100,"bankId"),
                new Statement(new DateTime(2021,7,1),"sender","subject",100,"bankId"),
            });
            _sut = new StatementService(_statementRepo, null, _finTsExecutor);

            var statements = _sut.GetStatements(new DateTime(2021, 5, 1), new DateTime(2021, 5, 2), "bankId");

            statements.Count().Should().Be(2);
            statements.ToList()[0].Should().BeEquivalentTo(new Statement(new DateTime(2021,5,1),"sender","subject",100,"bankId"));
            statements.ToList()[1].Should().BeEquivalentTo(new Statement(new DateTime(2021,5,2),"sender","subject",100,"bankId"));
        }

        [TestMethod]
        public void Test_GetStatements_BankIds()
        {
            _statementRepo.GetStatements().ReturnsForAnyArgs(new List<Statement>{
                new Statement(new DateTime(2021,5,1),"sender","subject",100,"bankId1"),
                new Statement(new DateTime(2021,5,2),"sender","subject",100,"bankId2"),
                new Statement(new DateTime(2021,7,1),"sender","subject",100,"bankId3"),
            });
            _sut = new StatementService(_statementRepo, null, _finTsExecutor);

            var statements = _sut.GetStatements(new DateTime(2021, 5, 1), new DateTime(2021, 8, 2), "bankId1,bankId3");

            statements.Count().Should().Be(2);
            statements.ToList()[0].Should().BeEquivalentTo(new Statement(new DateTime(2021,5,1),"sender","subject",100,"bankId1"));
            statements.ToList()[1].Should().BeEquivalentTo(new Statement(new DateTime(2021,7,1),"sender","subject",100,"bankId3"));
        }


        [TestMethod]
        public void Test_DownloadLatestStatements()
        {
            var existingStatements = new List<Statement>{
                new Statement(new DateTime(2021,5,1),"sender","subject",100,"bankId1"),
                new Statement(new DateTime(2021,5,2),"sender","subject",100,"bankId2")
            };
            _statementRepo.GetStatements().ReturnsForAnyArgs(existingStatements);
            
            var newStatements = new List<Statement>{
                new Statement(new DateTime(2021,4,1),"sender","subject",100,"bankId3"),
                new Statement(new DateTime(2021,8,1),"sender8","subject",100,"bankId3")
            };
            var allStatements = new List<Statement>(existingStatements);
            allStatements.AddRange(newStatements);
            _finTsExecutor.Download(default, default, default).ReturnsForAnyArgs(allStatements);
            _sut = new StatementService(_statementRepo, null, _finTsExecutor);

            var statements = _sut.DownloadLatestStatements(new BankParams[]{null});

            statements.Count().Should().Be(2);
            statements.ToList()[0].Should().BeEquivalentTo(newStatements[0]);
            statements.ToList()[1].Should().BeEquivalentTo(newStatements[1]);
        }
    }
}