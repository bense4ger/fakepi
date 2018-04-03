using System.Globalization;
using FakePI.Lib.Entities;

namespace FakePI.Lib.Util.Contracts
{
    public interface IJsonConverter
    {
        T FromJson<T>(string json);
        string ToJson<T>(T entity);
    }
}