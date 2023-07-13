using System.Collections;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Brickz {
  public abstract class Brick : MapObject, IBreakable {
    public BrickType BrickType { get; set; }
    public abstract AnimationClip BreakAnimation { get; set; }

    protected override void Start() {
      base.Start();

      LevelManager.Instance.SetBlockedAt(location, true);
      LevelManager.Instance.SetHardTurnAt(location, true);
    }

    public abstract IEnumerator Break();
  }
}
