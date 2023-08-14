using GruntzUnityverse.Objectz.Itemz;

namespace GruntzUnityverse.Objectz.MapItemz {
  public class MapToy : MapItem {
    public Toy pickupToy;


    protected override void Start() {
      base.Start();

      pickupToy = gameObject.GetComponent<Toy>();
    }
  }
}
