using GruntzUnityverse.V2.Itemz;

namespace GruntzUnityverse.V2.Objectz {
public interface IObjectHolder {
	public LevelItem HeldItem { get; set; }

	public void DropItem(bool isSceneLoaded);
}
}
