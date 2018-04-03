using FakePI.Lib.Entities;
using FakePI.Lib.Util.Contracts;
using Newtonsoft.Json;

namespace FakePI.Lib.Util
{
    public class JsonConverter : IJsonConverter
    {
        public T FromJson<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public string ToJson<T>(T entity)
        {
            return JsonConvert.SerializeObject(entity);
        }
    }
}