using UnityEngine;

namespace CloonsTD {
    [CreateAssetMenu(fileName = "MonkeyData", menuName = "Scriptable Objects/MonkeyData")]
    public class MonkeyData : ScriptableObject {
        public Transform projectilePrefab;

        public float range;
        public float fireRate;

        public int projectilesPerShot;
        public float spreadAngle;

        public bool aimsAtTarget;
    }
}
