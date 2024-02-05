using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.V2.Objectz.Bridgez;
using UnityEngine;

namespace GruntzUnityverse.V2.Objectz.Switchez {
  public class BlueToggleSwitchV2 : SwitchV2 {
    /// <summary>
    /// The Bridgez that this BlueSwitch controls.
    /// </summary>
    public List<BridgeV2> bridgez;

    protected override void Setup() {
      base.Setup();

      bridgez = transform.parent.GetComponentsInChildren<BridgeV2>().ToList();
    }

    protected override IEnumerator OnTriggerEnter2D(Collider2D other) {
      ToggleOn();

      bridgez.ForEach(bridge => bridge.Toggle());

      yield break;
    }

    protected override IEnumerator OnTriggerExit2D(Collider2D other) {
      ToggleOff();

      yield break;
    }
  }
}
