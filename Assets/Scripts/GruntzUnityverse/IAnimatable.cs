using System.Collections.Generic;
using UnityEngine;

namespace GruntzUnityverse {
    public interface IAnimatable : IRenderable {
        public List<Sprite> AnimationFrames { get; set; }
    }
}
