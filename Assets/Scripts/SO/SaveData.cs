using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SaveData", menuName = "Scriptable Objects/SaveData")]
public class SaveData : ScriptableObject
{
    [Serializable]
    public struct GameSaveStruct {
        public int round;
        public int level;
        public bool isVictory;
    }

    public GameSaveStruct currentData;
    
    public Action<int> OnLevelStart = delegate { };
    public Action<int> OnRoundStarted = delegate { };
    public Action<int> OnRoundFinished = delegate { };

    private void OnEnable() {
        ResetData();
    }

    public void ResetData() {
        currentData = new GameSaveStruct {
            round = 1,
            level = 1,
            isVictory = false
        };
    }
    
    public void UpdateLevel(int newLevel) {
        if (currentData.level >= newLevel) return;
        currentData.level = newLevel;
    }

    public void UpdateRound(int newRound) {
        if(currentData.round >= newRound) return;
        currentData.round = newRound;
    }
}
