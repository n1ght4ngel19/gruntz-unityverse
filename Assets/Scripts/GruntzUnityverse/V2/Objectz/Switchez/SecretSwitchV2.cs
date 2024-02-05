using System.Collections.Generic;
using GruntzUnityverse.V2.Core;

namespace GruntzUnityverse.V2.Objectz.Switchez {
  public class SecretSwitchV2 : SwitchV2 {
    public List<SecretObjectV2> secretObjectz;

    public override void ToggleOn() {
      base.ToggleOn();

      DisableTrigger();

      GM.Instance.levelStatz.secretz++;
      secretObjectz.ForEach(so => StartCoroutine(so.ToggleOn()));
    }
  }
}
