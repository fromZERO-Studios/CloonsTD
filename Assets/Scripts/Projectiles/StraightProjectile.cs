using UnityEngine;

namespace CloonsTD {
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class StraightProjectile : Projectile {
        protected override void Launch() => _rigidbody.linearVelocity = transform.right * _speed;
    }
}