using System;

namespace MobilePatronsApp.DataContracts.Interfaces
{
	public interface IOffer
	{	
		String PromoCode { get; set; }
		DateTime PromoStartDate { get; set; }
		DateTime PromoEndDate { get; set; }
		String PromoText { get; set; }
		String PromoType { get; set; }
		String ErrorCode { get; set; }
	}

	public interface IPowerReward
	{
		String PromoAmount { get; set; }
		String PromoCode { get; set; }
		String PromoStartDate { get; set; }
		String PromoStartTime { get; set; }
		String PromoEndDate { get; set; }
		String PromoEndTime { get; set; }
		String PromoStatus { get; set; }
		String PromoText { get; set; }
		String ErrorCode { get; set; }
	}

	public interface ISweepstake
	{
		String Activation { get; set; }
		String ActiveEntries { get; set; }
		String BonusEntries { get; set; }
		String SweepstakesCode { get; set; }
		String StartDate { get; set; }
		String StartTime { get; set; }
		String EndDate { get; set; }
		String EndTime { get; set; }
		String FreeEntries { get; set; }
		String TotalEntries { get; set; }
		String Description { get; set; }
		String ErrorCode { get; set; }
	}

	public interface ISweepstakeDrawing
	{
		String Activation { get; set; }
		String StartDate { get; set; }
		String StartTime { get; set; }
		String EndDate { get; set; }
		String EndTime { get; set; }
		String DrawingDate { get; set; }
		String DrawingTime { get; set; }
		String EntriesForDrawing { get; set; }
		String ErrorCode { get; set; }
	}

	public interface IWinLossStatement
	{
		Int32 Year { get; set; }
		String WinLossValue { get; set; }
	}

	public interface ICompCashBalanceStatement
	{
		String CompCashBalanceValue { get; set; }
	}

	public interface IUpdatePatronEmailAddress
	{
		String NewEmailAddress { get; set; }
	}

	public interface IUpdatePatronPassword
	{
		String UserName { get; set; }
		String PatronNumber { get; set; }
		String Pin { get; set; }
		String NewPassword { get; set; }
	}

	public interface ISweepstakesDrawingOffer
	{
		String Offer { get; set; }
	}
}
