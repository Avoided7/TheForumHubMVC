using System.Runtime.CompilerServices;
using System.Text.Json;

namespace TheForumHubMVC.Data.Helpers
{
    public static class SessionHelper
    {
        public static void SetJson(this ISession sesion, string key, object value)
        {
            sesion.SetString(key, JsonSerializer.Serialize(value));
        }
        public static T GetJson<T>(this ISession session, string key)
        {
            var result = session.GetString(key);
            return result == null ? default(T) : JsonSerializer.Deserialize<T>(result);
        }
    }
}
