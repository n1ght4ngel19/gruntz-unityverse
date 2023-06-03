using UnityEngine;

namespace GruntzUnityverse.Objectz.MapItemz {
  public class MapItem : MapObject {
    [field: SerializeField] public AnimationClip RotationAnimation { get; set; }
  }
}
