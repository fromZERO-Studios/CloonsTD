using System;
using System.Collections.Generic;
using UnityEngine;

namespace CloonsTD {

    [RequireComponent(typeof(CircleCollider2D))]
    public class MonkeyHandler : MonoBehaviour {
        private static WaitForSeconds _waitForSeconds0_1 = new WaitForSeconds(0.1f);

        private const string TARGET_TAG = "Cloon";

        private readonly List<Transform> inRange = new();
        private CircleCollider2D _bloonCollider;

        private Transform _target;

        private void LookAt2D(Transform target) {
            print("LookAt2D: " + target.name);
            // transform.LookAt only works in 3D
            var direction = target.position - transform.position;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // get the angle and convert it from radians to degrees
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        private void Awake() {
            _bloonCollider = GetComponent<CircleCollider2D>();
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

    }
};

