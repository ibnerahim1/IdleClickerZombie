using UnityEngine;
using UniRx;
using System.Collections.Generic;

public class GameManager : Utils.Singleton<GameManager>
{
    public Dictionary<eButtonType, eButtonState> ButtonStates = new Dictionary<eButtonType, eButtonState>() { { eButtonType.AddGun, eButtonState.On }, { eButtonType.MergeGun, eButtonState.On }, { eButtonType.Income, eButtonState.On }, { eButtonType.OpenPortal, eButtonState.On } };
    public List<Gun> guns = new List<Gun>();

    protected override void Awake()
    {
        base.Awake();
        SaveLoadManager.Instance.LoadData();
    }
}