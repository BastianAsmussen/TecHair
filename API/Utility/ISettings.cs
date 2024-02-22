using Config.Net;

namespace API.Utility;

public interface ISettings
{
    string ConnectionString { get; }
}