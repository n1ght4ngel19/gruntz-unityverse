using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Core;
using GruntzUnityverse.Objectz.Secretz;

namespace GruntzUnityverse.Objectz.Switchez {
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
