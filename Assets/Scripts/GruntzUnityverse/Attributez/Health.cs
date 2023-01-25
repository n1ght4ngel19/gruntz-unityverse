using UnityEngine;

namespace GruntzUnityverse.Attributez {
    public class Health : MonoBehaviour, IAttribute {
        public int MaxValue { get; set; }
        public int ActualValue { get; set; }

        private void Start() {
            MaxValue = 20;
            ActualValue = MaxValue;
        }
    }
}
