namespace API.Utility;

public interface ISettings
{
    public string ConnectionString { get; set; }
    public string Secret { get; set; }
}