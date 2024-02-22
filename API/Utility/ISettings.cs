using Config.Net;

namespace API.Utility;

public interface ISettings
{
    public string CachePath { get; set; }
}