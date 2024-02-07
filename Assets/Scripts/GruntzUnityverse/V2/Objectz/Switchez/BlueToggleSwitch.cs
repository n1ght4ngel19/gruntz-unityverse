using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.V2.Objectz.Bridgez;
using UnityEngine;

namespace GruntzUnityverse.V2.Objectz.Switchez {
  public class BlueToggleSwitch : SwitchV2 {
    /// <summary>
    /// The Bridgez that this BlueSwitch controls.
    /// </summary>
    public List<Bridge> bridgez;

    public override void Setup() {
      base.Setup();
      bridgez = transform.parent.GetComponentsInChildren<Bridge>().ToList();
    }

    protected override IEnumerator OnTriggerEnter2D(Collider2D other) {
      yield return base.OnTriggerEnter2D(other);

      bridgez.ForEach(bridge => bridge.Toggle());
    }

    protected override IEnumerator OnTriggerExit2D(Collider2D other) {
      yield return base.OnTriggerExit2D(other);
    }
  }
}
