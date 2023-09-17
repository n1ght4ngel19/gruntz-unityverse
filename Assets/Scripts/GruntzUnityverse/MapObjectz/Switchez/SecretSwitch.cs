using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.MapObjectz.BaseClasses;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse.MapObjectz.Switchez {
  public class SecretSwitch : ObjectSwitch {
    public List<SecretObject> secretObjectz;
    private const float TimeStep = 0.1f;
    // -------------------------------------------------------------------------------- //

    protected override void Start() {
      base.Start();

      secretObjectz = parent.GetComponentsInChildren<SecretObject>().ToList();
      
      if (secretObjectz.Count.Equals(0)) {
        WarnWithSpriteChange("There is no Secret Object assigned to this Switch, this way the Switch won't work properly!");
      }
    }

    private void Update() {
      if (!GameManager.Instance.currentLevelManager.allGruntz.Any(grunt => grunt.AtNode(ownNode))) {
        return;
      }

      ToggleSwitch();
      Addressables.LoadAssetAsync<AudioClip>("Assets/Audio/Soundz/Sound_SecretSwitch.wav").Completed += handle => {
        GameManager.Instance.audioSource.PlayOneShot(handle.Result);
      };

      foreach (SecretObject secretObject in secretObjectz) {
        StartCoroutine(HandleSecretObject(secretObject));
      }

      enabled = false;
      StatzManager.acquiredSecretz++;
    }

    private IEnumerator HandleSecretObject(SecretObject secretObject) {
      while (secretObject.delay > 0) {
        secretObject.delay -= TimeStep;

        yield return new WaitForSeconds(TimeStep);
      }

      secretObject.ActivateSecret();

      while (secretObject.duration > 0) {
        secretObject.duration -= TimeStep;

        yield return new WaitForSeconds(TimeStep);
      }

      secretObject.DeactivateSecret();
    }
  }
}
