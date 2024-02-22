using System.Collections.Generic;

namespace GruntzUnityverse.Objectz.Interfacez {
public interface IInteractable {
	public List<string> CompatibleItemz { get; }

	public void Interact();
}
}
