using UnityEngine;

namespace CloonsTD {
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class BoomerangProjectile : Projectile {
        private Vector2 _start;
        private Vector2 _tip;
        private float _flightDuration;
        private float _elapsed;

        protected override void Launch() {
            _start = transform.position;
            _tip = _start + (Vector2)transform.right * _range;
            _flightDuration = 2f * _range / _speed;
            _elapsed = 0f;
        }

        private void FixedUpdate() {
            _elapsed += Time.fixedDeltaTime;
            if (_elapsed >= _flightDuration) {
                Release();
                return;
            }

            var progress = Mathf.Sin(_elapsed / _flightDuration * Mathf.PI); // math magic for slowing down at the tip and speeding up on the way back
            _rigidbody.MovePosition(Vector2.Lerp(_start, _tip, progress));
        }
    }
}