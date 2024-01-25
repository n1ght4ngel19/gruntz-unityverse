using Vector3 = UnityEngine.Vector3;

namespace GruntzUnityverse.V2.DataPersistence {
  [System.Serializable]
  public struct GruntDataV2 {
    public string guid;
    public string gruntName;
    public Vector3 position;
  }
}
