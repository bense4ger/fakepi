using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FakePI.Lib.Core;
using FakePI.Lib.Data.Contexts.Contracts;
using FakePI.Lib.Entities;
using FakePI.Lib.Util.Contracts;
using Microsoft.Extensions.Options;

namespace FakePI.Lib.Data.Contexts
{
    public class JsonFileContext<T> : IDataContext<T> where T: Entity
    {
        public JsonFileContext(IFileReader fileReader, 
            IJsonConverter jsonConverter,
            IOptions<AppConfig> config)
        {
            FileReader = fileReader;
            JsonConverter = jsonConverter;
            
            DataStorePath = config.Value?.DataStorePath;
            Filename = $"{typeof(T).Name}.json";
            
            if(!FileReader.FileExists(Path.Combine(DataStorePath, Filename))) throw new Exception($"Unable to load file {Path.Combine(DataStorePath, Filename)}");
        }
        
        private IFileReader FileReader { get; }
        private IJsonConverter JsonConverter { get; }
        
        private string DataStorePath { get; }
        private string Filename { get; }
        
        private ICollection<T> SimpleCache { get; set; }
        
        public async Task<T> GetAsync(Guid id)
        {
            return GetFromCache(id);
        }

        private T GetFromCache(Guid id)
        {
            if (SimpleCache == null) PopulateCache();
            
            return SimpleCache?.FirstOrDefault(x => x.Id == id);
        }
        
        
        public async Task<ICollection<T>> GetAllAsync()
        {
            return GetAllFromCache();
        }

        private ICollection<T> GetAllFromCache()
        {
            if (SimpleCache == null) PopulateCache();
            return SimpleCache;
        }
        
        public async Task<bool> PutAsync(T entity)
        {
            throw new NotSupportedException($"{nameof(PutAsync)} is not a supported operation");
        }

        public async Task<bool> PutManyAsync(ICollection<T> entities)
        {
            throw new NotSupportedException($"{nameof(PutManyAsync)} is not a supported operation");
        }
        
        #region Helpers

        private void PopulateCache()
        {
            var rawData = FileReader.ReadFile(Path.Combine(DataStorePath, Filename));
            SimpleCache = JsonConverter.FromJson<ICollection<T>>(rawData);
        }
        
        #endregion
    }
}