using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CloonsTD {

    [RequireComponent(typeof(CircleCollider2D))]
    public class MonkeyHandler : MonoBehaviour {

        private const string TARGET_TAG = "Cloon";

        private readonly List<Transform> inRange = new();
        private CircleCollider2D _bloonCollider;

        [SerializeField]
        private MonkeyData _data;

        private Transform _target;

        private void LookAt2D(Transform target) {
            // print("LookAt2D: " + target.name);
            // transform.LookAt only works in 3D
            var direction = target.position - transform.position;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // get the angle and convert it from radians to degrees
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        IEnumerator Shoot() {
            while (true) {
                if (_target != null) {
                    var projectile = PoolManager.Spawn(_data.projectilePrefab, transform.position, transform.rotation);
                    projectile.Init(_data.damage, _data.pierce, _data.projectileSpeed);
                }
                yield return new WaitForSeconds(1f / _data.fireRate);
            }
        }

        private void Awake() {
            _bloonCollider = GetComponent<CircleCollider2D>();
            _bloonCollider.radius = _data.range;
        }

        private void Start() {
            StartCoroutine(Shoot());
        }

        private void Update() {
            if (_target == null) {
                inRange.RemoveAll(t => t == null); // remove all dead entries
                _target = inRange.Count > 0 ? inRange[0] : null; // try to get a new target if we don't have one
            }

            if (_target == null) return;

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

