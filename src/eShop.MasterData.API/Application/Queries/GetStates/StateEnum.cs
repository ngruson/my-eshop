using Ardalis.SmartEnum;

namespace eShop.MasterData.API.Application.Queries.GetStates;

public sealed class StateEnum : SmartEnum<StateEnum, string>
{
    private StateEnum(string name, string value) : base(name, value)
    {
    }

    public static readonly StateEnum Alabama = new(nameof(Alabama), "AL");
    public static readonly StateEnum Alaska = new(nameof(Alaska), "AK");
    public static readonly StateEnum Arizona = new(nameof(Arizona), "AZ");
    public static readonly StateEnum Arkansas = new(nameof(Arkansas), "AR");
    public static readonly StateEnum California = new(nameof(California), "CA");
    public static readonly StateEnum Colorado = new(nameof(Colorado), "CO");
    public static readonly StateEnum Connecticut = new(nameof(Connecticut), "CT");
    public static readonly StateEnum Delaware = new(nameof(Delaware), "DE");
    public static readonly StateEnum Florida = new(nameof(Florida), "FL");
    public static readonly StateEnum Georgia = new(nameof(Georgia), "GA");
    public static readonly StateEnum Hawaii = new(nameof(Hawaii), "HI");
    public static readonly StateEnum Idaho = new(nameof(Idaho), "ID");
    public static readonly StateEnum Illinois = new(nameof(Illinois), "IL");
    public static readonly StateEnum Indiana = new(nameof(Indiana), "IN");
    public static readonly StateEnum Iowa = new(nameof(Iowa), "IA");
    public static readonly StateEnum Kansas = new(nameof(Kansas), "KS");
    public static readonly StateEnum Kentucky = new(nameof(Kentucky), "KY");
    public static readonly StateEnum Louisiana = new(nameof(Louisiana), "LA");
    public static readonly StateEnum Maine = new(nameof(Maine), "ME");
    public static readonly StateEnum Maryland = new(nameof(Maryland), "MD");
    public static readonly StateEnum Massachusetts = new(nameof(Massachusetts), "MA");
    public static readonly StateEnum Michigan = new(nameof(Michigan), "MI");
    public static readonly StateEnum Minnesota = new(nameof(Minnesota), "MN");
    public static readonly StateEnum Mississippi = new(nameof(Mississippi), "MS");
    public static readonly StateEnum Missouri = new(nameof(Missouri), "MO");
    public static readonly StateEnum Montana = new(nameof(Montana), "MT");
    public static readonly StateEnum Nebraska = new(nameof(Nebraska), "NE");
    public static readonly StateEnum Nevada = new(nameof(Nevada), "NV");
    public static readonly StateEnum NewHampshire = new("New Hampshire", "NH");
    public static readonly StateEnum NewJersey = new("New Jersey", "NJ");
    public static readonly StateEnum NewMexico = new("New Mexico", "NM");
    public static readonly StateEnum NewYork = new("New York", "NY");
    public static readonly StateEnum NorthCarolina = new("North Carolina", "NC");
    public static readonly StateEnum NorthDakota = new("North Dakota", "ND");
    public static readonly StateEnum Ohio = new(nameof(Ohio), "OH");
    public static readonly StateEnum Oklahoma = new(nameof(Oklahoma), "OK");
    public static readonly StateEnum Oregon = new(nameof(Oregon), "OR");
    public static readonly StateEnum Pennsylvania = new(nameof(Pennsylvania), "PA");
    public static readonly StateEnum RhodeIsland = new("Rhode Island", "RI");
    public static readonly StateEnum SouthCarolina = new("South Carolina", "SC");
    public static readonly StateEnum SouthDakota = new("South Dakota", "SD");
    public static readonly StateEnum Tennessee = new(nameof(Tennessee), "TN");
    public static readonly StateEnum Texas = new(nameof(Texas), "TX");
    public static readonly StateEnum Utah = new(nameof(Utah), "UT");
    public static readonly StateEnum Vermont = new(nameof(Vermont), "VT");
    public static readonly StateEnum Virginia = new(nameof(Virginia), "VA");
    public static readonly StateEnum Washington = new(nameof(Washington), "WA");
    public static readonly StateEnum WestVirginia = new("West Virginia", "WV");
    public static readonly StateEnum Wisconsin = new(nameof(Wisconsin), "WI");
    public static readonly StateEnum Wyoming = new(nameof(Wyoming), "WY");
}
