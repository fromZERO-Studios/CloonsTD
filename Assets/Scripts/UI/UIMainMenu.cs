using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

namespace CloonsTD {
    public class UIMainMenu : MonoBehaviour {
        [SerializeField] private AudioMixer _audioMixer;
        
        public void PlayGame() {
            SceneManager.LoadScene("Level1");
        }

        public void OpenOptions() {
            UIOptions.Open();
        }

        public void QuitGame() {
            Application.Quit();
        }
        
        // TODO: AudioMixer
        // TODO: Reset counts of scores, etc.
    }   
}
