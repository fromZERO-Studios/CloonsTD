using UnityEngine;

namespace CloonsTD {
    [CreateAssetMenu(fileName = "MonkeyData", menuName = "Scriptable Objects/MonkeyData")]
    public class MonkeyData : ScriptableObject {
        public Projectile projectilePrefab;

        public int range;
        public int fireRate;

        public int projectilesPerShot;
        public float spreadAngle;

        public bool aimsAtTarget;
    }
}
