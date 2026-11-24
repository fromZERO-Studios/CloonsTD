using UnityEngine;

namespace CloonsTD {
    [CreateAssetMenu(fileName = "MonkeyData", menuName = "Scriptable Objects/MonkeyData")]
    public class MonkeyData : ScriptableObject {
        public Projectile projectilePrefab;
        public float damage;
        public int pierce;
        public float projectileSpeed;

        public float range;
        public float fireRate;

        public int projectilesPerShot;
        public float spreadAngle;

        public bool aimsAtTarget;
        public MonkeyUpgrade[] upgrades;
    }
}
