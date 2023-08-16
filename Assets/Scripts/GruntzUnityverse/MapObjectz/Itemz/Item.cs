using System.Collections;
using GruntzUnityverse.Actorz;
using UnityEngine;

namespace GruntzUnityverse.MapObjectz.Itemz {
  public abstract class Item : MonoBehaviour {
    public SpriteRenderer spriteRenderer;
    public Grunt ownGrunt;


    protected virtual void Start() {
      spriteRenderer = GetComponent<SpriteRenderer>();
      ownGrunt = gameObject.GetComponent<Grunt>();
    }

    public abstract IEnumerator Use(Grunt grunt);
  }
}
