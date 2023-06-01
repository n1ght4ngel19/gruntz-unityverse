using System.Collections;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Enumz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Itemz {
  public class Tool : MonoBehaviour {
    [field: SerializeField] public ToolType Type { get; set; }

    public virtual IEnumerator Use(Grunt grunt) {
      yield return null;
    }
  }
}
