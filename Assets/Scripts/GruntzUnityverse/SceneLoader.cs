using System;
using GruntzUnityverse.Enumz;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GruntzUnityverse {
  public class SceneLoader : MonoBehaviour {
    public Area area;

    public void LoadScene() {
      switch (area) {
        case Area.RockyRoadz:
          SceneManager.LoadSceneAsync($"RR_{gameObject.name}");

          break;
        case Area.Gruntziclez:
          SceneManager.LoadSceneAsync($"GR_{gameObject.name}");

          break;
        case Area.TroubleInTheTropicz:
          SceneManager.LoadSceneAsync($"TITT_{gameObject.name}");

          break;
        case Area.HighOnSweetz:
          SceneManager.LoadSceneAsync($"HOS_{gameObject.name}");

          break;
        case Area.HighRollerz:
          SceneManager.LoadSceneAsync($"HR_{gameObject.name}");

          break;
        case Area.HoneyIShrunkTheGruntz:
          SceneManager.LoadSceneAsync($"HISTG_{gameObject.name}");

          break;
        case Area.TheMiniatureMasterz:
          SceneManager.LoadSceneAsync($"TMM_{gameObject.name}");

          break;
        case Area.GruntzInSpace:
          SceneManager.LoadSceneAsync($"GIS_{gameObject.name}");

          break;
        case Area.None:
          throw new ArgumentException("The value of area should be one of the 9 areas.");
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
  }
}
