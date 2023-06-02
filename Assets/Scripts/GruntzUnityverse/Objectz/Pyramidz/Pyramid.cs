using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Pyramidz {
  public class Pyramid : MapObject {
    [field: SerializeField] public bool IsDown { get; set; }
    private AnimationClip DownAnim { get; set; }
    private AnimationClip UpAnim { get; set; }


    protected override void Start() {
      base.Start();

      LevelManager.Instance.SetBlockedAt(OwnLocation, !IsDown);

      DownAnim = Resources.Load<AnimationClip>($"Animationz/MapObjectz/Pyramidz/Clipz/{GetType().Name}_Down");
      UpAnim = Resources.Load<AnimationClip>($"Animationz/MapObjectz/Pyramidz/Clipz/{GetType().Name}_Up");
    }

    public void TogglePyramid() {
      Animancer.Play(IsDown ? UpAnim : DownAnim);

      IsDown = !IsDown;
      LevelManager.Instance.SetBlockedAt(OwnLocation, !IsDown);
    }
  }
}
