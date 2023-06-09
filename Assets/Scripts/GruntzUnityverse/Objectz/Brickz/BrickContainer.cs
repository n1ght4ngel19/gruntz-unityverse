using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Managerz;

namespace GruntzUnityverse.Objectz.Brickz {
  public class BrickContainer : MapObject {
    private List<Brick> Brickz { get; set; }

    protected override void Start() {
      base.Start();

      Brickz = GetComponentsInChildren<Brick>().ToList();
      LevelManager.Instance.BrickContainerz.Add(this);
    }

    private void Update() {
      if (Brickz.Count != 0) {
        return;
      }

      LevelManager.Instance.BrickContainerz.Remove(this);
      Destroy(gameObject);
    }
  }
}
