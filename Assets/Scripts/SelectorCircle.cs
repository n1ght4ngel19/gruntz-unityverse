using UnityEngine;

public class SelectorCircle : MonoBehaviour {
    void Start() {
    }

    void Update() {
        transform.position = Custom.Round(Camera.main.ScreenToWorldPoint(Input.mousePosition) + Vector3.forward);
    }
}
