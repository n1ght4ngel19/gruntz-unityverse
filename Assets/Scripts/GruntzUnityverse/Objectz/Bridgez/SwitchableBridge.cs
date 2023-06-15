using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Bridgez {
  public class SwitchableBridge : MapObject {
    [field: SerializeField] public bool IsDown { get; set; }
    private AnimationClip DownAnim { get; set; }
    private AnimationClip UpAnim { get; set; }

    public void ToggleBridge() {
      Animancer.Play(IsDown ? UpAnim : DownAnim);
      Animator.Play(IsDown ? "WaterBridge_Up" : "WaterBridge_Down");

      IsDown = !IsDown;
      LevelManager.Instance.SetInaccessibleAt(Location, IsDown);
      LevelManager.Instance.SetIsDrowningAt(Location, IsDown);
    }

    protected override void Start() {
      base.Start();


      // Todo: !!! Introduce tileset parameter/field/whatever where necessary !!!
      // Todo: Generalize
      DownAnim = Resources.Load<AnimationClip>($"Animationz/MapObjectz/Bridgez/RockyRoadz/Clipz/WaterBridge_Down");
      UpAnim = Resources.Load<AnimationClip>($"Animationz/MapObjectz/Bridgez/RockyRoadz/Clipz/WaterBridge_Up");
    }

    private void Update() {
      LevelManager.Instance.SetInaccessibleAt(Location, IsDown);
      LevelManager.Instance.SetIsDrowningAt(Location, IsDown);

      enabled = false;
    }
  }
}
