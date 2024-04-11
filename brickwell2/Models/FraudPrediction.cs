using System.ComponentModel.DataAnnotations;

namespace brickwell2.Models;

public class FraudPrediction
{
    [Key] 
    public int transaction_ID { get; set; }
    public int time {get; set;}
    public int amount {get; set;}
    public int day_of_week_Mon {get; set;}
    public int day_of_week_Sat {get; set;}
    public int day_of_week_Sun {get; set;}
    public int day_of_week_Thu {get; set;}
    public int day_of_week_Tue {get; set;}
    public int day_of_week_Wed {get; set;}
    public int entry_mode_PIN {get; set;}
    public int entry_mode_Tap {get; set;}
    public int type_of_transaction_Online {get; set;}
    public int type_of_transaction_POS {get; set;}
    public int country_of_transaction_India {get; set;}
    public int country_of_transaction_Russia {get; set;}
    public int country_of_transaction_USA {get; set;}
    public int country_of_transaction_UnitedKingdom {get; set;}
    public int shipping_address_India {get; set;}
    public int shipping_address_Russia {get; set;}
    public int shipping_address_USA {get; set;}
    public int shipping_address_UnitedKingdom {get; set;}
    public int bank_HSBC {get; set;}
    public int bank_Halifax {get; set;}
    public int bank_Lloyds {get; set;}
    public int bank_Metro {get; set;}
    public int bank_Monzo {get; set;}
    public int bank_RBS {get; set;}
    public int type_of_card_Visa {get; set;}

}