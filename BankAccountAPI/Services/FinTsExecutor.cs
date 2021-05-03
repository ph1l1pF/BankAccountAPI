using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using BankAccountAPI.Models;

namespace BankAccountAPI.Services
{
    public class FinTsExecutor : IFinTsExecutor
    {
        public static readonly string Date = "Buchungstag";
        public static readonly string Sender = "Beguenstigter/Zahlungspflichtiger";
        public static readonly string Subject = "Verwendungszweck";
        public static readonly string Amount = "Betrag";
        
        public IEnumerable<Statement> Download(DateTime start, DateTime end, BankParams bankParams)
        {
            var csvPath = Path.Combine(Directory.GetCurrentDirectory(), "file1.csv");
            if (File.Exists(csvPath)) File.Delete(csvPath);

            var pythonPath = Environment.GetEnvironmentVariable("PYTHON_PATH") ?? "python3";
            var pythonScriptPath = Environment.GetEnvironmentVariable("PYTHON_SCRIPT_PATH") 
                ?? Path.Combine(GetPythonDirectory(), "Script.py");

            var command = $"{pythonPath} {pythonScriptPath} {bankParams.BankNumber} {bankParams.UserId} {bankParams.AccountNumber} {bankParams.Pin}" +
                    $" {start.ToString("yyyy-MM-dd")} {end.ToString("yyyy-MM-dd")} {bankParams.HttpsEndpoint} {csvPath}";
            ExecuteCommand(command);
            return ReadStatementsFromFile(csvPath, bankParams.BankId);
        }

        private void ExecuteCommand(string command)
        {
            var process = new Process();
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.FileName = "/bin/bash";
            process.StartInfo.Arguments = "-c \" " + command + " \"";
            process.Start();
            process.WaitForExit();
        }

        private static string GetPythonDirectory()
        {
            var workingDirectory = Directory.GetParent(Directory.GetCurrentDirectory());
            return Path.Combine(workingDirectory.FullName, "BankAccountAPI/PythonScript");
        }

        private IList<Statement> ReadStatementsFromFile(string csvPath, string bankId)
        {
            List<string> lines;
            try
            {
                lines = new List<string>(File.ReadAllLines(csvPath));
            }
            catch (FileNotFoundException)
            {
                throw new TransactionReadException("Fehler beim Lesen der Transaktionen: Es konnten keine Transaktionen geladen werden.");
            }
            catch (Exception e)
            {
                throw new TransactionReadException($"Fehler beim Lesen der Transaktionen: {e.Message}");
            }

            if (lines.Count == 0) throw new TransactionReadException("Problem: Die Bank hat eine Antwort gesendet, " +
                 "aber es befinden sich keine Transaktionen in der Antwort.");

            var headLineToIndex = GetIndexesOfHeadlines(lines[0]);
            lines.RemoveAt(0);
            var accountStatements = new List<Statement>();

            foreach (var line in lines)
            {
                string[] columnValues = line.Split(";");
                var dateTime = DateTime.Parse(columnValues[headLineToIndex[Date]]);

                var amountStringWithPoint = columnValues[headLineToIndex[Amount]].Replace(",", ".");
                var amount = double.Parse(amountStringWithPoint, NumberStyles.Any, CultureInfo.InvariantCulture);

                var accStatement = new Statement(dateTime,
                    columnValues[headLineToIndex[Sender]],
                    columnValues[headLineToIndex[Subject]],
                    amount, bankId);
                accountStatements.Add(accStatement);

            }

            return accountStatements;
        }

        private IDictionary<string, int> GetIndexesOfHeadlines(string firstLineOfFile)
        {
            var headlines = new List<string>(firstLineOfFile.Split(";"));
            return new Dictionary<string, int>
            {
                { Date, headlines.IndexOf(Date) },
                { Sender, headlines.IndexOf(Sender) },
                { Subject, headlines.IndexOf(Subject) },
                { Amount, headlines.IndexOf(Amount) }
            };
        }
    }
}