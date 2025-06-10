namespace SnapSell.Presistance.Context;

public interface IMongoDbSettings
{
    string ConnectionString { get; set; }
    string DatabaseName { get; set; }
}

public class MongoDbSettings : IMongoDbSettings
{
    public const string SectionName = "MongoSetting";
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;

}