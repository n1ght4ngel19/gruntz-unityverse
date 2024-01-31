using GruntzUnityverse.V2.Itemz;

namespace GruntzUnityverse.V2.Objectz {
  public interface IObjectHolder {
    public ItemV2 HeldItem { get; set; }
    
    public void DropItem();
  }
}
