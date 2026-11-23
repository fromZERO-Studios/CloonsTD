using UnityEngine;

namespace CloonsTD {

    public class Cloon : PooledObject {
        CloonData data;
        int currentHealth;
        float distanceTraveled;
        float movespeed;
        bool isDead;

        private Transform checkpoint;
        private int index = 0;
        public CloonData Data => data;
        public float DistanceTraveled => distanceTraveled;

        private Rigidbody2D rb;

        void Awake() {
            rb = GetComponent<Rigidbody2D>();
        }

        public void Initialize(CloonData cloonData, float startDistance = 0f, int startCheckpointIndex = 0) {
            data = cloonData;
            currentHealth = data.maxHealth;
            movespeed = data.speed;
            gameObject.name = data.cloonName;
            distanceTraveled = startDistance;
            isDead = false;

            transform.localScale = data.scale;

            if (CloonManager.main != null && CloonManager.main.checkpoints.Length > 0) {
                index = Mathf.Clamp(startCheckpointIndex, 0, CloonManager.main.checkpoints.Length - 1);
                checkpoint = CloonManager.main.checkpoints[index];

                if (startCheckpointIndex == 0 && startDistance == 0f) {
                    transform.position = checkpoint.position;
                }
            }
        }

        void FixedUpdate() {
            if (isDead || checkpoint == null) return;
            
            float step = movespeed * Time.fixedDeltaTime;
            Vector2 currentPos = rb.position;
            Vector2 targetPos = checkpoint.position;

            
            Vector2 direction = (targetPos - currentPos).normalized;
            if (direction != Vector2.zero) {
                transform.right = direction;
            }

            
            Vector2 nextPos = Vector2.MoveTowards(currentPos, targetPos, step);
            rb.MovePosition(nextPos);
            distanceTraveled += Vector2.Distance(currentPos, nextPos);

            
            if (Vector2.Distance(nextPos, targetPos) <= 0.01f) {
                index++;

                if (index >= CloonManager.main.checkpoints.Length) {
                    
                    Release();
                    return;
                }

                checkpoint = CloonManager.main.checkpoints[index];
            }
        }

        public override void OnSpawn() {
            isDead = false;
        }

        public override void OnDespawn() {
            data = null;
        }

        public void TakeDamage(int amount) {
            if (isDead || data == null) return;

            currentHealth -= amount;

            if (currentHealth <= 0) {
                int spilloverDamage = Mathf.Abs(currentHealth);
                Pop(spilloverDamage);
            }
        }

        void Pop(int spilloverDamage) {
            isDead = true;

            // EconomyManager.AddCash(data.cashReward);

            if (data.childrenOnPop != null) {
                for (int i = 0; i < data.childrenOnPop.Length; i++) {
                    var entry = data.childrenOnPop[i];
                    for (int c = 0; c < entry.count; c++) {
                        float distance = distanceTraveled;
                        var child = PoolManager.Spawn(entry.cloonData.prefab, transform.position, transform.rotation);
                        child.Initialize(entry.cloonData, distance);

                        if (spilloverDamage > 0)
                            child.TakeDamage(spilloverDamage);
                    }
                }
            }


            Release();
        }


    }
}
