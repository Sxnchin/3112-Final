using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Interfaces;

namespace DefaultNamespace.Models
{
    public class ResultsProcessor
    {
        private readonly List<IGameObserver> _observers;

        public ResultsProcessor(List<IGameObserver> observers)
        {
            _observers = observers;
        }

        public void ProcessFinalResults(List<FinancialReport> playerReports, List<FinancialReport> opponentReports)
        {
            Notify("");
            Notify("===== FINAL RESULTS =====");

            var playerFinalReport = new FinancialReport(
                playerReports.Sum(r => r.Revenue),
                playerReports.Sum(r => r.Expenses)
            );

            var opponentFinalReport = new FinancialReport(
                opponentReports.Sum(r => r.Revenue),
                opponentReports.Sum(r => r.Expenses)
            );

            Notify($"Your Final Profit: ${playerFinalReport.Profit:F2}");
            Notify($"Opponent Final Profit: ${opponentFinalReport.Profit:F2}");

            DetermineWinner(playerFinalReport, opponentFinalReport);
        }

        private void DetermineWinner(FinancialReport playerReport, FinancialReport opponentReport)
        {
            if (playerReport.Profit > opponentReport.Profit)
                Notify(" YOU WIN! Your business outperformed the rival!");
            else if (playerReport.Profit < opponentReport.Profit)
                Notify(" YOU LOSE! The rival dominated the market.");
            else
                Notify("It's a tie! You don't see that often!");
        }

        private void Notify(string message)
        {
            foreach (var o in _observers)
                o.Update(message);
        }
    }
}
