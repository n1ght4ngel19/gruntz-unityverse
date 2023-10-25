using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.MapObjectz.BaseClasses;

namespace GruntzUnityverse.MapObjectz {
  public class WarpTrigger : MapObject {
    public List<RedWarp> warpz;

    protected override void Start() {
      base.Start();

      warpz = parent.GetComponentsInChildren<RedWarp>().ToList();

      if (warpz.Count.Equals(0)) {
        WarnWithSpriteChange("There is no Warp assigned to this Trigger, this way the Trigger won't work properly!");
      }
    }

    private void Update() {
      if (!IsGruntOnTop()) {
        return;
      }

      enabled = false;

      warpz.ForEach(warp => {
        warp.SetEnabled(warp.isEntrance);
      });
    }


    // GetGruntOnTop().audioSource.PlayOneShot(secretSpotVoice);
  }
}
