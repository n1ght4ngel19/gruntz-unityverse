using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Bridgez {
  public class Bridge : MapObject {
    public bool isDown;
    public bool isDeathBridge;
    private AnimationClip _downAnim;
    private AnimationClip _upAnim;


    protected override void Start() {
      base.Start();

      AssignAreaBySpriteName();

      string optionalDeath = isDeathBridge ? "Death" : "";

      _downAnim = Resources.Load<AnimationClip>(
        $"Animationz/MapObjectz/Bridgez/{area}/Clipz/{optionalDeath}Bridge_Down"
      );

      _upAnim = Resources.Load<AnimationClip>($"Animationz/MapObjectz/Bridgez/{area}/Clipz/{optionalDeath}Bridge_Up");
    }

    private void Update() {
      LevelManager.Instance.SetBlockedAt(location, isDown);
      LevelManager.Instance.SetWaterAt(location, isDown);
      LevelManager.Instance.SetDeathAt(location, isDeathBridge);

      enabled = false;
    }

    public void Toggle() {
      Animancer.Play(isDown ? _upAnim : _downAnim);

      isDown = !isDown;
      LevelManager.Instance.SetBlockedAt(location, isDown);
      LevelManager.Instance.SetWaterAt(location, isDown);
    }
  }
}
