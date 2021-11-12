using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace TheatricalPlayersRefactoringKata
{
    public class StatementPrinter
    {
        public string Print(Invoice invoice, Dictionary<string, Play> plays)
        {
            var totalAmount = 0;
            var volumeCredits = 0;
            var result = GetInvoiceHeader(invoice);
            CultureInfo cultureInfo = new CultureInfo("en-US");

            foreach(var perf in invoice.Performances) 
            {
                var play = plays[perf.PlayID];
                var thisAmount = GetPerformaceCost(perf, plays);
                
                // add volume credits
                volumeCredits += Math.Max(perf.Audience - 30, 0);
                // add extra credit for every ten comedy attendees
                if ("comedy" == play.Type) volumeCredits += (int)Math.Floor((decimal)perf.Audience / 5);

                // print line for this order
                result += String.Format(cultureInfo, "  {0}: {1:C} ({2} seats)\n", play.Name, Convert.ToDecimal(thisAmount / 100), perf.Audience);
                totalAmount += thisAmount;
            }
            result += String.Format(cultureInfo, "Amount owed is {0:C}\n", Convert.ToDecimal(totalAmount / 100));
            result += String.Format("You earned {0} credits\n", volumeCredits);
            return result;
        }

        string GetInvoiceHeader(Invoice invoice)
        {
            return $"Statement for {invoice.Customer}\n";
        }

        int GetPerformaceCost(Performance performance, Dictionary<string, Play> plays)
        {
            var play = plays[performance.PlayID];
            int cost;

            switch (play.Type)
            {
                case "tragedy":
                    cost = 40000;
                    if (performance.Audience > 30)
                    {
                        cost += 1000 * (performance.Audience - 30);
                    }
                    break;
                case "comedy":
                    cost = 30000;
                    if (performance.Audience > 20)
                    {
                        cost += 10000 + 500 * (performance.Audience - 20);
                    }
                    cost += 300 * performance.Audience;
                    break;
                default:
                    throw new Exception("unknown type: " + play.Type);
            }

            return cost;
        }

        public string PrintAsHtml(Invoice invoice, Dictionary<string, Play> plays)
        {
            var stringBuilder = new StringBuilder("<html>");
            stringBuilder.AppendLine($"<h1>{GetInvoiceHeader(invoice)}</h1>");
            stringBuilder.AppendLine("</html>");
            return stringBuilder.ToString();
        }
    }
}
