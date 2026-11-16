using UnityEngine;

namespace CloonsTD;

public class Cloon : PooledObject {
    [SerializeField] SpriteRenderer spriteRenderer; // O MeshRenderer si es 3D

    CloonData data;
    int currentHealth;
    float distanceTraveled;
    bool isDead;

    public CloonData Data => data;
    public float DistanceTraveled => distanceTraveled;

    public void Initialize(CloonData cloonData, float startDistance = 0f) {
        data = cloonData;
        currentHealth = data.maxHealth;
        distanceTraveled = startDistance;
        isDead = false;

        transform.localScale = data.scale;
        if (spriteRenderer != null)
            spriteRenderer.color = data.tintColor;
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
                    float offset = distanceTraveled - (entry.pathSpacingOffset * c);
                    var child = PoolManager.Spawn(entry.cloonData.prefab, transform.position, transform.rotation);
                    child.Initialize(entry.cloonData, offset);

                    if (spilloverDamage > 0)
                        child.TakeDamage(spilloverDamage);
                }
            }
        }

  
        Release();
    }


}

