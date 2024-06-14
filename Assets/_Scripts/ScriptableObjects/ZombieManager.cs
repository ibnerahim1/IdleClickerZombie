using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using Utils.Extensions;
using UnityEngine.AI;

public class ZombieManager : Utils.Singleton<ZombieManager>
{
    [SerializeField] List<Zombie> Pool = new List<Zombie>();
    public float SpawnDelay;

    private void Start()
    {
        Observable.Interval(System.TimeSpan.FromSeconds(SpawnDelay)).Subscribe(_ => SpawnZombie()).AddTo(this);
    }
    public void SpawnZombie()
    {
        if (Pool.Count > 0)
        {
            Zombie zombie = Pool[0];
            Pool.Remove(zombie);
            zombie.Initialize(ZombieData.GetZombieData(Random.Range(1, 5)));
            zombie.transform.position = transform.position;
            zombie.transform.rotation = transform.rotation;
            zombie.GetComponent<NavMeshAgent>().SetDestination(Vector3.zero);
        }
    }
    public void Kill(Zombie zombie)
    {
        zombie.Off();
        Pool.Add(zombie);
    }
}
