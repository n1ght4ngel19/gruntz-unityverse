﻿using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Bridgez {
  public class ToggleBridge : MapObject {
    public int interval;
    public bool isDown;
    public bool isDeathBridge;
    private AnimationClip _downAnim;
    private AnimationClip _upAnim;


    protected override void Start() {
      base.Start();
      
      AssignAreaBySpriteName();

      string optionalDeath = isDeathBridge ? "Death" : "";

      _downAnim = Resources.Load<AnimationClip>(
        $"Animationz/MapObjectz/Bridgez/{area}/Clipz/Toggle{optionalDeath}Bridge_Down"
      );

      _upAnim = Resources.Load<AnimationClip>(
        $"Animationz/MapObjectz/Bridgez/{area}/Clipz/Toggle{optionalDeath}Bridge_Up"
      );
    }

    private void Update() {
      if (interval <= 0) {
        Debug.LogError("Interval has to be a positive number!");

        enabled = false;
      }
    }

    public void Toggle() {
      animancer.Play(isDown ? _upAnim : _downAnim);

      isDown = !isDown;
      LevelManager.Instance.SetBlockedAt(location, isDown);
      LevelManager.Instance.SetWaterAt(location, isDown);
    }
  }
}
