using UnityEngine;
using System.IO;
using UniRx;
using System;

public delegate void DataUpdated();
public class SaveLoadManager : Utils.Singleton<SaveLoadManager>
{
    private string saveFilePath;

    public event DataUpdated OnDataUpdated;

    protected override void Awake()
    {
        base.Awake();
        saveFilePath = Path.Combine(Application.persistentDataPath, $"GameData.json");
        LoadData();
        GameData.Coins.Subscribe(_ => SaveData()).AddTo(this);
        GameData.Settings.SoundOn.Subscribe(_ => SaveData()).AddTo(this);
        GameData.Settings.HapticsOn.Subscribe(_ => SaveData()).AddTo(this);
    }

    public GameData GameData = new GameData();

    // Save the game data to a file
    public void SaveData()
    {
        try
        {
            SerializableGameData serializableData = new SerializableGameData(GameData);
            string json = JsonUtility.ToJson(serializableData);
            File.WriteAllText(saveFilePath, json);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save data: {e.Message}");
        }
    }

    // Load the game data from a file
    public void LoadData()
    {
        if (File.Exists(saveFilePath))
        {
            try
            {
                string json = File.ReadAllText(saveFilePath);
                SerializableGameData serializableData = JsonUtility.FromJson<SerializableGameData>(json);
                GameData = new GameData(serializableData);

                OnDataUpdated?.Invoke();
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load data: {e.Message}");
                GameData = new GameData();
            }
        }
        else
        {
            GameData = new GameData();
            Debug.LogWarning("Save file not found. Creating a new one with default values.");
        }
    }
}

#region MasterDataClass
[Serializable]
public class GameData
{
    public ReactiveProperty<float> Coins = new ReactiveProperty<float>(10310);
    public GameDataSettings Settings = new GameDataSettings();
    public GameDataUpgrades Upgrades = new GameDataUpgrades();

    public GameData() { }

    public GameData(SerializableGameData serializableData)
    {
        Coins.Value = serializableData.Coins;
        Settings = new GameDataSettings(serializableData.Settings);
        Upgrades = new GameDataUpgrades(serializableData.Upgrades);
    }
}
#endregion

[Serializable]
public class GameDataSettings
{
    public ReactiveProperty<bool> SoundOn = new ReactiveProperty<bool>(true);
    public ReactiveProperty<bool> HapticsOn = new ReactiveProperty<bool>(true);

    public GameDataSettings() { }

    public GameDataSettings(SerializableGameDataSettings settings)
    {
        SoundOn.Value = settings.SoundOn;
        HapticsOn.Value = settings.HapticsOn;
    }
}

[Serializable]
public class GameDataUpgrades
{
    public ReactiveProperty<int> AddGunLevel = new ReactiveProperty<int>(0);
    public ReactiveProperty<int> MergeGunLevel = new ReactiveProperty<int>(0);
    public ReactiveProperty<int> IncomeLevel = new ReactiveProperty<int>(0);
    public ReactiveProperty<int> OpenPortalLevel = new ReactiveProperty<int>(0);

    public GameDataUpgrades() { }

    public GameDataUpgrades(SerializableGameDataUpgrades upgrades)
    {
        AddGunLevel.Value = upgrades.AddGunLevel;
        MergeGunLevel.Value = upgrades.MergeGunLevel;
        IncomeLevel.Value = upgrades.IncomeLevel;
        OpenPortalLevel.Value = upgrades.OpenPortalLevel;
    }
}

// Serializable classes for JSON serialization
[Serializable]
public class SerializableGameData
{
    public float Coins;
    public SerializableGameDataSettings Settings;
    public SerializableGameDataUpgrades Upgrades;

    public SerializableGameData(GameData gameData)
    {
        Coins = gameData.Coins.Value;
        Settings = new SerializableGameDataSettings(gameData.Settings);
        Upgrades = new SerializableGameDataUpgrades(gameData.Upgrades);
    }
}

[Serializable]
public class SerializableGameDataSettings
{
    public bool SoundOn;
    public bool HapticsOn;

    public SerializableGameDataSettings(GameDataSettings settings)
    {
        SoundOn = settings.SoundOn.Value;
        HapticsOn = settings.HapticsOn.Value;
    }
}

[Serializable]
public class SerializableGameDataUpgrades
{
    public int AddGunLevel;
    public int MergeGunLevel;
    public int IncomeLevel;
    public int OpenPortalLevel;

    public SerializableGameDataUpgrades(GameDataUpgrades upgrades)
    {
        AddGunLevel = upgrades.AddGunLevel.Value;
        MergeGunLevel = upgrades.MergeGunLevel.Value;
        IncomeLevel = upgrades.IncomeLevel.Value;
        OpenPortalLevel = upgrades.OpenPortalLevel.Value;
    }
}