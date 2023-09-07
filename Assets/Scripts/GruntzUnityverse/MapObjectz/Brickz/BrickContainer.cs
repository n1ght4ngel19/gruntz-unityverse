using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.MapObjectz.Interactablez;
using UnityEngine;

namespace GruntzUnityverse.MapObjectz.Brickz {
  public class BrickContainer : MapObject, IBreakable {
    private List<Brick> Brickz { get; set; }
    public AnimationClip BreakAnimation { get; set; }

    protected override void Start() {
      base.Start();

      Brickz = GetComponentsInChildren<Brick>().ToList();
      GameManager.Instance.currentLevelManager.BrickContainerz.Add(this);
    }

    private void Update() {
      if (Brickz.Count != 0) {
        return;
      }

      GameManager.Instance.currentLevelManager.BrickContainerz.Remove(this);
      GameManager.Instance.currentLevelManager.SetBlockedAt(location, false);
      GameManager.Instance.currentLevelManager.SetHardTurnAt(location, false);
      Destroy(gameObject, 1f);
    }


    public IEnumerator Break(float contactDelay) {
      // 1.5s is the delay after the beginning of the GauntletzGrunt's Rock breaking animation (when the Brick actually should break)
      yield return new WaitForSeconds(1.5f);

      Brick brickToBreak = Brickz.OrderBy(brick => brick.transform.position.z).First();

      StartCoroutine(brickToBreak.Break(contactDelay));
      Brickz.Remove(brickToBreak);

      yield return null;
    }
  }
}
