using UnityEngine;

namespace GruntzUnityverse.Attributez {
    public class Stamina : MonoBehaviour, IAttribute {
        public int MaxValue { get; set; }
        public int ActualValue { get; set; }

        private void Start() {
            MaxValue = 20;
            ActualValue = MaxValue;
        }
    }
}
