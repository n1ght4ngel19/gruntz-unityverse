using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.V2.Core;

namespace GruntzUnityverse.V2.Objectz.Switchez {
public class SecretSwitch : Switch {
	public List<SecretObject> secretObjectz;

	public override void Setup() {
		secretObjectz = transform.parent.GetComponentsInChildren<SecretObject>(true).ToList();
	}

	public override void ToggleOn() {
		base.ToggleOn();

		DisableTrigger();

		GameManager.Instance.levelStatz.discoveredSecretz++;
		secretObjectz.ForEach(so => StartCoroutine(so.ToggleOn()));
	}
}
}
