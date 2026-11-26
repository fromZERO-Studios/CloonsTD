using UnityEngine;
using UnityEngine.Pool;

namespace CloonsTD {
    public abstract class PooledObject : MonoBehaviour {
        internal IObjectPool<PooledObject> pool;

        public void Release() => pool.Release(this);

        public virtual void OnSpawn() { }    // reset state here
        public virtual void OnDespawn() { }  // stop coroutines, clear trails, etc.
    }
}
