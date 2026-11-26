using UnityEngine;

namespace CloonsTD {
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public abstract class Projectile : PooledObject {
        private const string TARGET_TAG = "Cloon";

        protected float _damage;
        protected int _maxPierceCount;
        protected int _currentPierceCount;
        protected float _speed;
        protected float _range;
        protected bool _destroyOnRangeExit;

        protected Vector2 _originPos;

        protected Rigidbody2D _rigidbody;
        protected Collider2D _collider;

        private void Awake() {
            _rigidbody = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
        }

        public void Init(MonkeyStats stats) {
            _damage = stats.damage;
            _maxPierceCount = stats.pierce;
            _speed = stats.projectileSpeed;
            _range = stats.range;
            _destroyOnRangeExit = stats.destroyProjectileOnRangeExit;

            _originPos = (Vector2)transform.position;

            Launch();
        }

        public override void OnSpawn() {
            _currentPierceCount = 0;
        }

        protected virtual void Launch() { }

        protected virtual void FixedUpdate() {
            if (!_destroyOnRangeExit) {
                return;
            }

            if ((_rigidbody.position - _originPos).sqrMagnitude > _range * _range) {
                Release();
            }
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (!other.CompareTag(TARGET_TAG)) return;

            if (_currentPierceCount >= _maxPierceCount) return;

            _currentPierceCount++;
            if (_currentPierceCount >= _maxPierceCount) {
                Release();
            }

            // TODO: Damage the target here
            print($"Projectile hit {other.name} for {_damage} damage. Pierce count: {_currentPierceCount}/{_maxPierceCount}");
        }

    }
}