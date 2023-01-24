using UnityEngine;

namespace GruntzUnityverse {
    public interface IRenderable {
        public SpriteRenderer Renderer { get; set; }
        public Sprite DisplayFrame { get; set; }
    }
}
