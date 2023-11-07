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
    protected override void Start() {
      base.Start();

      secretObjectz = parent.GetComponentsInChildren<SecretObject>().ToList();

      if (secretObjectz.Count.Equals(0)) {
        WarnWithSpriteChange("There is no Secret Object assigned to this Switch, this way the Switch won't work properly!");
      }
    }

    private void Update() {
      if (!IsBeingPressed()) {
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
      yield return new WaitForSeconds(secretObject.delay);

      secretObject.ActivateSecret();

      yield return new WaitForSeconds(secretObject.duration);

      secretObject.DeactivateSecret();
    }
  }
}
