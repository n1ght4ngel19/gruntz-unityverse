using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Managerz;
using UnityEngine;

namespace _Test
{
  public class TSecretSwitch : MonoBehaviour
  {
    [field: SerializeField] public Vector2Int OwnLocation { get; set; }
    [field: SerializeField] public bool CanBePressed { get; set; }
    [field: SerializeField] public List<TSecretObject> SecretObjects { get; set; }
    private const float TimeStep = 0.1f;

    private void Start()
    {
      OwnLocation = Vector2Int.FloorToInt(transform.position);
      CanBePressed = true;
    }

    private void Update()
    {
      if (!CanBePressed)
      {
        return;
      }

      if (LevelManager.Instance.testGruntz
        .Any(grunt => grunt.NavComponent.OwnLocation.Equals(OwnLocation))
      )
      {
        CanBePressed = false;

        foreach (TSecretObject secretObject in SecretObjects)
        {
          StartCoroutine(HandleSecretObject(secretObject));
        }
      }
    }

    private IEnumerator HandleSecretObject(TSecretObject secretObject)
    {
      while (secretObject.Delay > 0)
      {
        secretObject.Delay -= TimeStep;

        yield return new WaitForSeconds(TimeStep);
      }

      secretObject.SetSecretActive(true);

      while (secretObject.Duration > 0)
      {
        secretObject.Duration -= TimeStep;

        yield return new WaitForSeconds(TimeStep);
      }

      secretObject.SetSecretActive(false);

      Destroy(secretObject.gameObject);
    }
  }
}
