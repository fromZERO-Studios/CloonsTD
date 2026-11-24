using System;
using UnityEngine;

namespace CloonsTD {
    [Serializable]
    public struct MonkeyStats {
        public Projectile projectilePrefab;
        public float projectileSpeed;
        public bool destroyProjectileOnRangeExit;

        public float damage;
        public int pierce;
        public float range;
        public float fireRate;
    }
}