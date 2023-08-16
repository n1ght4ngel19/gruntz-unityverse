using GruntzUnityverse.MapObjectz.Itemz;

namespace GruntzUnityverse.MapObjectz.MapItemz {
  public class MapToy : MapItem {
    public Toy pickupToy;


    protected override void Start() {
      base.Start();

      pickupToy = gameObject.GetComponent<Toy>();
    }
  }
}
