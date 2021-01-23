﻿using System.Collections;
using System.Collections.Generic;

namespace KenneyAsteroids.Engine
{
    public sealed class EntityCollection : IEnumerable<IEntity>
    {
        private readonly List<IEntity> _entities;
        private readonly List<Modification> _modifications;

        public EntityCollection()
        {
            _entities = new List<IEntity>();
            _modifications = new List<Modification>();
        }

        public void Commit()
        {
            _modifications.Iter(x =>
            {
                switch (x.Type)
                {
                    case ModificationType.Add:
                        _entities.Add(x.Entity);
                        break;

                    case ModificationType.Remove:
                        _entities.Remove(x.Entity);
                        break;
                }
            });

            _modifications.Clear();
        }

        public void Add(IEntity entity)
        {
            var modification = new Modification
            {
                Type = ModificationType.Add,
                Entity = entity
            };

            _modifications.Add(modification);
        }

        public void Remove(IEntity entity)
        {
            var modification = new Modification
            {
                Type = ModificationType.Remove,
                Entity = entity
            };

            _modifications.Add(modification);
        }

        public IEnumerator<IEntity> GetEnumerator()
        {
            return _entities.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _entities.GetEnumerator();
        }

        private enum ModificationType
        {
            Add,
            Remove
        }

        private sealed class Modification
        {
            public IEntity Entity { get; set; }

            public ModificationType Type { get; set; }
        }
    }
}
