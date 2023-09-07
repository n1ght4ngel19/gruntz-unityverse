using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.MapObjectz.Pyramidz;
using GruntzUnityverse.MapObjectz.Switchez;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse.MapObjectz {
  public class Checkpoint : MonoBehaviour {
    private List<CheckpointSwitch> _switchez;
    private List<CheckpointPyramid> _pyramidz;
    private List<CheckpointFlag> _flagz;
    // ------------------------------------------------------------ //

    private void Start() {
      _switchez = transform.parent.GetComponentsInChildren<CheckpointSwitch>().ToList();
      _pyramidz = transform.parent.GetComponentsInChildren<CheckpointPyramid>().ToList();
      _flagz = transform.parent.GetComponentsInChildren<CheckpointFlag>().ToList();
    }
    // ------------------------------------------------------------ //

    private void Update() {
      if (_switchez.Any(checkpointSwitch => !checkpointSwitch.isPressed)) {
        return;
      }

      Complete();
    }
    // ------------------------------------------------------------ //

    /// <summary>
    /// Takes care of everything related to completing a Checkpoint,
    /// such as saving the game or disabling the Checkpoint.
    /// </summary>
    private void Complete() {
      int idx = Random.Range(1, 4);
      // Todo: Randomized voice
      // Todo: Different clip based on clicking
      Addressables.LoadAssetAsync<AudioClip>($"Voice_Checkpoint_Good_0{idx}.wav").Completed += handle => {
        GameManager.Instance.audioSource.PlayOneShot(handle.Result);
      };

      Addressables.LoadAssetAsync<AudioClip>("Sound_CheckpointFlag_Raise.wav").Completed += handle => {
        GameManager.Instance.audioSource.clip = handle.Result;
      };

      _flagz.ForEach(flag => {
        StartCoroutine(flag.PlayAnim());
      });

      _pyramidz.ForEach(pyramid => {
        pyramid.Toggle();
        pyramid.enabled = false;
      });

      _switchez.ForEach(sw => sw.enabled = false);

      // Todo: Save game here

      enabled = false;
    }
  }
}
