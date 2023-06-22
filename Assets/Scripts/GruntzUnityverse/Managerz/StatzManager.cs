using UnityEngine;

namespace GruntzUnityverse.Managerz {
  public class StatzManager : MonoBehaviour {
    private static StatzManager _Instance;

    public static StatzManager Instance {
      get => _Instance;
    }

    private void Start() {
      if (_Instance != null && _Instance != this) {
        Destroy(gameObject);
      } else {
        _Instance = this;
      }
    }


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
