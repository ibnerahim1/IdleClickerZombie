using UnityEngine;

public class Gun : MonoBehaviour
{
    public GunData gunData;

    public void Initialize(GunData data)
    {
        gunData = data;
    }

    public void Shoot()
    {
        // Implement shooting logic based on damage and shootingSpeed
    }

    void Update()
    {
        // Handle shooting logic
    }
}
