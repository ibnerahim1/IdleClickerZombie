using MoreMountains.NiceVibrations;
using UniRx;

public class HapticManager : Utils.Singleton<HapticManager>
{
    GameData gameData => SaveLoadManager.Instance.GameData;
    public void Play(HapticTypes type)
    {
        if (!gameData.Settings.HapticsOn.Value)
            return;
        switch (type)
        {
            case HapticTypes.Selection:
                MMVibrationManager.Haptic(HapticTypes.Selection);
                break;
            case HapticTypes.Success:
                MMVibrationManager.Haptic(HapticTypes.Success);
                break;
            case HapticTypes.Warning:
                MMVibrationManager.Haptic(HapticTypes.Warning);
                break;
            case HapticTypes.Failure:
                MMVibrationManager.Haptic(HapticTypes.Failure);
                break;
            case HapticTypes.LightImpact:
                MMVibrationManager.Haptic(HapticTypes.LightImpact);
                break;
            case HapticTypes.MediumImpact:
                MMVibrationManager.Haptic(HapticTypes.MediumImpact);
                break;
            case HapticTypes.HeavyImpact:
                MMVibrationManager.Haptic(HapticTypes.HeavyImpact);
                break;
            case HapticTypes.RigidImpact:
                MMVibrationManager.Haptic(HapticTypes.RigidImpact);
                break;
            case HapticTypes.SoftImpact:
                MMVibrationManager.Haptic(HapticTypes.SoftImpact);
                break;
        }
    }
}