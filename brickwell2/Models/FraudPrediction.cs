using System;
using System.Collections.Generic;

namespace brickwell2.Models;

public partial class FraudPrediction
{
    public int? Time { get; set; }

    public int? Amount { get; set; }

    public int? DayOfWeekMon { get; set; }

    public int? DayOfWeekSat { get; set; }

    public int? DayOfWeekSun { get; set; }

    public int? DayOfWeekThu { get; set; }

    public int? DayOfWeekTue { get; set; }

    public int? DayOfWeekWed { get; set; }

    public int? EntryModePin { get; set; }

    public int? EntryModeTap { get; set; }

    public int? TypeOfTransactionOnline { get; set; }

    public int? TypeOfTransactionPos { get; set; }

    public int? CountryOfTransactionIndia { get; set; }

    public int? CountryOfTransactionRussia { get; set; }

    public int? CountryOfTransactionUsa { get; set; }

    public int? CountryOfTransactionUnitedKingdom { get; set; }

    public int? ShippingAddressIndia { get; set; }

    public int? ShippingAddressRussia { get; set; }

    public int? ShippingAddressUsa { get; set; }

    public int? ShippingAddressUnitedKingdom { get; set; }

    public int? BankHsbc { get; set; }

    public int? BankHalifax { get; set; }

    public int? BankLloyds { get; set; }

    public int? BankMetro { get; set; }

    public int? BankMonzo { get; set; }

    public int? BankRbs { get; set; }

    public int? TypeOfCardVisa { get; set; }
}
