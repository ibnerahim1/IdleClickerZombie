using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using Utils.Config;
using UniRx;

public class UIManager : Utils.Singleton<UIManager>
{
    GameData gameData => SaveLoadManager.Instance.GameData;

    [SerializeField] GameObject menuPanel, gamePanel;
    [SerializeField] RectTransform settingsBG;
    [SerializeField] Image setting_image, sound_image, haptic_image;
    [SerializeField] Sprite settingsOn_sprite, settingsOff_sprite, soundOn_sprite, soundOff_sprite, hapticOn_sprite, hapticOff_sprite;
    [SerializeField] TextMeshProUGUI coins_textUI, income_textUI;

    [Header("Button Refs")]
    [SerializeField] Color inActiveColor;
    [SerializeField] ButtonRefs addGunButton, mergeGunButton, incomeButton, openPortalButton;

    private bool settingsOn = false;

    private void Start()
    {
        UpdateSettingUI();
        UpdateSoundUI();
        UpdateHapticUI();
        UpdateButtonsState();

        gameData.Coins
            .Subscribe(_ => UpdateButtonsState())
            .AddTo(this);
    }

    #region Settings
    public void ToggleSettigs()
    {
        settingsOn = !settingsOn;
        UpdateSettingUI();
    }
    public void ToggleSound()
    {
        gameData.Settings.SoundOn.Value = !gameData.Settings.SoundOn.Value;
        UpdateSoundUI();
    }
    public void ToggleHaptic()
    {
        gameData.Settings.HapticsOn.Value = !gameData.Settings.HapticsOn.Value;
        UpdateHapticUI();
    }

    public void UpdateSettingUI()
    {
        if (settingsOn)
        {
            setting_image.sprite = settingsOn_sprite;
            settingsBG.DOSizeDelta(new Vector2(120, 360), 0.2f).SetEase(Ease.InSine);
            sound_image.GetComponent<RectTransform>().DOSizeDelta(new Vector2(80, 80), 0.2f).SetEase(Ease.InSine);
            haptic_image.GetComponent<RectTransform>().DOSizeDelta(new Vector2(80, 80), 0.2f).SetEase(Ease.InSine);
            return;
        }
        setting_image.sprite = settingsOff_sprite;
        settingsBG.DOSizeDelta(new Vector2(120, 120), 0.2f).SetEase(Ease.InSine);
        sound_image.GetComponent<RectTransform>().DOSizeDelta(new Vector2(0, 0), 0.2f).SetEase(Ease.InSine);
        haptic_image.GetComponent<RectTransform>().DOSizeDelta(new Vector2(0, 0), 0.2f).SetEase(Ease.InSine);
    }
    public void UpdateSoundUI()
    {
        sound_image.sprite = gameData.Settings.SoundOn.Value ? soundOn_sprite : soundOff_sprite;
    }
    public void UpdateHapticUI()
    {
        haptic_image.sprite = gameData.Settings.HapticsOn.Value ? hapticOn_sprite : hapticOff_sprite;
    }
    #endregion

    #region Upgrades
    public void AddGunClicked()
    {
        gameData.Upgrades.AddGunLevel.Value++;
        gameData.Coins.Value -= Upgrades.AddGunCost[gameData.Upgrades.AddGunLevel.Value];
        Debug.Log("Add Gun Clicked");
    }
    public void MergeGunsClicked()
    {
        gameData.Upgrades.MergeGunLevel.Value++;
        gameData.Coins.Value -= Upgrades.MergeGunCost[gameData.Upgrades.MergeGunLevel.Value];
        Debug.Log("Merge Gun Clicked");
    }
    public void IncomeClicked()
    {
        gameData.Upgrades.IncomeLevel.Value++;
        gameData.Coins.Value -= Upgrades.IncomeCost[gameData.Upgrades.IncomeLevel.Value];
        Debug.Log("Income Clicked");
    }
    public void OpenPortalClicked()
    {
        gameData.Upgrades.OpenPortalLevel.Value++;
        gameData.Coins.Value -= Upgrades.OpenPortalCost[gameData.Upgrades.OpenPortalLevel.Value];
        Debug.Log("Open Portal Clicked");
    }
    #endregion


    private void UpdateButtonsState()
    {
        GameManager.Instance.ButtonStates[eButtonType.AddGun] = GetButtonState(Upgrades.AddGunCost, gameData.Upgrades.AddGunLevel.Value);
        GameManager.Instance.ButtonStates[eButtonType.MergeGun] = GetButtonState(Upgrades.MergeGunCost, gameData.Upgrades.MergeGunLevel.Value);
        GameManager.Instance.ButtonStates[eButtonType.Income] = GetButtonState(Upgrades.IncomeCost, gameData.Upgrades.IncomeLevel.Value);
        GameManager.Instance.ButtonStates[eButtonType.OpenPortal] = GetButtonState(Upgrades.OpenPortalCost, gameData.Upgrades.OpenPortalLevel.Value);

        UpdateButtonUI(eButtonType.AddGun);
        UpdateButtonUI(eButtonType.MergeGun);
        UpdateButtonUI(eButtonType.Income);
        UpdateButtonUI(eButtonType.OpenPortal);
    }

    public eButtonState GetButtonState(float[] cost, int level)
    {
        if (level == cost.Length - 1)
            return eButtonState.Max;
        // if (for ads check)
        //     return eButtonState.Ads;
        if (gameData.Coins.Value < cost[level + 1])
            return eButtonState.LowCoins;
        return eButtonState.On;
    }

    public void UpdateButtonUI(eButtonType type)
    {
        ButtonRefs button = new ButtonRefs();
        string costText = " ";
        switch (type)
        {
            case eButtonType.AddGun:
                button = addGunButton;
                button.title_textUI.text = $"Add Gun\n{gameData.Upgrades.AddGunLevel}/{Upgrades.AddGunCost.Length - 1} ";
                if (GameManager.Instance.ButtonStates[type] == eButtonState.Max)
                    break;
                costText = Upgrades.AddGunCost[gameData.Upgrades.AddGunLevel.Value + 1].ToString();
                break;
            case eButtonType.MergeGun:
                button = mergeGunButton;
                button.title_textUI.text = $"Merge Gun\n{gameData.Upgrades.MergeGunLevel}/{Upgrades.MergeGunCost.Length - 1} ";
                if (GameManager.Instance.ButtonStates[type] == eButtonState.Max)
                    break;
                costText = Upgrades.MergeGunCost[gameData.Upgrades.MergeGunLevel.Value + 1].ToString();
                break;
            case eButtonType.Income:
                button = incomeButton;
                button.title_textUI.text = $"Income\n{gameData.Upgrades.IncomeLevel}/{Upgrades.IncomeCost.Length - 1} ";
                if (GameManager.Instance.ButtonStates[type] == eButtonState.Max)
                    break;
                costText = Upgrades.IncomeCost[gameData.Upgrades.IncomeLevel.Value + 1].ToString();
                break;
            case eButtonType.OpenPortal:
                button = openPortalButton;
                button.title_textUI.text = $"Open Portal\n{gameData.Upgrades.OpenPortalLevel}/{Upgrades.OpenPortalCost.Length - 1} ";
                if (GameManager.Instance.ButtonStates[type] == eButtonState.Max)
                    break;
                costText = Upgrades.OpenPortalCost[gameData.Upgrades.OpenPortalLevel.Value + 1].ToString();
                break;
        }
        switch (GameManager.Instance.ButtonStates[type])
        {
            case eButtonState.LowCoins:
                button.cost_textUI.text = costText;
                button.image.GetComponent<Button>().interactable = false;
                button.image.DOColor(inActiveColor, 0.2f).SetEase(Ease.InSine);
                break;
            case eButtonState.On:
                button.cost_textUI.text = costText;
                button.image.GetComponent<Button>().interactable = true;
                button.image.DOColor(button.activeColor, 0.2f).SetEase(Ease.InSine);
                break;
            case eButtonState.Ads:
                button.cost_textUI.text = "FREE";
                button.image.GetComponent<Button>().interactable = true;
                button.image.DOColor(button.activeColor, 0.2f).SetEase(Ease.InSine);
                break;
            case eButtonState.Max:
                button.cost_textUI.text = "MAX";
                button.image.GetComponent<Button>().interactable = false;
                button.image.DOColor(inActiveColor, 0.2f).SetEase(Ease.InSine);
                break;
        }

    }
}
[System.Serializable]
public class ButtonRefs
{
    public TextMeshProUGUI title_textUI, cost_textUI;
    public Image image;
    public Color activeColor;
}