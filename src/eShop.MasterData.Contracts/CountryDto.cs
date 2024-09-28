namespace eShop.MasterData.Contracts;

public class CountryDto(string code, string name)
{
    public string Code { get; set; } = code;
    public string Name { get; set; } = name;
}
