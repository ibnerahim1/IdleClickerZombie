using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Utils.Extensions;

public class Zombie : MonoBehaviour
{
    public ZombieData zombieData;
    AnimationInstancing.AnimationInstancing animInstancing;

    public void Initialize(ZombieData data)
    {
        zombieData = data;
        GetComponentInChildren<SkinnedMeshRenderer>().material.color = data.color;
        animInstancing = GetComponent<AnimationInstancing.AnimationInstancing>();
        // animInstancing.InitializeAnimation();
        this.On();
        animInstancing.PlayAnimation("1 Run");
    }

    void Start()
    {
        this.OnDestroyAsObservable()
            .Subscribe(_ =>
            {
                SaveLoadManager.Instance.GameData.Coins.Value += zombieData.Coins;
                PlayDeathEffect();
            });
        this.UpdateAsObservable()
        .Where(_ => Input.GetMouseButtonDown(0))
        .Subscribe(_ => PlayDeathEffect())
        .AddTo(this);
    }

    void PlayDeathEffect()
    {
        this.Off();
        GetComponentInChildren<SkinnedMeshRenderer>().material.color = Color.grey;
        this.On();
        animInstancing.PlayAnimation("2 Die");
        // Add visual and audio effects for zombie death
        this.DelayedAction(() => ZombieManager.Instance.Kill(this), 2);
    }
}
