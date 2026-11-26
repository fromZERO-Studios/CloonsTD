using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

namespace CloonsTD {
    public class UIMainMenu : MonoBehaviour {
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private SaveData _saveData;
        
        public void PlayGame() {
            // Reiniciamos los datos por si venimos de una partida anterior
            _saveData.ResetData();
            
            // Lanzamos el evento directamente desde el ScriptableObject
            _saveData.OnLevelStart?.Invoke(1);
            
            SceneManager.LoadScene("Level1");
        }

        public void OpenOptions() {
            UIOptions.Open();
        }

        public void QuitGame() {
            Application.Quit();
        }
        
        // TODO: AudioMixer
    }   
}