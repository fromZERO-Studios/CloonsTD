using UnityEngine;

namespace CloonsTD {

    public class CloonManager : MonoBehaviour {
        public static CloonManager main;
        public Transform[] checkpoints;

        [Header("Prueba rápida")]
        public CloonData testCloonData; 

        private void Awake() {
            main = this;
        }

        private void Start() {
            if (testCloonData != null) {
                SpawnCloon(testCloonData);
            }
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Space) && testCloonData != null) {
                SpawnCloon(testCloonData);
            }
        }

        public void SpawnCloon(CloonData cloonData) {
            Vector3 spawnPos = checkpoints.Length > 0 ? checkpoints[0].position : Vector3.zero;
            Cloon cloon = PoolManager.Spawn(cloonData.prefab, spawnPos, Quaternion.identity);
            cloon.Initialize(cloonData);
        }
    }
}