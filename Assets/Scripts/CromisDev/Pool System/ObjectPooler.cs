using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CromisDev.Pooling
{

    [System.Serializable]
    public class ObjectPooler<T> where T : MonoBehaviour, IPoolable
    {
        private readonly List<T> POOL = new();
        private readonly T PREFAB_REFERENCE;
        private readonly Transform PARENT_REFERENCE;

        public List<T> Pool => POOL;

        public int ActiveObjects => Pool.Where(p => !p.Pooled).Count();
        public int PooledObjects => Pool.Where(p => p.Pooled).Count();

        private T Create()
        {
            T obj = Object.Instantiate(PREFAB_REFERENCE);
            obj.Hide();
            obj.name = PREFAB_REFERENCE.name;

            if (PARENT_REFERENCE)
            {
                obj.SetParent(PARENT_REFERENCE);
            }

            POOL.Add(obj);

            return obj;
        }

        public ObjectPooler(T prefab, uint amountToPool, Transform parent = null)
        {
            PREFAB_REFERENCE = prefab;
            PARENT_REFERENCE = parent;

            for (ushort i = ushort.MinValue; i < amountToPool; i++)
            {
                Create();
            }
        }

        public T GetPooledObject(bool increaseOnEmpty = false)
        {
            T availableItem = POOL.Find(item => item.Pooled && item.ShouldSpawn());
            if (availableItem != null) return availableItem;
            return increaseOnEmpty ? Create() : null;
        }
    }
}
