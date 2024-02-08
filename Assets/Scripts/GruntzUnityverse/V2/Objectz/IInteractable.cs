using System.Collections.Generic;

namespace GruntzUnityverse.V2.Objectz {
  public interface IInteractable {
    public List<string> CompatibleItemz { get; }

    public void Interact();
  }
}
