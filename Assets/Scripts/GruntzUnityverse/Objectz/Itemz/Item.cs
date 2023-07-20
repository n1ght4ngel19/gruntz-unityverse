using System.Collections;
using GruntzUnityverse.Actorz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Itemz {
  public abstract class Item : MonoBehaviour {
    public SpriteRenderer spriteRenderer;


    protected virtual void Start() {
      spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public abstract IEnumerator Use(Grunt grunt);
  }
}
