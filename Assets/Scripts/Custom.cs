using UnityEngine;

public static class Custom {
    public static Vector3 Round(Vector3 vector) {
        return new Vector3(
            Mathf.Floor(vector.x / 1) + 0.5f,
            Mathf.Floor(vector.y / 1) + 0.5f,
            vector.z
        );
    }
}
