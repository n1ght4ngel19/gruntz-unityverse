using System.Collections;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.Managerz;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse.MapObjectz.Brickz {
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

      LevelManager.Instance.SetBlockedAt(location, true);
      LevelManager.Instance.SetHardTurnAt(location, true);
    }

    public virtual IEnumerator Break() {
      yield return null;
    }

    protected override void LoadAnimationz() {
      Addressables.LoadAssetAsync<AnimationClip>($"Effect_Shared_BrickBreak_{GetType()}.anim").Completed +=
        (handle) => {
          BreakAnimation = handle.Result;
        };
    }
  }
}
