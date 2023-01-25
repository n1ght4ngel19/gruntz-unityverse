using GruntzUnityverse.Actorz;
using GruntzUnityverse.Attributez;

namespace GruntzUnityverse {
    public interface IKillable {
        public IAttribute Health { get; set; }
        public IAttribute Stamina { get; set; }
        public HealthBar OwnHealthBar { get; set; }
        public StaminaBar OwnStaminaBar { get; set; }
    }
}
