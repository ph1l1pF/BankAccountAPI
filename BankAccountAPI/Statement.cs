using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BankAccountAPI
{
    public class Statement
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public string SenderOrReceiver { get; set; }
        public string Subject { get; set; }
        public double Amount { get; set; }

        // hier möchten wir die bank als lesbaren string haben
        public string BankId { get; set; }
        public Statement(DateTime dateTime, string senderOrReceiver, string subject, double amount, string bankId)
        {
            Date = dateTime;
            SenderOrReceiver = senderOrReceiver;
            Subject = subject;
            Amount = amount;
            BankId = bankId;
        }

        public override bool Equals(object obj)
        {
            var statement = (Statement)obj;
            return Date.Equals(statement.Date)
                && SenderOrReceiver.Equals(statement.SenderOrReceiver)
                && Subject.Equals(statement.Subject)
                && Amount.Equals(statement.Amount)
                && BankId.Equals(statement.BankId);
        }

        public override int GetHashCode()
        {
            return Date.GetHashCode() + SenderOrReceiver.GetHashCode() +
                Subject.GetHashCode() + Amount.GetHashCode() + BankId.GetHashCode();
        }

        public override string ToString()
        {
            return $"{Date.ToString("dd.MM.yyyy")}§{SenderOrReceiver.Trim()}§{Subject.Trim()}§{Amount}§{BankId}";
        }
    }
}