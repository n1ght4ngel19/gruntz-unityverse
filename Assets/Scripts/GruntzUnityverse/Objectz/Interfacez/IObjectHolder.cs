using GruntzUnityverse.Itemz.Base;

namespace GruntzUnityverse.Objectz.Interfacez {
public interface IObjectHolder {
	public LevelItem HeldItem { get; set; }

	public void DropItem(bool isSceneLoaded);
}
}
