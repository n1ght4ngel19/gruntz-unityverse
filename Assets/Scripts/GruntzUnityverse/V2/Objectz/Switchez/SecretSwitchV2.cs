using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.V2.Core;

namespace GruntzUnityverse.V2.Objectz.Switchez {
  public class SecretSwitchV2 : SwitchV2 {
    public List<SecretObjectV2> secretObjectz;

    public override void Setup() {
      secretObjectz = transform.parent.GetComponentsInChildren<SecretObjectV2>(true).ToList();
    }

    public override void ToggleOn() {
      base.ToggleOn();

      DisableTrigger();

      GM.Instance.levelStatz.secretz++;
      secretObjectz.ForEach(so => StartCoroutine(so.ToggleOn()));
    }
  }
}
