using Microsoft.AspNetCore.Http;
 
using Newtonsoft.Json;
 
 /// <summary>Extension to add the session in the application</summary>
public static class SessionExtensions
{

    /// <summary>Add an element to the session</summary>
    public static void Set(this ISession session, string key, object value) 
    {
        session.SetString(key, JsonConvert.SerializeObject(value));
    }

    /// <summary>Get an element in the session</summary>
    public static T Get<T>(this ISession session, string key)
    {
        string value = session.GetString(key);
        return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
    }

}