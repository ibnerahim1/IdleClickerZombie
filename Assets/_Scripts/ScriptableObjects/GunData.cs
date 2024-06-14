using UnityEngine;

[CreateAssetMenu(fileName = "GunData", menuName = "ScriptableObjects/GunData", order = 2)]
public class GunData : ScriptableObject
{
    public float Damage;
    public float ShootingSpeed;
    public float Range;
    public Transform Model;

    const string path = "ScriptableObjects/GunData";

    public static GunData GetGunData(int level)
    {
        return Resources.Load<GunData>($"{path} {level}");
    }
}
