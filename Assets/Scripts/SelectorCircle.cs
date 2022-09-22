using UnityEngine;

public class SelectorCircle : MonoBehaviour {
    void Update() {
        transform.position = CustomStuff.Round(Camera.main.ScreenToWorldPoint(Input.mousePosition) + Vector3.forward);
    }
}
