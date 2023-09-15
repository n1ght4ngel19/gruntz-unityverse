using GruntzUnityverse.Actorz;
using UnityEngine;

namespace GruntzUnityverse.MapObjectz.BaseClasses {
  public abstract class Item : MonoBehaviour {
    public SpriteRenderer spriteRenderer;
    public Grunt ownGrunt;
    public string mapItemName;
    public string category;
    // ------------------------------------------------------------ //

    protected virtual void Start() {
      spriteRenderer = GetComponent<SpriteRenderer>();
      ownGrunt = gameObject.GetComponent<Grunt>();
    }
  }
}
