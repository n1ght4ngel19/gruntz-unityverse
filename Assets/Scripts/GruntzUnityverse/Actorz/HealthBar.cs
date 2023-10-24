﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse.Actorz {
  public class HealthBar : MonoBehaviour {
    public SpriteRenderer spriteRenderer;
    public List<Sprite> frames;

    private void Start() {
      spriteRenderer = GetComponent<SpriteRenderer>();

      Addressables.LoadAssetAsync<Sprite[]>("HealthBar.png").Completed += handle => {
        frames = handle.Result.ToList();
      };
    }
  }
}
