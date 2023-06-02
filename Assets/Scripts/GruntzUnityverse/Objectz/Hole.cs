using System.Collections;
using UnityEngine;

namespace GruntzUnityverse.Objectz {
  public class Hole : MapObject {
    [field: SerializeField] public Sprite OpenSprite { get; set; }
    [field: SerializeField] public Sprite FilledSprite { get; set; }
    [field: SerializeField] public bool IsOpen { get; set; }

    public IEnumerator Dig() {
      // Animator.Play("DirtFlying");
      yield return new WaitForSeconds(Animator.GetCurrentAnimatorStateInfo(0).length);

      IsOpen = !IsOpen;
      Renderer.sprite = IsOpen ? OpenSprite : FilledSprite;
    }
  }
}
