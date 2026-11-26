using System;
using UnityEngine;

namespace CloonsTD {
    [Serializable]
    public struct MonkeyUpgrade {
        public string upgradeName;
        public int cost;
        public float damageBonus;
        public int pierceBonus;
        public float rangeBonus;
        public float fireRateMultiplier;
        public Projectile projectilePrefabOverride; // null = keep current
        public Sprite spriteOverride; // null = keep current
    }
}