using GruntzUnityverse.Objectz.Itemz;
using GruntzUnityverse.Objectz.MapItemz.Misc;
using GruntzUnityverse.Objectz.Switchez;
using UnityEngine;

namespace GruntzUnityverse.Managerz {
  public class StatzManager : MonoBehaviour {
    private static StatzManager _instance;

    public static StatzManager Instance {
      get => _instance;
    }

    private void Start() {
      if (_instance is not null && _instance != this) {
        Destroy(gameObject);
      } else {
        _instance = this;
      }

      maxToolz = LevelManager.Instance.mapObjectContainer.GetComponentsInChildren<Tool>().Length;
      maxToyz = LevelManager.Instance.mapObjectContainer.GetComponentsInChildren<Toy>().Length;
      maxPowerupz = LevelManager.Instance.mapObjectContainer.GetComponentsInChildren<Powerup>().Length;
      maxCoinz = LevelManager.Instance.mapObjectContainer.GetComponentsInChildren<Coin>().Length;
      maxSecretz = LevelManager.Instance.mapObjectContainer.GetComponentsInChildren<SecretSwitch>().Length;
      maxWarpletterz = LevelManager.Instance.mapObjectContainer.GetComponentsInChildren<Warpletter>().Length;
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

    public int maxWarpletterz;
    public int acquiredWarpletterz;
  }
}
