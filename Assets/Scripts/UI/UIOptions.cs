using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CloonsTD {
    public class UIOptions : MonoBehaviour {
        public const string SCENE_NAME = "UIOptions";
        private const string MAIN_MENU_SCENE = "MainMenu";
        
        [Header("UI References")]
        [SerializeField] private Slider _masterSlider;
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _sfxSlider;
        [SerializeField] private Toggle _fullScreenToggle;
        
        public static bool IsOpen => SceneManager.GetSceneByName(SCENE_NAME).isLoaded;

        public static void Open() {
            if (!IsOpen) {
                SceneManager.LoadScene(SCENE_NAME, LoadSceneMode.Additive);
            }
        }

        public static void Close() {
            if (IsOpen) {
                SceneManager.UnloadSceneAsync(SCENE_NAME);
            }
        }

        private void Awake() {
            //Assert.IsNotNull(_audioMixer, "AudioMixer is not assigned in the UIOptions.");
            Assert.IsNotNull(_masterSlider, "MasterSlider is not assigned in the UIOptions.");
            Assert.IsNotNull(_musicSlider, "MusicSlider is not assigned in the UIOptions.");
            Assert.IsNotNull(_sfxSlider, "SfxSlider is not assigned in the UIOptions.");
            Assert.IsNotNull(_fullScreenToggle, "FullScreenToggle is not assigned in the UIOptions.");
        }

        private void Start() {
            _masterSlider.SetValueWithoutNotify(GameSettings.MasterVolume);
            _musicSlider.SetValueWithoutNotify(GameSettings.MusicVolume);
            _sfxSlider.SetValueWithoutNotify(GameSettings.SfxVolume);
            _fullScreenToggle.SetIsOnWithoutNotify(GameSettings.FullScreen);
        }

        private void OnDestroy() {
            GameSettings.Save();
        }

        public void SetMasterVolume(float volume) {
            GameSettings.MasterVolume = volume;
            //GameSettings.Apply(_audioMixer);
        }

        public void SetMusicVolume(float volume) {
            GameSettings.MusicVolume = volume;
            //GameSettings.Apply(_audioMixer);
        }

        public void SetSfxVolume(float volume) {
            GameSettings.SfxVolume = volume;
            //GameSettings.Apply(_audioMixer);
        }

        public void SetFullScreen(bool isFullScreen) {
            GameSettings.FullScreen = isFullScreen;
            //GameSettings.Apply(_audioMixer);
        }

        public void ExitMenu() {
            Time.timeScale = 1f;
            SceneManager.LoadScene(MAIN_MENU_SCENE);
        }
    }    
}

