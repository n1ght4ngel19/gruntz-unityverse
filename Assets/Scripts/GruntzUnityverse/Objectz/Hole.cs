using System.Collections;
using UnityEngine;

namespace GruntzUnityverse.Objectz {
  public class Hole : MapObject {
    [field: SerializeField] public Sprite OpenSprite { get; set; }
    [field: SerializeField] public Sprite FilledSprite { get; set; }
    [field: SerializeField] public bool IsOpen { get; set; }

    /// <summary>
    /// Toggles whether the Hole is open.
    /// </summary>
    /// <returns>An <see cref="IEnumerator"/> since this is a <see cref="Coroutine"/></returns>
    public IEnumerator Dig() {
      // Animator.Play("DirtFlying");
      yield return new WaitForSeconds(OwnAnimator.GetCurrentAnimatorStateInfo(0).length);

      IsOpen = !IsOpen;
      Renderer.sprite = IsOpen ? OpenSprite : FilledSprite;
    }
  }
}
