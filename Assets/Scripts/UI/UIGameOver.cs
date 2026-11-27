using System;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace CloonsTD{
    public class UIGameOver : MonoBehaviour {
        private const string MainMenuScene = "MainMenu";
        private const string VictoryScene = "VictoryScene";
        private const int MaxLevels = 3;

        [Header("References")] [SerializeField]
        private SaveData _saveData;
        [SerializeField] private TextMeshProUGUI _roundCounter;
        [SerializeField] private GameObject _nextLevelButton;
        [SerializeField] private GameObject _retryButton;

        private void Awake() {
            Assert.IsNotNull(_roundCounter, "_roundCounter is not assigned in the UIGameOver");
            Assert.IsNotNull(_saveData, "_saveData is not assigned in the UIGameOver");
            Assert.IsNotNull(_nextLevelButton, "_nextLevelButton is not assigned in the UIGameOver");
            Assert.IsNotNull(_retryButton, "_retryButton is not assigned in the UIGameOver");
        }

        private void Start() {
            var level = _saveData.currentData.level;
            var round = _saveData.currentData.round;
            var isVictory = _saveData.currentData.isVictory;
            
            

            _roundCounter.text = $"Level: {level}, Round: {round}";
            
            _nextLevelButton.SetActive(isVictory);
            _retryButton.SetActive(!isVictory);
        }
        
        public void ReturnToMainMenu() {
            SceneManager.LoadScene(MainMenuScene);
        }

        public void GoNextLevel() {
            var nextLevel = _saveData.currentData.level + 1;

            if (nextLevel > MaxLevels) {
                SceneManager.LoadScene(VictoryScene);
                return;
            }

            _saveData.currentData.round = 1;
            _saveData.currentData.isVictory = false;
            _saveData.OnLevelStart?.Invoke(nextLevel);

            SceneManager.LoadScene($"Level{nextLevel}");
        }

        public void RetryLevel() {
            var currentLevel = _saveData.currentData.level;
            
            _saveData.currentData.round = 1;
            _saveData.OnLevelStart?.Invoke(currentLevel);
            SceneManager.LoadScene($"Level{currentLevel}");
        }
        
        public void QuitGame() {
            Application.Quit();
        }
    }
}
