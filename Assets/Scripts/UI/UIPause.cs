using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace CloonsTD {
    public class UIPause : MonoBehaviour {
        private bool _isPaused;
        private InputActions _inputActions;
        private InputAction _pauseAction;

        private void Awake() {
            _inputActions = new InputActions();
            _pauseAction = _inputActions.UI.Pause;
        }

        private void OnEnable() {
            _pauseAction.Enable();
            _pauseAction.performed += OnPause;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void OnDisable() {
            _pauseAction.performed -= OnPause;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        private void OnPause(InputAction.CallbackContext context) {
            print("aaaa");
            if (_isPaused) {
                UIOptions.Close();
            } else {
                Pause();
            }
        }
        
        private void OnSceneUnloaded(Scene scene) {
            if (scene.name == UIOptions.SCENE_NAME) {
                Resume();
            }
        }
        
        private void Resume() {
            Time.timeScale = 1f;
            _isPaused = false;
        }
        
        private void Pause() {
            UIOptions.Open();
            Time.timeScale = 0f;
            _isPaused = true;
        }
    }   
}
