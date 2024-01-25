using System.Collections.Generic;

namespace GruntzUnityverse.V2.DataPersistence {
  [System.Serializable]
  public class GameData {
    public string levelAddress;

    public List<GruntDataV2> gruntData;
  }
}
