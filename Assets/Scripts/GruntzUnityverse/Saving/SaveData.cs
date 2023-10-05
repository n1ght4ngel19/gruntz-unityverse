using System.Collections.Generic;

namespace GruntzUnityverse.Saving {
  public struct SaveData {
    public List<GruntData> gruntData;
    public List<ObjectData> objectData;
    
    public SaveData(List<GruntData> gruntData, List<ObjectData> objectData) {
      this.gruntData = gruntData;
      this.objectData = objectData;
    }
  }
}
