using Ardalis.SmartEnum;
using eShop.Shared.Data;

namespace eShop.Customer.Domain.AggregatesModel.CustomerAggregate;

public class CardType(string name, int value)
        : SmartEnum<CardType, int>(name, value), IAggregateRoot
{
    public static readonly CardType Amex = new(nameof(Amex), 1);
    public static readonly CardType Visa = new(nameof(Visa), 2);
    public static readonly CardType MasterCard = new(nameof(MasterCard), 3);
}
