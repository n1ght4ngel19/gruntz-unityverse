using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.V2.Objectz.Switchez;
using UnityEngine;

namespace GruntzUnityverse.V2.Objectz.Secretz {
public class Secret : MonoBehaviour {
	public SecretSwitch secretSwitch;
	public List<SecretObject> secretObjectz;

	void Start() {
		secretSwitch = GetComponentInChildren<SecretSwitch>();
		secretObjectz = GetComponentsInChildren<SecretObject>().ToList();
	}
}
}
