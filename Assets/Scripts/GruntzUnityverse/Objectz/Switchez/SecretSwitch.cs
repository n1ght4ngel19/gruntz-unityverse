using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Switchez {
  public class SecretSwitch : ObjectSwitch {
    [field: SerializeField] public Behaviour Behaviour { get; set; }
    [field: SerializeField] public List<SecretObject> SecretObjectz { get; set; }
    private const float TimeStep = 0.1f;


    private void Update() {
      if (LevelManager.Instance.PlayerGruntz.Any(grunt => grunt.NavComponent.OwnLocation.Equals(OwnLocation))) {
        Renderer.sprite = PressedSprite;

        foreach (SecretObject secretObject in SecretObjectz) {
          StartCoroutine(HandleSecretObject(secretObject));
        }

        enabled = false;
      }
    }

    private IEnumerator HandleSecretObject(SecretObject secretObject) {
      while (secretObject.Delay > 0) {
        secretObject.Delay -= TimeStep;

        yield return new WaitForSeconds(TimeStep);
      }

      secretObject.ActivateSecret();

      while (secretObject.Duration > 0) {
        secretObject.Duration -= TimeStep;

        yield return new WaitForSeconds(TimeStep);
      }

      secretObject.DeactivateSecret();
    }
  }
}
