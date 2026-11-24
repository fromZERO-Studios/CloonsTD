using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace CloonsTD {
    [RequireComponent(typeof(CircleCollider2D))]

    [RequireComponent(typeof(SpriteRenderer))]
    public class MonkeyHandler : MonoBehaviour {

        private const string TARGET_TAG = "Cloon";

        private readonly List<Transform> inRange = new();
        private CircleCollider2D _bloonCollider;

        [SerializeField]
        private MonkeyData _data;

        private SpriteRenderer _spriteRenderer;

        private MonkeyStats _stats;
        private int _upgradeLevel;

        private Transform _target;

        public bool CanUpgrade => _upgradeLevel < _data.upgrades.Length;
        public MonkeyUpgrade? NextUpgrade => CanUpgrade ? _data.upgrades[_upgradeLevel] : null;


        private void LookAt2D(Transform target) {
            // print("LookAt2D: " + target.name);
            // transform.LookAt only works in 3D
            var direction = target.position - transform.position;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // get the angle and convert it from radians to degrees
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        IEnumerator Shoot() {
            while (true) {
                if (_target != null) FireVolley();
                yield return new WaitForSeconds(1f / _stats.fireRate);
            }
        }

        private void FireVolley() {
            var count = _data.projectilesPerShot;
            var isFullCircle = _data.spreadAngle >= 360f;
            // A full circle has no edges, so divide by count to avoid stacking the first and last shot.
            var step = count <= 1 ? 0f : _data.spreadAngle / (isFullCircle ? count : count - 1);
            var startAngle = isFullCircle ? 0f : -_data.spreadAngle / 2f;

            for (var i = 0; i < count; i++) {
                var rotation = transform.rotation * Quaternion.Euler(0, 0, startAngle + step * i);
                var projectile = PoolManager.Spawn(_stats.projectilePrefab, transform.position, rotation);
                projectile.Init(_stats);
            }
        }

        private void Awake() {
            _stats = new MonkeyStats {
                projectilePrefab = _data.projectilePrefab,
                projectileSpeed = _data.projectileSpeed,
                destroyProjectileOnRangeExit = _data.destroyProjectileOnRangeExit,

                damage = _data.damage,
                pierce = _data.pierce,
                range = _data.range,
                fireRate = _data.fireRate
            };
            _bloonCollider = GetComponent<CircleCollider2D>();
            _bloonCollider.radius = _data.range;
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void Upgrade(MonkeyUpgrade upgrade) {
            if (!CanUpgrade) {
                print($"Can't upgrade monkey {name}");
                return;
            }

            var newUpgrade = _data.upgrades[_upgradeLevel];

            _stats.damage += newUpgrade.damageBonus;
            _stats.pierce += newUpgrade.pierceBonus;
            _stats.range += newUpgrade.rangeBonus;
            _stats.fireRate *= newUpgrade.fireRateMultiplier;

            if (upgrade.projectilePrefabOverride != null) {
                _stats.projectilePrefab = upgrade.projectilePrefabOverride;
            }

            if (upgrade.spriteOverride != null) {
                _spriteRenderer.sprite = upgrade.spriteOverride;
            }

            _bloonCollider.radius = _stats.range;
        }

        private void Start() {
            StartCoroutine(Shoot());
        }

        private void Update() {
            if (_target == null) {
                inRange.RemoveAll(t => t == null); // remove all dead entries
                _target = inRange.Count > 0 ? inRange[0] : null; // try to get a new target if we don't have one
            }

            if (_target == null || !_data.aimsAtTarget) return;

            LookAt2D(_target);
        }

        private void OnTriggerEnter2D(Collider2D other) {
            print("OnTriggerEnter2D: " + other.name);
            if (other.CompareTag(TARGET_TAG))
                inRange.Add(other.transform);
        }

        private void OnTriggerExit2D(Collider2D other) {
            print("OnTriggerExit2D: " + other.name);
            inRange.Remove(other.transform);
            if (other.transform == _target) _target = null;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected() {
            if (_data == null) return;
            UnityEditor.Handles.color = new Color(1f, 0f, 0f, 0.05f);
            UnityEditor.Handles.DrawSolidDisc(transform.position, Vector3.forward, _data.range);
        }
#endif

    }
};

