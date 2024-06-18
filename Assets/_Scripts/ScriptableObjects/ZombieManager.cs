using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using Utils.Extensions;
using UnityEngine.AI;

public class ZombieManager : Utils.Singleton<ZombieManager>
{
    public ePoolType[] ZombieTypes;
    PoolManager poolManager => PoolManager.Instance;
    public float SpawnDelay;

    private void Start()
    {
        Observable.Interval(System.TimeSpan.FromSeconds(SpawnDelay)).Subscribe(_ => SpawnZombie()).AddTo(this);
    }
    public void SpawnZombie()
    {
        GameObject zombie = poolManager.Dequeue(ZombieTypes[Random.Range(0, ZombieTypes.Length)], transform.position, transform.rotation, transform);
        this.SkipFrame(() =>
        {
            zombie.GetComponent<NavMeshAgent>().nextPosition = zombie.transform.position;
            zombie.GetComponent<NavMeshAgent>().SetDestination(Vector3.zero);
        });
    }
}
