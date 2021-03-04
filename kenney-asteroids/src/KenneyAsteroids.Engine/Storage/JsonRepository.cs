using Newtonsoft.Json;
using System;
using System.IO;

namespace KenneyAsteroids.Engine.Storage
{
    public sealed class DefaultInitializerRepositoryDecorator<TItem> : IRepository<TItem>
        where TItem : class, new()
    {
        private readonly IRepository<TItem> _parent;

        public DefaultInitializerRepositoryDecorator(IRepository<TItem> parent)
        {
            _parent = parent;
        }

        public TItem Read()
        {
            var item = _parent.Read();

            if (item == default)
            {
                item = (TItem)Activator.CreateInstance(typeof(TItem));
            }

            return item;
        }

        public void Update(TItem item)
        {
            if (item == default)
            {
                item = (TItem)Activator.CreateInstance(typeof(TItem));
            }

            _parent.Update(item);
        }
    }

    public sealed class JsonRepository<TItem> : IRepository<TItem>
        where TItem : class, new()
    {
        private readonly string _path;

        public JsonRepository(string path)
        {
            _path = path;
        }

        public TItem Read()
        {
            var item = default(TItem);
            
            if (File.Exists(_path))
            {
                var content = File.ReadAllText(_path);
                item = JsonConvert.DeserializeObject<TItem>(content);
            }

            return item;
        }

        public void Update(TItem item)
        {
            var content = JsonConvert.SerializeObject(item);
            File.WriteAllText(_path, content);
        }
    }
}
