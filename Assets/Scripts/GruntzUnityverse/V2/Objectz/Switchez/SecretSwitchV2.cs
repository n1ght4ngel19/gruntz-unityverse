using System.Collections.Generic;

namespace GruntzUnityverse.V2.Objectz.Switchez {
  public class SecretSwitchV2 : SwitchV2 {
    public List<SecretObjectV2> secretObjectz;

    public override void ToggleOn() {
      base.ToggleOn();

      DisableTrigger();

      secretObjectz.ForEach(so => StartCoroutine(so.ToggleOn()));
    }
  }
}
