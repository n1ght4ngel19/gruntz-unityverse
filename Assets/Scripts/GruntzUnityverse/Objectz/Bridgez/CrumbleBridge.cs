using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Bridgez {
  public class CrumbleBridge : MapObject {
    public bool isDeathBridge;
    private AnimationClip Anim { get; set; }


    protected override void Start() {
      base.Start();

      string optionalDeath = isDeathBridge ? "Death" : "";

      Anim = Resources.Load<AnimationClip>(
        $"Animationz/MapObjectz/Bridgez/{Area}/Clipz/Crumble{optionalDeath}Bridge_Down"
      );
    }

    public void Crumble() {
      Animancer.Play(Anim);

      LevelManager.Instance.SetBlockedAt(Location, false);
    }
  }
}
