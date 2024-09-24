using Ardalis.SmartEnum;

namespace eShop.MasterData.API.Application.Queries.GetCountries;

public sealed class CountryEnum : SmartEnum<CountryEnum, string>
{
    private CountryEnum(string name, string value) : base(name, value)
    {
    }

    public static readonly CountryEnum Afghanistan = new(nameof(Afghanistan), "AF");
    public static readonly CountryEnum ÅlandIslands = new("Åland Islands", "AX");
    public static readonly CountryEnum Albania = new(nameof(Albania), "AL");
    public static readonly CountryEnum Algeria = new(nameof(Algeria), "DZ");
    public static readonly CountryEnum AmericanSamoa = new("American Samoa", "AS");
    public static readonly CountryEnum Andorra = new(nameof(Andorra), "AD");
    public static readonly CountryEnum Angola = new(nameof(Angola), "AO");
    public static readonly CountryEnum Anguilla = new(nameof(Anguilla), "AI");
    public static readonly CountryEnum Antarctica = new(nameof(Antarctica), "AQ");
    public static readonly CountryEnum AntiguaAndBarbuda = new("Antigua and Barbuda", "AG");
    public static readonly CountryEnum Argentina = new(nameof(Argentina), "AR");
    public static readonly CountryEnum Armenia = new(nameof(Armenia), "AM");
    public static readonly CountryEnum Aruba = new(nameof(Aruba), "AW");
    public static readonly CountryEnum Australia = new(nameof(Australia), "AU");
    public static readonly CountryEnum Austria = new(nameof(Austria), "AT");
    public static readonly CountryEnum Azerbaijan = new(nameof(Azerbaijan), "AZ");
    public static readonly CountryEnum Bahamas = new(nameof(Bahamas), "BS");
    public static readonly CountryEnum Bahrain = new(nameof(Bahrain), "BH");
    public static readonly CountryEnum Bangladesh = new(nameof(Bangladesh), "BD");
    public static readonly CountryEnum Barbados = new(nameof(Barbados), "BB");
    public static readonly CountryEnum Belarus = new(nameof(Belarus), "BY");
    public static readonly CountryEnum Belgium = new(nameof(Belgium), "BE");
    public static readonly CountryEnum Belize = new(nameof(Belize), "BZ");
    public static readonly CountryEnum Benin = new(nameof(Benin), "BJ");
    public static readonly CountryEnum Bermuda = new(nameof(Bermuda), "BM");
    public static readonly CountryEnum Bhutan = new(nameof(Bhutan), "BT");
    public static readonly CountryEnum Bolivia = new("Bolivia", "BO");
    public static readonly CountryEnum BonaireSintEustatiusAndSaba = new("Bonaire, Sint Eustatius and Saba", "BQ");
    public static readonly CountryEnum BosniaAndHerzegovina = new("Bosnia and Herzegovina", "BA");
    public static readonly CountryEnum Botswana = new(nameof(Botswana), "BW");
    public static readonly CountryEnum BouvetIsland = new("Bouvet Island", "BV");
    public static readonly CountryEnum Brazil = new(nameof(Brazil), "BR");
    public static readonly CountryEnum BritishIndianOceanTerritory = new("British Indian Ocean Territory", "IO");
    public static readonly CountryEnum BruneiDarussalam = new("Brunei Darussalam", "BN");
    public static readonly CountryEnum Bulgaria = new(nameof(Bulgaria), "BG");
    public static readonly CountryEnum BurkinaFaso = new("Burkina Faso", "BF");
    public static readonly CountryEnum Burundi = new(nameof(Burundi), "BI");
    public static readonly CountryEnum CaboVerde = new("Cabo Verde", "CV");
    public static readonly CountryEnum Cambodia = new(nameof(Cambodia), "KH");
    public static readonly CountryEnum Cameroon = new(nameof(Cameroon), "CM");
    public static readonly CountryEnum Canada = new(nameof(Canada), "CA");
    public static readonly CountryEnum CaymanIslands = new("Cayman Islands", "KY");
    public static readonly CountryEnum CentralAfricanRepublic = new("Central African Republic", "CF");
    public static readonly CountryEnum Chad = new(nameof(Chad), "TD");
    public static readonly CountryEnum Chile = new(nameof(Chile), "CL");
    public static readonly CountryEnum China = new(nameof(China), "CN");
    public static readonly CountryEnum ChristmasIsland = new("Christmas Island", "CX");
    public static readonly CountryEnum CocosKeelingIslands = new("Cocos (Keeling) Islands", "CC");
    public static readonly CountryEnum Colombia = new(nameof(Colombia), "CO");
    public static readonly CountryEnum Comoros = new(nameof(Comoros), "KM");
    public static readonly CountryEnum Congo = new(nameof(Congo), "CG");
    public static readonly CountryEnum CongoDemocraticRepublicOfThe = new("Congo, Democratic Republic of the", "CD");
    public static readonly CountryEnum CookIslands = new("Cook Islands", "CK");
    public static readonly CountryEnum CostaRica = new("Costa Rica", "CR");
    public static readonly CountryEnum CôteDIvoire = new("Côte d'Ivoire", "CI");
    public static readonly CountryEnum Croatia = new(nameof(Croatia), "HR");
    public static readonly CountryEnum Cuba = new(nameof(Cuba), "CU");
    public static readonly CountryEnum Curaçao = new(nameof(Curaçao), "CW");
    public static readonly CountryEnum Cyprus = new(nameof(Cyprus), "CY");
    public static readonly CountryEnum Czechia = new(nameof(Czechia), "CZ");
    public static readonly CountryEnum Denmark = new(nameof(Denmark), "DK");
    public static readonly CountryEnum Djibouti = new(nameof(Djibouti), "DJ");
    public static readonly CountryEnum Dominica = new(nameof(Dominica), "DM");
    public static readonly CountryEnum DominicanRepublic = new("Dominican Republic", "DO");
    public static readonly CountryEnum Ecuador = new(nameof(Ecuador), "EC");
    public static readonly CountryEnum Egypt = new(nameof(Egypt), "EG");
    public static readonly CountryEnum ElSalvador = new("El Salvador", "SV");
    public static readonly CountryEnum EquatorialGuinea = new("Equatorial Guinea", "GQ");
    public static readonly CountryEnum Eritrea = new(nameof(Eritrea), "ER");
    public static readonly CountryEnum Estonia = new(nameof(Estonia), "EE");
    public static readonly CountryEnum Eswatini = new(nameof(Eswatini), "SZ");
    public static readonly CountryEnum Ethiopia = new(nameof(Ethiopia), "ET");
    public static readonly CountryEnum FalklandIslandsMalvinas = new("Falkland Islands (Malvinas)", "FK");
    public static readonly CountryEnum FaroeIslands = new("Faroe Islands", "FO");
    public static readonly CountryEnum Fiji = new(nameof(Fiji), "FJ");
    public static readonly CountryEnum Finland = new(nameof(Finland), "FI");
    public static readonly CountryEnum France = new(nameof(France), "FR");
    public static readonly CountryEnum FrenchGuiana = new("French Guiana", "GF");
    public static readonly CountryEnum FrenchPolynesia = new("French Polynesia", "PF");
    public static readonly CountryEnum FrenchSouthernTerritories = new("French Southern Territories", "TF");
    public static readonly CountryEnum Gabon = new(nameof(Gabon), "GA");
    public static readonly CountryEnum Gambia = new(nameof(Gambia), "GM");
    public static readonly CountryEnum Georgia = new(nameof(Georgia), "GE");
    public static readonly CountryEnum Germany = new(nameof(Germany), "DE");
    public static readonly CountryEnum Ghana = new(nameof(Ghana), "GH");
    public static readonly CountryEnum Gibraltar = new(nameof(Gibraltar), "GI");
    public static readonly CountryEnum Greece = new(nameof(Greece), "GR");
    public static readonly CountryEnum Greenland = new(nameof(Greenland), "GL");
    public static readonly CountryEnum Grenada = new(nameof(Grenada), "GD");
    public static readonly CountryEnum Guadeloupe = new(nameof(Guadeloupe), "GP");
    public static readonly CountryEnum Guam = new(nameof(Guam), "GU");
    public static readonly CountryEnum Guatemala = new(nameof(Guatemala), "GT");
    public static readonly CountryEnum Guernsey = new(nameof(Guernsey), "GG");
    public static readonly CountryEnum Guinea = new(nameof(Guinea), "GN");
    public static readonly CountryEnum GuineaBissau = new("Guinea-Bissau", "GW");
    public static readonly CountryEnum Guyana = new(nameof(Guyana), "GY");
    public static readonly CountryEnum Haiti = new(nameof(Haiti), "HT");
    public static readonly CountryEnum HeardIslandAndMcDonaldIslands = new("Heard Island and McDonald Islands", "HM");
    public static readonly CountryEnum HolySee = new("Holy See", "VA");
    public static readonly CountryEnum Honduras = new(nameof(Honduras), "HN");
    public static readonly CountryEnum HongKong = new("Hong Kong", "HK");
    public static readonly CountryEnum Hungary = new(nameof(Hungary), "HU");
    public static readonly CountryEnum Iceland = new(nameof(Iceland), "IS");
    public static readonly CountryEnum India = new(nameof(India), "IN");
    public static readonly CountryEnum Indonesia = new(nameof(Indonesia), "ID");
    public static readonly CountryEnum IranIslamicRepublicOf = new("Iran, Islamic Republic of", "IR");
    public static readonly CountryEnum Iraq = new(nameof(Iraq), "IQ");
    public static readonly CountryEnum Ireland = new(nameof(Ireland), "IE");
    public static readonly CountryEnum IsleOfMan = new("Isle of Man", "IM");
    public static readonly CountryEnum Israel = new(nameof(Israel), "IL");
    public static readonly CountryEnum Italy = new(nameof(Italy), "IT");
    public static readonly CountryEnum Jamaica = new(nameof(Jamaica), "JM");
    public static readonly CountryEnum Japan = new(nameof(Japan), "JP");
    public static readonly CountryEnum Jersey = new(nameof(Jersey), "JE");
    public static readonly CountryEnum Jordan = new(nameof(Jordan), "JO");
    public static readonly CountryEnum Kazakhstan = new(nameof(Kazakhstan), "KZ");
    public static readonly CountryEnum Kenya = new(nameof(Kenya), "KE");
    public static readonly CountryEnum Kiribati = new(nameof(Kiribati), "KI");
    public static readonly CountryEnum KoreaDemocraticPeoplesRepublicOf = new("Korea, Democratic People's Republic of", "KP");
    public static readonly CountryEnum KoreaRepublicOf = new("Korea, Republic of", "KR");
    public static readonly CountryEnum Kuwait = new(nameof(Kuwait), "KW");
    public static readonly CountryEnum Kyrgyzstan = new(nameof(Kyrgyzstan), "KG");
    public static readonly CountryEnum LaoPeoplesDemocraticRepublic = new("Lao People's Democratic Republic", "LA");
    public static readonly CountryEnum Latvia = new(nameof(Latvia), "LV");
    public static readonly CountryEnum Lebanon = new(nameof(Lebanon), "LB");
    public static readonly CountryEnum Lesotho = new(nameof(Lesotho), "LS");
    public static readonly CountryEnum Liberia = new(nameof(Liberia), "LR");
    public static readonly CountryEnum Libya = new(nameof(Libya), "LY");
    public static readonly CountryEnum Liechtenstein = new(nameof(Liechtenstein), "LI");
    public static readonly CountryEnum Lithuania = new(nameof(Lithuania), "LT");
    public static readonly CountryEnum Luxembourg = new(nameof(Luxembourg), "LU");
    public static readonly CountryEnum Macao = new(nameof(Macao), "MO");
    public static readonly CountryEnum Madagascar = new(nameof(Madagascar), "MG");
    public static readonly CountryEnum Malawi = new(nameof(Malawi), "MW");
    public static readonly CountryEnum Malaysia = new(nameof(Malaysia), "MY");
    public static readonly CountryEnum Maldives = new(nameof(Maldives), "MV");
    public static readonly CountryEnum Mali = new(nameof(Mali), "ML");
    public static readonly CountryEnum Malta = new(nameof(Malta), "MT");
    public static readonly CountryEnum MarshallIslands = new("Marshall Islands", "MH");
    public static readonly CountryEnum Martinique = new(nameof(Martinique), "MQ");
    public static readonly CountryEnum Mauritania = new(nameof(Mauritania), "MR");
    public static readonly CountryEnum Mauritius = new(nameof(Mauritius), "MU");
    public static readonly CountryEnum Mayotte = new(nameof(Mayotte), "YT");
    public static readonly CountryEnum Mexico = new(nameof(Mexico), "MX");
    public static readonly CountryEnum MicronesiaFederatedStatesOf = new("Micronesia, Federated States of", "FM");
    public static readonly CountryEnum MoldovaRepublicOf = new("Moldova, Republic of", "MD");
    public static readonly CountryEnum Monaco = new(nameof(Monaco), "MC");
    public static readonly CountryEnum Mongolia = new(nameof(Mongolia), "MN");
    public static readonly CountryEnum Montenegro = new(nameof(Montenegro), "ME");
    public static readonly CountryEnum Montserrat = new(nameof(Montserrat), "MS");
    public static readonly CountryEnum Morocco = new(nameof(Morocco), "MA");
    public static readonly CountryEnum Mozambique = new(nameof(Mozambique), "MZ");
    public static readonly CountryEnum Myanmar = new(nameof(Myanmar), "MM");
    public static readonly CountryEnum Namibia = new(nameof(Namibia), "NA");
    public static readonly CountryEnum Nauru = new(nameof(Nauru), "NR");
    public static readonly CountryEnum Nepal = new(nameof(Nepal), "NP");
    public static readonly CountryEnum Netherlands = new(nameof(Netherlands), "NL");
    public static readonly CountryEnum NewCaledonia = new(nameof(NewCaledonia), "NC");
    public static readonly CountryEnum NewZealand = new(nameof(NewZealand), "NZ");
    public static readonly CountryEnum Nicaragua = new(nameof(Nicaragua), "NI");
    public static readonly CountryEnum Niger = new(nameof(Niger), "NE");
    public static readonly CountryEnum Nigeria = new(nameof(Nigeria), "NG");
    public static readonly CountryEnum Niue = new(nameof(Niue), "NU");
    public static readonly CountryEnum NorfolkIsland = new("Norfolk Island", "NF");
    public static readonly CountryEnum NorthMacedonia = new("North Macedonia", "MK");
    public static readonly CountryEnum NorthernMarianaIslands = new("Northern Mariana Islands", "MP");
    public static readonly CountryEnum Norway = new(nameof(Norway), "NO");
    public static readonly CountryEnum Oman = new(nameof(Oman), "OM");
    public static readonly CountryEnum Pakistan = new(nameof(Pakistan), "PK");
    public static readonly CountryEnum Palau = new(nameof(Palau), "PW");
    public static readonly CountryEnum Palestine = new(nameof(Palestine), "PS");
    public static readonly CountryEnum Panama = new(nameof(Panama), "PA");
    public static readonly CountryEnum PapuaNewGuinea = new("Papua New Guinea", "PG");
    public static readonly CountryEnum Paraguay = new(nameof(Paraguay), "PY");
    public static readonly CountryEnum Peru = new(nameof(Peru), "PE");
    public static readonly CountryEnum Philippines = new(nameof(Philippines), "PH");
    public static readonly CountryEnum Pitcairn = new(nameof(Pitcairn), "PN");
    public static readonly CountryEnum Poland = new(nameof(Poland), "PL");
    public static readonly CountryEnum Portugal = new(nameof(Portugal), "PT");
    public static readonly CountryEnum PuertoRico = new("Puerto Rico", "PR");
    public static readonly CountryEnum Qatar = new(nameof(Qatar), "QA");
    public static readonly CountryEnum Réunion = new(nameof(Réunion), "RE");
    public static readonly CountryEnum Romania = new(nameof(Romania), "RO");
    public static readonly CountryEnum RussianFederation = new("Russian Federation", "RU");
    public static readonly CountryEnum Rwanda = new(nameof(Rwanda), "RW");
    public static readonly CountryEnum SaintBarthélemy = new("Saint Barthélemy", "BL");
    public static readonly CountryEnum SaintHelenaAscensionAndTristanDaCunha = new("Saint Helena, Ascension and Tristan da Cunha", "SH");
    public static readonly CountryEnum SaintKittsAndNevis = new("Saint Kitts and Nevis", "KN");
    public static readonly CountryEnum SaintLucia = new("Saint Lucia", "LC");
    public static readonly CountryEnum SaintMartinFrenchPart = new("Saint Martin (French part)", "MF");
    public static readonly CountryEnum SaintPierreAndMiquelon = new("Saint Pierre and Miquelon", "PM");
    public static readonly CountryEnum SaintVincentAndTheGrenadines = new("Saint Vincent and the Grenadines", "VC");
    public static readonly CountryEnum Samoa = new(nameof(Samoa), "WS");
    public static readonly CountryEnum SanMarino = new("San Marino", "SM");
    public static readonly CountryEnum SaoTomeAndPrincipe = new("Sao Tome and Principe", "ST");
    public static readonly CountryEnum SaudiArabia = new(nameof(SaudiArabia), "SA");
    public static readonly CountryEnum Senegal = new(nameof(Senegal), "SN");
    public static readonly CountryEnum Serbia = new(nameof(Serbia), "RS");
    public static readonly CountryEnum Seychelles = new(nameof(Seychelles), "SC");
    public static readonly CountryEnum SierraLeone = new("Sierra Leone", "SL");
    public static readonly CountryEnum Singapore = new(nameof(Singapore), "SG");
    public static readonly CountryEnum SintMaartenDutchPart = new("Sint Maarten (Dutch part)", "SX");
    public static readonly CountryEnum Slovakia = new(nameof(Slovakia), "SK");
    public static readonly CountryEnum Slovenia = new(nameof(Slovenia), "SI");
    public static readonly CountryEnum SolomonIslands = new("Solomon Islands", "SB");
    public static readonly CountryEnum Somalia = new(nameof(Somalia), "SO");
    public static readonly CountryEnum SouthAfrica = new("South Africa", "ZA");
    public static readonly CountryEnum SouthGeorgiaAndTheSouthSandwichIslands = new("South Georgia and the South Sandwich Islands", "GS");
    public static readonly CountryEnum SouthSudan = new("South Sudan", "SS");
    public static readonly CountryEnum Spain = new(nameof(Spain), "ES");
    public static readonly CountryEnum SriLanka = new("Sri Lanka", "LK");
    public static readonly CountryEnum Sudan = new(nameof(Sudan), "SD");
    public static readonly CountryEnum Suriname = new(nameof(Suriname), "SR");
    public static readonly CountryEnum SvalbardAndJanMayen = new("Svalbard and Jan Mayen", "SJ");
    public static readonly CountryEnum Sweden = new(nameof(Sweden), "SE");
    public static readonly CountryEnum Switzerland = new(nameof(Switzerland), "CH");
    public static readonly CountryEnum SyrianArabRepublic = new("Syrian Arab Republic", "SY");
    public static readonly CountryEnum TaiwanProvinceOfChina = new("Taiwan, Province of China", "TW");
    public static readonly CountryEnum Tajikistan = new(nameof(Tajikistan), "TJ");
    public static readonly CountryEnum TanzaniaUnitedRepublicOf = new("Tanzania, United Republic of", "TZ");
    public static readonly CountryEnum Thailand = new(nameof(Thailand), "TH");
    public static readonly CountryEnum TimorLeste = new("Timor-Leste", "TL");
    public static readonly CountryEnum Togo = new(nameof(Togo), "TG");
    public static readonly CountryEnum Tokelau = new(nameof(Tokelau), "TK");
    public static readonly CountryEnum Tonga = new(nameof(Tonga), "TO");
    public static readonly CountryEnum TrinidadAndTobago = new("Trinidad and Tobago", "TT");
    public static readonly CountryEnum Tunisia = new(nameof(Tunisia), "TN");
    public static readonly CountryEnum Türkiye = new("Türkiye", "TR");
    public static readonly CountryEnum Turkmenistan = new(nameof(Turkmenistan), "TM");
    public static readonly CountryEnum TurksAndCaicosIslands = new("Turks and Caicos Islands", "TC");
    public static readonly CountryEnum Tuvalu = new(nameof(Tuvalu), "TV");
    public static readonly CountryEnum Uganda = new(nameof(Uganda), "UG");
    public static readonly CountryEnum Ukraine = new(nameof(Ukraine), "UA");
    public static readonly CountryEnum UnitedArabEmirates = new("United Arab Emirates", "AE");
    public static readonly CountryEnum UnitedKingdom = new("United Kingdom", "GB");
    public static readonly CountryEnum UnitedStatesOfAmerica = new("United States of America", "US");
    public static readonly CountryEnum UnitedStatesMinorOutlyingIslands = new("United States Minor Outlying Islands", "UM");
    public static readonly CountryEnum Uruguay = new(nameof(Uruguay), "UY");
    public static readonly CountryEnum Uzbekistan = new(nameof(Uzbekistan), "UZ");
    public static readonly CountryEnum Vanuatu = new(nameof(Vanuatu), "VU");
    public static readonly CountryEnum VenezuelaBolivarianRepublicOf = new("Venezuela, Bolivarian Republic of", "VE");
    public static readonly CountryEnum VietNam = new("Viet Nam", "VN");
    public static readonly CountryEnum VirginIslandsBritish = new("Virgin Islands (British)", "VG");
    public static readonly CountryEnum VirginIslandsUS = new("Virgin Islands (U.S.)", "VI");
    public static readonly CountryEnum WallisAndFutuna = new("Wallis and Futuna", "WF");
    public static readonly CountryEnum WesternSahara = new("Western Sahara", "EH");
    public static readonly CountryEnum Yemen = new(nameof(Yemen), "YE");
    public static readonly CountryEnum Zambia = new(nameof(Zambia), "ZM");
    public static readonly CountryEnum Zimbabwe = new(nameof(Zimbabwe), "ZW");
}
