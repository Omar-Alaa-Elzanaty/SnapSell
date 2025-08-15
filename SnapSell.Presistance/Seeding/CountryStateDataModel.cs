namespace SnapSell.Presistance.Seeding;


public class CountryStateDataModel
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string NamePrimaryLang { get; set; }
    public string NameSecondaryLang { get; set; }
    public List<CityDataModel> CityDataModels { get; set; } = [];
}

public class CityDataModel
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string NamePrimaryLang { get; set; }
    public string NameSecondaryLang { get; set; }
    public int CountryStateDataModelId { get; set; }
    public CountryStateDataModel CountryStateDataModel { get; set; }
    public List<AreaDataModel> AreaDataModels { get; set; } = [];
}

public class AreaDataModel
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string NamePrimaryLang { get; set; }
    public string NameSecondaryLang { get; set; }
    public int CityDataModelId { get; set; }
    public CityDataModel CityDataModel { get; set; }
    public List<StreetDataModel> StreetDataModels { get; set; } = [];
}

public class StreetDataModel
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string NamePrimaryLang { get; set; }
    public string NameSecondaryLang { get; set; }
    public int AreaDataModelId { get; set; }
    public AreaDataModel AreaDataModel { get; set; }
}