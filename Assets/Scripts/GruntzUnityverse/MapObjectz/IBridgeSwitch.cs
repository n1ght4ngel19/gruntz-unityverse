using System.Collections.Generic;

namespace GruntzUnityverse.MapObjectz {
    public interface IBridgeSwitch : IMapObject, IAnimatable {
        public List<IBridge> Bridges { get; set; }
        public bool IsPressed { get; set; }

        public void ToggleBridges() {}
    }
}
