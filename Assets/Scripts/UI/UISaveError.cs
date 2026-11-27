using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CloonsTD {
    public class UISaveError : MonoBehaviour {
        [Header("UI References")]
        [SerializeField] private GameObject _errorPanel;
        [SerializeField] private TextMeshProUGUI _errorMessageText;
        [SerializeField] private Button _retryButton;
        [SerializeField] private Button _closeButton;

        private void Start() {
            _errorPanel.SetActive(false);
            
            _retryButton.onClick.AddListener(HandleRetryClicked);
            _closeButton.onClick.AddListener(HidePanel);
            
            if (SaveSystem.Instance != null) {
                SaveSystem.Instance.OnSaveFailed += ShowErrorPanel;
            }
        }

        private void OnDestroy() {
            if (SaveSystem.Instance != null) {
                SaveSystem.Instance.OnSaveFailed -= ShowErrorPanel;
            }
        }

        private void ShowErrorPanel(string message) {
            _errorMessageText.text = message;
            _errorPanel.SetActive(true);
        }

        private void HandleRetryClicked() {
            var success = SaveSystem.Instance.SaveGame();
        
            if (success) {
                HidePanel();
            }
        }

        private void HidePanel() {
            _errorPanel.SetActive(false);
        }
    }   
}