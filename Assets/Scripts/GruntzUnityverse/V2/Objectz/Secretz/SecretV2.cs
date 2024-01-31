using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.V2.Objectz.Switchez;
using UnityEngine;

namespace GruntzUnityverse.V2.Objectz.Secretz {
  public class SecretV2 : MonoBehaviour {
    public SecretSwitchV2 secretSwitch;
    public List<SecretObjectV2> secretObjectz;

    void Start() {
      secretSwitch = GetComponentInChildren<SecretSwitchV2>();
      secretObjectz = GetComponentsInChildren<SecretObjectV2>().ToList();
    }
  }
}
