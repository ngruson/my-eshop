using CsvHelper.Configuration.Attributes;

namespace eShop.Customer.Infrastructure.Seed;

internal class CustomerCsv
{
    [Index(0)]
    public int Id { get; set; }

    [Index(1)]
    public string? FirstName { get; set; }

    [Index(2)]
    public string? LastName { get; set; }

    [Index(3)]
    public string? UserName { get; set; }

    [Index(4)]
    public string? Email { get; set; }

    [Index(5)]
    public string? CardHolderName { get; set; }

    [Index(6)]
    public string? CardNumber { get; set; }

    [Index(7)]
    public string? City { get; set; }

    [Index(8)]
    public string? Country { get; set; }

    [Index(9)]
    public string? PhoneNumber { get; set; }

    [Index(10)]
    public string? ZipCode { get; set; }

    [Index(11)]
    public string? State { get; set; }

    [Index(12)]
    public string? Street { get; set; }
}
