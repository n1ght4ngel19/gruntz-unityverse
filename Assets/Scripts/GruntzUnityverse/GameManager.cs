using UnityEngine;

namespace GruntzUnityverse {
  public class GameManager : MonoBehaviour {
    private static GameManager _instance;

    public static GameManager Instance {
      get => _instance;
    }

    public bool isDebugMode;
  }
}
