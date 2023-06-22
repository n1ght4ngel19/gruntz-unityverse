using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Bridgez {
  public class Bridge : MapObject {
    public bool isDown;
    public bool isDeathBridge;
    private AnimationClip DownAnim { get; set; }
    private AnimationClip UpAnim { get; set; }

    public void Toggle() {
      Animancer.Play(isDown ? UpAnim : DownAnim);

      isDown = !isDown;
      LevelManager.Instance.SetBlockedAt(Location, isDown);
      LevelManager.Instance.SetIsWaterAt(Location, isDown);
    }

    protected override void Start() {
      base.Start();

      string optionalDeath = isDeathBridge ? "Death" : "";
      DownAnim = Resources.Load<AnimationClip>($"Animationz/MapObjectz/Bridgez/{Area}/Clipz/{optionalDeath}Bridge_Down");
      UpAnim = Resources.Load<AnimationClip>($"Animationz/MapObjectz/Bridgez/{Area}/Clipz/{optionalDeath}Bridge_Up");
    }

    private void Update() {
      LevelManager.Instance.SetBlockedAt(Location, isDown);
      LevelManager.Instance.SetIsWaterAt(Location, isDown);

      enabled = false;
    }
  }
}
