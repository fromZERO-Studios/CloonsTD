using System;
using System.IO;
using UnityEngine;

public class SaveSystem : MonoBehaviour {
    public static SaveSystem Instance { get; private set; }

    private int _round;
    private int _level;
    private string _saveFilePath;

    public int CurrentRound => _round;
    public int CurrentLevel => _level;

    public Action<int> OnLevelStart = delegate { };
    public Action<int> OnRoundStarted = delegate { };
    public Action<int> OnRoundFinished = delegate { };
    
    [Serializable]
    private struct SaveData {
        public int round;
        public int level;
    }
    
    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _saveFilePath = Path.Combine(Application.persistentDataPath, "savedata.json");

        LoadGame();

        OnLevelStart += LevelChanged;
        OnRoundFinished += RoundFinished;
    }

    private void OnDestroy() {
        if (Instance != this) return;
        OnLevelStart -= LevelChanged;
        OnRoundFinished -= RoundFinished;
    }

    private void LevelChanged(int newLevel) {
        if (_level > newLevel) return;
        _level = newLevel;
        SaveGame();
    }

    private void RoundFinished(int newRound) {
        if (_round > newRound) return;
        _round = newRound;
        SaveGame();
    }

    public void SaveGame() {
        var data = new SaveData {
            round = _round,
            level = _level
        };
        var json = JsonUtility.ToJson(data, true);
        File.WriteAllText(_saveFilePath, json);
    }

    public void LoadGame() {
        if (!File.Exists(_saveFilePath)) {
            _round = 1;
            _level = 1;
            return;
        }

        var json = File.ReadAllText(_saveFilePath);
        var data = JsonUtility.FromJson<SaveData>(json);

        _round = data.round;
        _level = data.level;
    }
}