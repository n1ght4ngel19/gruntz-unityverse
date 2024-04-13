using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Core;
using GruntzUnityverse.Objectz.Secretz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Switchez {
public class SecretSwitch : Switch {
	public List<SecretObject> secretObjectz;
	public List<SecretTile> secretTilez;

	public override void Setup() {
		base.Setup();

		secretObjectz = transform.parent.GetComponentsInChildren<SecretObject>(true).ToList();
		secretTilez = transform.parent.GetComponentsInChildren<SecretTile>(true).ToList();
	}

	public override void Toggle() {
		base.Toggle();

		DisableTrigger();
		Level.instance.levelStatz.discoveredSecretz++;
		secretObjectz.ForEach(so => StartCoroutine(so.Toggle()));
		secretTilez.ForEach(st => StartCoroutine(st.Reveal()));
	}

	protected override void OnTriggerExit2D(Collider2D other) { }
}
}
