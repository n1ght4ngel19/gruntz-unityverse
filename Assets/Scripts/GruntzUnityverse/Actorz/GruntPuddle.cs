using System.Collections;
using GruntzUnityverse.MapObjectz.BaseClasses;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse.Actorz {
  public class GruntPuddle : MapObject {
    public AnimationClip appearAnim;
    public AnimationClip disappearAnim;
    public AnimationClip idleAnim;

    private void OnEnable() {
      animancer.Play(appearAnim);
    }

    protected override void Start() {
      animancer.Play(idleAnim);
    }

    public void SetMaterial(string materialKey) {
      Addressables.LoadAssetAsync<Material>($"{materialKey}.mat").Completed += handle2 => {
        spriteRenderer.material = handle2.Result;
      };
    }

    public override IEnumerator BeUsed() {
      animancer.Play(disappearAnim);

      yield return new WaitForSeconds(1f);

      GameManager.Instance.currentLevelManager.puddleCounter++;

      Destroy(gameObject);
    }
  }
}
