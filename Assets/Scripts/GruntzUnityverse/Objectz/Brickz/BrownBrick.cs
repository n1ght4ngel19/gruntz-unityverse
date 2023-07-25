using System.Collections;
using GruntzUnityverse.Enumz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Brickz {
  public class BrownBrick : Brick {
    [field: SerializeField] public override AnimationClip BreakAnimation { get; set; }

    protected override void Start() {
      base.Start();

      BrickType = BrickType.Brown;

      BreakAnimation =
        Resources.Load<AnimationClip>($"Animationz/Effectz/Shared/Clipz/Effect_Shared_BrickBreak_{BrickType}");
    }

    public override IEnumerator Break() {
      Animancer.Play(BreakAnimation);

      Destroy(gameObject, 1f);

      yield return null;
    }
  }
}
