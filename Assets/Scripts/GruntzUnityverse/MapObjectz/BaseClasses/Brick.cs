using System.Collections;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.MapObjectz.Interactablez;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse.MapObjectz.BaseClasses {
  public class Brick : MapObject, IBreakable {
    public AnimationClip BreakAnimation { get; set; }
    public BrickType brickType;

    protected override void Start() {
      base.Start();

      brickType = spriteRenderer.sprite.name.Contains(BrickType.Brown.ToString())
        ? BrickType.Brown
        : spriteRenderer.sprite.name.Contains(BrickType.Yellow.ToString())
          ? BrickType.Yellow
          : spriteRenderer.sprite.name.Contains(BrickType.Red.ToString())
            ? BrickType.Red
            : spriteRenderer.sprite.name.Contains(BrickType.Black.ToString())
              ? BrickType.Black
              : spriteRenderer.sprite.name.Contains(BrickType.Blue.ToString())
                ? BrickType.Blue
                : BrickType.None;

      GameManager.Instance.currentLevelManager.SetBlockedAt(location, true);
      GameManager.Instance.currentLevelManager.SetHardTurnAt(location, true);
    }

    public virtual IEnumerator Break(float contactDelay) {
      yield return null;
    }

    protected override void LoadAnimationz() {
      Addressables.LoadAssetAsync<AnimationClip>($"Effect_Shared_BrickBreak_{GetType()}.anim").Completed +=
        handle => {
          BreakAnimation = handle.Result;
        };
    }
  }
}
