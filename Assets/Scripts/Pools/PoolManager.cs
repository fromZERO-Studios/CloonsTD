using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace CloonsTD {
    public class PoolManager : MonoBehaviour {
        static PoolManager instance;
        readonly Dictionary<PooledObject, ObjectPool<PooledObject>> pools = new();

        void Awake() => instance = this;

        public static T Spawn<T>(T prefab, Vector3 pos, Quaternion rot) where T : PooledObject {
            var obj = instance.GetPool(prefab).Get();
            obj.transform.SetPositionAndRotation(pos, rot);
            obj.gameObject.SetActive(true);
            obj.OnSpawn();
            return (T)obj;
        }

        ObjectPool<PooledObject> GetPool(PooledObject prefab) {
            if (pools.TryGetValue(prefab, out var existing)) return existing;

            ObjectPool<PooledObject> pool = null;
            pool = new ObjectPool<PooledObject>(
                createFunc: () => {
                    var o = Instantiate(prefab, transform);
                    o.gameObject.SetActive(false);
                    o.pool = pool;
                    return o;
                },
                actionOnRelease: o => { o.OnDespawn(); o.gameObject.SetActive(false); },
                actionOnDestroy: o => Destroy(o.gameObject),
                collectionCheck: true,   // catches double-release in the editor
                defaultCapacity: 32,
                maxSize: 500);

            pools[prefab] = pool;
            return pool;
        }
    }
}
