using Cysharp.Threading.Tasks;
using GruntzUnityverse.V2.Grunt;
using GruntzUnityverse.V2.Objectz;
using UnityEngine;

namespace GruntzUnityverse.V2.Itemz.Toolz {
  [CreateAssetMenu(fileName = "New Equipped Tool", menuName = "Gruntz Unityverse/Equipped Tool")]
  public class EquippedTool : ScriptableObject {
    public string toolName;
    public string description;

    [Range(0, 20)]
    public int damage;

    [Range(0, 10)]
    public int range;

    [Range(0, 5)]
    public float moveSpeed;

    public AnimationPackV2 animationPack;

    // public float useTime;
    // public float attackTime;
    // public float rechargeTime;

    public async void Use(GruntV2 target) {
      Debug.Log($"Attacking {target.name}");

      await UniTask.WaitForSeconds(0.5f);
      // yield return new WaitForSeconds(0.5f);

      target.TakeDamage(damage);
    }

    public async void Use(GridObject target) {
      Debug.Log($"Interacting with {target.name}");

      IInteractable interactable = target as IInteractable;

      await UniTask.WaitForSeconds(0.5f);
      // yield return new WaitForSeconds(0.5f);

      // target.StartCoroutine(interactable.Interact());
      interactable?.Interact();
    }
  }
}
