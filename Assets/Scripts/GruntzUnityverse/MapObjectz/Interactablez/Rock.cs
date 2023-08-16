using System.Collections;
using GruntzUnityverse.Managerz;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse.MapObjectz.Interactablez {
  public class Rock : MapObject, IBreakable {
    public AnimationClip BreakAnimation { get; set; }
    private Vector3 _brokenScale;
    private Quaternion _brokenRotation;

    protected override void Start() {
      base.Start();

      LevelManager.Instance.SetBlockedAt(location, true);
      LevelManager.Instance.SetHardTurnAt(location, true);
      Addressables.LoadAssetAsync<AnimationClip>($"RockBreak_{abbreviatedArea}_01.anim").Completed += (handle) => {
        BreakAnimation = handle.Result;
      };
      
      _brokenScale = new Vector3(0.7f, 0.7f, 0.7f);
      _brokenRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
    }

    public IEnumerator Break() {
      // 1.5s is the delay after the beginning of the GauntletzGrunt's Rock breaking animation (when the Rock actually should break)
      yield return new WaitForSeconds(1.5f);

      animancer.Play(BreakAnimation);

      transform.localScale = _brokenScale;
      transform.localRotation = _brokenRotation;

      LevelManager.Instance.Rockz.Remove(this);
      LevelManager.Instance.SetBlockedAt(location, false);
      LevelManager.Instance.SetHardTurnAt(location, false);

      //yield return new WaitForSeconds(1.5f);
      yield return new WaitForSeconds(BreakAnimation.length);

      spriteRenderer.sortingLayerName = "AlwaysBottom";
      spriteRenderer.sortingOrder = 15;
      enabled = false;
    }
  }
}
