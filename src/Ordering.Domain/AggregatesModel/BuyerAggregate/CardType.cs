using Ardalis.SmartEnum;
using eShop.Shared.Data;

namespace eShop.Ordering.Domain.AggregatesModel.BuyerAggregate;

/// <remarks> 
/// Card type class should be marked as abstract with protected constructor to encapsulate known enum types
/// this is currently not possible as OrderingContextSeed uses this constructor to load cardTypes from csv file
/// </remarks>
public class CardType(string name, int value)
        : SmartEnum<CardType, int>(name, value), IAggregateRoot
{    
    public static readonly CardType Amex = new(nameof(Amex), 1);
    public static readonly CardType Visa = new(nameof(Visa), 2);
    public static readonly CardType MasterCard = new(nameof(MasterCard), 3);
}
