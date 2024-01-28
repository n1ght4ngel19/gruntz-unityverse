using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.V2.Objectz.Pyramidz;
using GruntzUnityverse.V2.Objectz.Switchez;
using UnityEngine;

namespace GruntzUnityverse.V2 {
  public class CheckpointV2 : MonoBehaviour {
    public List<CheckpointSwitchV2> switchez;
    public List<CheckpointPyramidV2> pyramidz;
    public List<CheckpointFlag> flagz;

    // ? OnEnable / Setup
    private void Start() {
      switchez = GetComponentsInChildren<CheckpointSwitchV2>().ToList();

      if (switchez == null || switchez.Count == 0) {
        Debug.LogError($"No Switchez found for checkpoint {gameObject.name}!");
        enabled = false;
      }

      pyramidz = GetComponentsInChildren<CheckpointPyramidV2>().ToList();

      if (pyramidz == null || pyramidz.Count == 0) {
        Debug.LogError($"No Pyramidz found for checkpoint {gameObject.name}!");
        enabled = false;
      }

      flagz = GetComponentsInChildren<CheckpointFlag>().ToList();
    }

    private void Update() {
      if (switchez.TrueForAll(sw => sw.IsOn)) {
        Debug.Log("Checkpoint cleared!");

        switchez.ForEach(sw => sw.DisableTrigger());
        pyramidz.ForEach(pyramid => pyramid.Toggle());
        flagz.ForEach(flag => flag.PlayAnim());

        enabled = false;
      }
    }
  }
}
