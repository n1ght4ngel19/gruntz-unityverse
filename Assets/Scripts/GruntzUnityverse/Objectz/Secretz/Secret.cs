using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Objectz.Switchez;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Secretz {
public class Secret : MonoBehaviour {
	public SecretSwitch secretSwitch;
	public List<SecretObject> secretObjectz;

	void Start() {
		secretSwitch = GetComponentInChildren<SecretSwitch>();
		secretObjectz = GetComponentsInChildren<SecretObject>().ToList();
	}
}
}
