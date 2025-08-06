namespace velowrench.Core.Interfaces;

public interface ILocalizer
{
    string GetString(string key);
    string GetString(string key, params object[] args);
}
