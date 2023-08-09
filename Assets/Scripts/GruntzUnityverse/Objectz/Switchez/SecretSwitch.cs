using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Switchez {
  public class SecretSwitch : ObjectSwitch {
    public List<SecretObject> secretObjectz;
    private const float TimeStep = 0.1f;


    private void Update() {
      if (!LevelManager.Instance.allGruntz.Any(grunt => grunt.AtNode(ownNode))) {
        return;
      }

      ToggleSwitch();

      foreach (SecretObject secretObject in secretObjectz) {
        StartCoroutine(HandleSecretObject(secretObject));
      }

      enabled = false;
      StatzManager.Instance.acquiredSecretz++;
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
