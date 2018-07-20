using System;
using MobilePatronsApp.DataContracts.Interfaces;

namespace MobilePatronsApp.DataContracts.Implementations
{
	public class Offer : IOffer
	{
		public String PromoCode { get; set; }
		public DateTime PromoStartDate { get; set; }
		public DateTime PromoEndDate { get; set; }
		public String PromoText { get; set; }
		public String PromoType { get; set; }
		public String ErrorCode { get; set; }
	}

	public class PowerReward : IPowerReward
	{
		public String PromoAmount { get; set; }
		public String PromoCode { get; set; }
		public String PromoStartDate { get; set; }
		public String PromoStartTime { get; set; }
		public String PromoEndDate { get; set; }
		public String PromoEndTime { get; set; }
		public String PromoStatus { get; set; }
		public String PromoText { get; set; }
		public String ErrorCode { get; set; }
	}

    public class Sweepstake : ISweepstake
	{
		public String Activation { get; set; }
		public String ActiveEntries { get; set; }
		public String BonusEntries { get; set; }
		public String SweepstakesCode { get; set; }
		public String StartDate { get; set; }
		public String StartTime { get; set; }
		public String EndDate { get; set; }
		public String EndTime { get; set; }
		public String FreeEntries { get; set; }
		public String TotalEntries { get; set; }
		public String Description { get; set; }
		public String ErrorCode { get; set; }
	}

	public class SweepstakeDrawing : ISweepstakeDrawing
	{
		public String Activation { get; set; }
		public String StartDate { get; set; }
		public String StartTime { get; set; }
		public String EndDate { get; set; }
		public String EndTime { get; set; }
		public String DrawingDate { get; set; }
		public String DrawingTime { get; set; }
		public String EntriesForDrawing { get; set; }
		public String ErrorCode { get; set; }
	}

	public class WinLossStatement : IWinLossStatement
	{
		public Int32 Year { get; set; }
		public String WinLossValue { get; set; }
	}

    public class CompCashBalanceStatement : ICompCashBalanceStatement
    {
        public String CompCashBalanceValue { get; set; }
    }

    public class UpdatePatronEmailAddress : IUpdatePatronEmailAddress
	{
		public String NewEmailAddress { get; set; }
	}

	public class UpdatePatronPassword : IUpdatePatronPassword
	{
		public String UserName { get; set; }
		public String PatronNumber { get; set; }
		public String Pin { get; set; }
		public String NewPassword { get; set; }
	}

	public class SweepstakesDrawingOffer : ISweepstakesDrawingOffer
	{
		public String Offer { get; set; }
	}
}
