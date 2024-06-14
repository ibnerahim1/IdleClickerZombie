using UnityEngine;

[CreateAssetMenu(fileName = "ZombieData", menuName = "ScriptableObjects/ZombieData", order = 1)]
public class ZombieData : ScriptableObject
{
    public int Index;
    public int Health;
    public int Coins;
    public float Speed;
    public Color color;

    private const string path = "ScriptableObjects/ZombieData";
    public static ZombieData GetZombieData(int level)
    {
        return Resources.Load<ZombieData>($"{path} {level}");
    }
}
