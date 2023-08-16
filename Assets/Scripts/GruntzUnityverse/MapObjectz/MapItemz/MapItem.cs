using UnityEngine;

namespace GruntzUnityverse.MapObjectz.MapItemz {
  public class MapItem : MapObject {
    [field: SerializeField] public AnimationClip RotationAnimation { get; set; }

    protected override void Start() {
      base.Start();

      animancer.Play(RotationAnimation);
    }
  }
}
