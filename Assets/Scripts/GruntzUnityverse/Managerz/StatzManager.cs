using GruntzUnityverse.MapObjectz.Itemz;
using GruntzUnityverse.MapObjectz.MapItemz.Misc;
using GruntzUnityverse.MapObjectz.Switchez;
using TMPro;
using UnityEngine;

namespace GruntzUnityverse.Managerz {
  public class StatzManager : MonoBehaviour {
    private void Start() {
      timeValueText = GameObject.Find("TimeValueText")?.GetComponent<TMP_Text>();

      survivorzValueText = GameObject.Find("SurvivorzValueText")?.GetComponent<TMP_Text>();
      deathzValueText = GameObject.Find("DeathzValueText")?.GetComponent<TMP_Text>();

      toolzValueText = GameObject.Find("ToolzValueText")?.GetComponent<TMP_Text>();
      toyzValueText = GameObject.Find("ToyzValueText")?.GetComponent<TMP_Text>();
      powerupzValueText = GameObject.Find("PowerupzValueText")?.GetComponent<TMP_Text>();

      coinzValueText = GameObject.Find("CoinzValueText")?.GetComponent<TMP_Text>();
      secretzValueText = GameObject.Find("SecretzValueText")?.GetComponent<TMP_Text>();
      warpletterzValueText = GameObject.Find("WarpletterzValueText")?.GetComponent<TMP_Text>();


      maxToolz = LevelManager.Instance.mapObjectContainer.GetComponentsInChildren<Tool>().Length;
      maxToyz = LevelManager.Instance.mapObjectContainer.GetComponentsInChildren<Toy>().Length;
      maxPowerupz = LevelManager.Instance.mapObjectContainer.GetComponentsInChildren<Powerup>().Length;
      maxCoinz = LevelManager.Instance.mapObjectContainer.GetComponentsInChildren<Coin>().Length;
      maxSecretz = LevelManager.Instance.mapObjectContainer.GetComponentsInChildren<SecretSwitch>().Length;
      maxWarpletterz = LevelManager.Instance.mapObjectContainer.GetComponentsInChildren<Warpletter>().Length;
    }

    private void Update() {
      if (timeValueText is not null) {
        timeValueText.text = $"{hourz}:{minutez}:{secondz}";
      }

      if (survivorzValueText is not null) {
        survivorzValueText.text = $"{survivorz}";
      }

      if (deathzValueText is not null) {
        deathzValueText.text = $"{deathz}";
      }

      if (toolzValueText is not null) {
        toolzValueText.text = $"{acquiredToolz} OF {maxToolz}";
      }

      if (toyzValueText is not null) {
        toyzValueText.text = $"{acquiredToyz} OF {maxToyz}";
      }

      if (powerupzValueText is not null) {
        powerupzValueText.text = $"{acquiredPowerupz} OF {maxPowerupz}";
      }

      if (coinzValueText is not null) {
        coinzValueText.text = $"{acquiredCoinz} OF {maxCoinz}";
      }

      if (secretzValueText is not null) {
        secretzValueText.text = $"{acquiredSecretz} OF {maxSecretz}";
      }

      if (warpletterzValueText is not null) {
        warpletterzValueText.text = $"{acquiredWarpletterz} OF {maxWarpletterz}";
      }

      enabled = false;
    }

    public TMP_Text timeValueText;

    public TMP_Text survivorzValueText;
    public TMP_Text deathzValueText;

    public TMP_Text toolzValueText;
    public TMP_Text toyzValueText;
    public TMP_Text powerupzValueText;

    public TMP_Text coinzValueText;
    public TMP_Text secretzValueText;
    public TMP_Text warpletterzValueText;

    public static int hourz;
    public static int minutez;
    public static int secondz;

    public static int survivorz;
    public static int deathz;

    public static int maxToolz;
    public static int acquiredToolz;

    public static int maxToyz;
    public static int acquiredToyz;

    public static int maxPowerupz;
    public static int acquiredPowerupz;

    public static int maxCoinz;
    public static int acquiredCoinz;

    public static int maxSecretz;
    public static int acquiredSecretz;

    public static int maxWarpletterz;
    public static int acquiredWarpletterz;
  }
}
