using System;
using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Bridgez {
  public class ToggleBridge : MapObject {
    public int interval;
    public bool isDown;
    public bool isDeathBridge;
    private AnimationClip DownAnim { get; set; }
    private AnimationClip UpAnim { get; set; }


    protected override void Start() {
      base.Start();

      string optionalDeath = isDeathBridge ? "Death" : "";

      DownAnim = Resources.Load<AnimationClip>(
        $"Animationz/MapObjectz/Bridgez/{Area}/Clipz/Toggle{optionalDeath}Bridge_Down"
      );

      UpAnim = Resources.Load<AnimationClip>(
        $"Animationz/MapObjectz/Bridgez/{Area}/Clipz/Toggle{optionalDeath}Bridge_Up"
      );
    }

    private void Update() {
      if (interval <= 0) {
        Debug.LogError("Interval has to be a positive number!");

        enabled = false;
      }
    }

    public void Toggle() {
      Animancer.Play(isDown ? UpAnim : DownAnim);

      isDown = !isDown;
      LevelManager.Instance.SetBlockedAt(Location, isDown);
      LevelManager.Instance.SetIsDrowningAt(Location, isDown);
    }
  }
}
