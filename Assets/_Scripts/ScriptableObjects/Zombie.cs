using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Utils.Extensions;
using Utils.Interfaces;
using DG.Tweening;
using UnityEngine.AI;
using TMPro;

public class Zombie : MonoBehaviour, IPoolable
{
    public int Index;
    public float MaxHealth;
    public float Health;
    public int Coins;
    public float Speed;
    [SerializeField] Animator animator;
    [SerializeField] ePoolType poolType;
    float scale;
    Color color;

    private void Awake()
    {
        scale = transform.localScale.x;
        color = transform.FindChildByName<SkinnedMeshRenderer>("Body").material.color;
    }
    public void OnObjectDespawn()
    {
        GetComponent<NavMeshAgent>().enabled = true;
    }

    public void OnObjectSpawn()
    {
        transform.FindChildByName<SkinnedMeshRenderer>("Body").material.color = color;
        transform.localScale = Vector3.one * scale;
        animator.Play("Run");
        Health = MaxHealth;

        this.UpdateAsObservable()
            .Where(_ => Input.GetMouseButtonDown(0))
            .TakeUntil(this.ObserveEveryValueChanged(x => x.Health).Where(h => h <= 0))
            .Subscribe(_ =>
            {
                Health -= 0.2f;
                Debug.Log("Health: " + Health);
                if (Health <= 0)
                    ZombieDead();
            })
            .AddTo(this);
    }

    void ZombieDead()
    {
        transform.FindChildByName<SkinnedMeshRenderer>("Body").material.color = Color.grey;
        animator.SetTrigger("Die");
        SaveLoadManager.Instance.GameData.Coins.Value += Coins;
        transform.DOScale(Vector3.zero, 0.5f).SetDelay(3).SetEase(Ease.InSine).OnComplete(() => PoolManager.Instance.Enqueue(poolType, gameObject));
        GetComponent<NavMeshAgent>().enabled = false;
    }
}
