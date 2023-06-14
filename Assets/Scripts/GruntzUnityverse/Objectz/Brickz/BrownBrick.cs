using System.Collections;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Brickz {
  public class BrownBrick : Brick {
    [field: SerializeField] public override AnimationClip BreakAnimation { get; set; }

    protected override void Start() {
      base.Start();

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
