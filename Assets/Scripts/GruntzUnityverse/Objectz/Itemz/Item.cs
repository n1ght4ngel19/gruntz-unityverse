using System.Collections;
using GruntzUnityverse.Actorz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Itemz {
  public abstract class Item : MonoBehaviour {
    protected abstract void Start();

    public abstract IEnumerator Use(Grunt grunt);
  }
}
