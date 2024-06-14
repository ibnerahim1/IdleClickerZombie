using UnityEngine;
using UnityEngine.UIElements;
using UniRx;
using Utils.Extensions;

public class SoundManager : Utils.Singleton<SoundManager>
{
    GameData gameData => SaveLoadManager.Instance.GameData;
    [SerializeField] AudioSource MusicSource;

    private void Start()
    {
        gameData.Settings.SoundOn
            .Subscribe(val => MusicSource.volume = val.ToInt())
            .AddTo(this);
    }

    public void SpawnSFX(eSFXType type)
    {
        if (!gameData.Settings.SoundOn.Value)
            return;
        switch (type)
        {
            case eSFXType.Tap:

                break;
            case eSFXType.Stack:

                break;
            case eSFXType.Unstack:

                break;
            case eSFXType.Money:

                break;
            case eSFXType.Upgrade:

                break;
        }
    }
}