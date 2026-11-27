using System;
using System.IO;
using UnityEngine;

public class SaveSystem : MonoBehaviour {
    public static SaveSystem Instance { get; private set; }

    [Header("Reference to Scriptable Object")] [SerializeField]
    private SaveData _saveData;
    
    private string _saveFilePath;
    
    public event Action<string> OnSaveFailed;

    public int CurrentRound => _saveData.currentData.round;
    public int CurrentLevel => _saveData.currentData.level;
    
    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _saveFilePath = Path.Combine(Application.persistentDataPath, "savedata.json");

        //LoadGame();

        _saveData.OnLevelStart += HandleLevelChanged;
        _saveData.OnRoundFinished += HandleRoundFinished;
    }

    private void OnDestroy() {
        if (Instance != this) return;
        _saveData.OnLevelStart -= HandleLevelChanged;
        _saveData.OnRoundFinished -= HandleRoundFinished;
    }

    private void HandleLevelChanged(int newLevel) {
        _saveData.UpdateLevel(newLevel);
        SaveGame();
    }

    private void HandleRoundFinished(int newRound) {
        _saveData.UpdateRound(newRound);
        SaveGame();
    }

    public bool SaveGame() {
        try {
            string json = JsonUtility.ToJson(_saveData.currentData, true);
            File.WriteAllText(_saveFilePath, json);
            return true;
        }
        catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException) {
            Debug.LogError($"[SaveSystem] Error saving the game in {_saveFilePath}: {ex.Message}");
            OnSaveFailed?.Invoke("Progress couldn't be saved. Check the space or permissions on your disc.");
            return false;
        }
    }

    public void LoadGame() {
        if (!File.Exists(_saveFilePath)) {
            _saveData.ResetData();
            return;
        }

        try {
            var json = File.ReadAllText(_saveFilePath);
            _saveData.currentData = JsonUtility.FromJson<SaveData.GameSaveStruct>(json);
        }
        catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException) {
            Debug.LogError($"[SaveSystem] Error reading save file: {ex.Message}");
            _saveData.ResetData();
        }
    }
}
