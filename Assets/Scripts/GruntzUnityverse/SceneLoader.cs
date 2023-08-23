using System;
using GruntzUnityverse.Enumz;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse {
  public class SceneLoader : MonoBehaviour {
    public Area area;

    /// <summary>
    /// Loads a scene that corresponds the SceneLoader's gameObject's name.
    /// </summary>
    /// <exception cref="ArgumentException">Throws an error when the Area specified on the gameObject isn't valid.</exception>
    public void LoadScene() {
      string prefix = "";

      switch (area) {
        case Area.RockyRoadz:
          prefix = "RR_";

          break;
        case Area.Gruntziclez:
          prefix = "GR_";

          break;
        case Area.TroubleInTheTropicz:
          prefix = "TITT_";

          break;
        case Area.HighOnSweetz:
          prefix = "HOS_";

          break;
        case Area.HighRollerz:
          prefix = "HR_";

          break;
        case Area.HoneyIShrunkTheGruntz:
          prefix = "HISTG_";

          break;
        case Area.TheMiniatureMasterz:
          prefix = "TMM_";

          break;
        case Area.GruntzInSpace:
          prefix = "GIS_";

          break;
        case Area.None:
          throw new ArgumentException("The value of area should be one of the 9 areas.");
      }

      Addressables.LoadSceneAsync($"Assets/Levelz/{area}/{prefix}{gameObject.name}.unity");
    }
  }
}
