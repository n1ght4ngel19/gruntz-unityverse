using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Switchez
{
  public class SecretSwitch : MonoBehaviour
  {
    [field: SerializeField] public Vector2Int OwnLocation { get; set; }
    [field: SerializeField] public Behaviour Behaviour { get; set; }
    [field: SerializeField] public List<SecretObject> SecretObjects { get; set; }
    private const float TimeStep = 0.1f;


    private void Start()
    {
      OwnLocation = Vector2Int.FloorToInt(transform.position);
      Behaviour = gameObject.GetComponent<Behaviour>();
    }

    private void Update()
    {
      if (LevelManager.Instance.PlayerGruntz
        .Any(grunt => grunt.NavComponent.OwnLocation.Equals(OwnLocation))
      )
      {
        foreach (SecretObject secretObject in SecretObjects)
        {
          StartCoroutine(HandleSecretObject(secretObject));
        }

        Behaviour.enabled = false;
      }
    }

    private IEnumerator HandleSecretObject(SecretObject secretObject)
    {
      while (secretObject.Delay > 0)
      {
        secretObject.Delay -= TimeStep;

        yield return new WaitForSeconds(TimeStep);
      }

      secretObject.ActivateSecret();

      while (secretObject.Duration > 0)
      {
        secretObject.Duration -= TimeStep;

        yield return new WaitForSeconds(TimeStep);
      }

      secretObject.DeactivateSecret();
    }
  }
}
