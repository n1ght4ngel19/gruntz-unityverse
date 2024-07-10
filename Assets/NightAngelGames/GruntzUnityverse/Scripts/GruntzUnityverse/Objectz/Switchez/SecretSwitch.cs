using System.Collections.Generic;
using GruntzUnityverse.Core;
using GruntzUnityverse.Objectz.Secretz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Switchez {
public class SecretSwitch : Switch {
	public List<SecretObject> secretObjectz;

	public List<SecretTile> secretTilez;

	// --------------------------------------------------
	// Overrides
	// --------------------------------------------------

	protected override void Toggle(bool checkPressed = false) {
		base.Toggle(checkPressed);

		secretObjectz.ForEach(so => so.Toggle());
		secretTilez.ForEach(st => st.Reveal());

		Level.instance.levelStatz.discoveredSecretz++;

		Deactivate();
	}

	// --------------------------------------------------
	// Lifecycle
	// --------------------------------------------------

	protected override void Start() {
		base.Start();

		secretObjectz = GetSiblings<SecretObject>(includeInactive: true);
		secretTilez = GetSiblings<SecretTile>(includeInactive: true);
	}

	protected override void OnTriggerExit2D(Collider2D other) { }
}
}
