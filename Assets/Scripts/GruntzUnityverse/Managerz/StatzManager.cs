using UnityEngine;

namespace GruntzUnityverse.Managerz {
  public class StatzManager : MonoBehaviour {
    #region Singleton Stuff

    private static StatzManager _instance;

    public static StatzManager Instance {
      get => _instance;
    }

    private void Awake() {
      if (_instance != null && _instance != this) {
        Destroy(gameObject);
      } else {
        _instance = this;
      }
    }

    #endregion


    public int hourz;
    public int minutez;
    public int secondz;

    public int survivorz;
    public int deathz;

    public int maxToolz;
    public int acquiredToolz;

    public int maxToyz;
    public int acquiredToyz;

    public int maxPowerupz;
    public int acquiredPowerupz;

    public int maxCoinz;
    public int acquiredCoinz;

    public int maxSecretz;
    public int acquiredSecretz;

    public int maxWarpLetterz;
    public int acquiredWarpLetterz;
  }
}
