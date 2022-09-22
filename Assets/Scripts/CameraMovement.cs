using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private int mapWidthInTiles;
    [SerializeField]
    private int mapHeightInTiles;
    [SerializeField]
    private Camera cam;

    // The smaller the ScrollRate, the faster the camera moves
    private const int ScrollRate = 30;

    private float targetZoom;
    private float zoomFactor = 3f;
    private float zoomLerpSpeed = 10;
    
    
    
    void Update() {
        ScrollWithKeys();
        ZoomWithMouse();
    }

    void ZoomWithMouse() {
        targetZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomFactor;
        targetZoom = Math.Clamp(targetZoom, 4f, 11f);
        cam.orthographicSize = Mathf.Lerp(
            cam.orthographicSize,
            targetZoom,
            Time.deltaTime * zoomLerpSpeed
        );
    }
    
    private void ScrollWithKeys() {
        float camHalfWidth = cam.orthographicSize * cam.aspect;
        
        if (cam.transform.position.x + camHalfWidth > mapWidthInTiles - 0.05)
            LimitMovement("right");
        else if (cam.transform.position.x - camHalfWidth < 0.05)
            LimitMovement("left");
        else if (cam.transform.position.y + cam.orthographicSize > mapHeightInTiles - 0.05)
            LimitMovement("up");
        else if (cam.transform.position.y - cam.orthographicSize < 0.05)
            LimitMovement("down");
        else
            MoveFreely();
    }
    
    private void LimitMovement(string direction) {
        switch (direction) {
            case "up":
                if (Input.GetKey(KeyCode.DownArrow))
                    cam.transform.position += Vector3.down / ScrollRate;
                if (Input.GetKey(KeyCode.LeftArrow))
                    cam.transform.position += Vector3.left / ScrollRate;
                if (Input.GetKey(KeyCode.RightArrow))
                    cam.transform.position += Vector3.right / ScrollRate;
                break;
            case "down":
                if (Input.GetKey(KeyCode.UpArrow))
                    cam.transform.position += Vector3.up / ScrollRate;
                if (Input.GetKey(KeyCode.LeftArrow))
                    cam.transform.position += Vector3.left / ScrollRate;
                if (Input.GetKey(KeyCode.RightArrow))
                    cam.transform.position += Vector3.right / ScrollRate;
                break;
            case "left":
                if (Input.GetKey(KeyCode.UpArrow))
                    cam.transform.position += Vector3.up / ScrollRate;
                if (Input.GetKey(KeyCode.DownArrow))
                    cam.transform.position += Vector3.down / ScrollRate;
                if (Input.GetKey(KeyCode.RightArrow))
                    cam.transform.position += Vector3.right / ScrollRate;
                break;
            case "right":
                if (Input.GetKey(KeyCode.UpArrow))
                    cam.transform.position += Vector3.up / ScrollRate;
                if (Input.GetKey(KeyCode.DownArrow))
                    cam.transform.position += Vector3.down / ScrollRate;
                if (Input.GetKey(KeyCode.LeftArrow))
                    cam.transform.position += Vector3.left / ScrollRate;
                break;
        }
    }

    private void MoveFreely() {
        if (Input.GetKey(KeyCode.UpArrow))
            cam.transform.position += Vector3.up / ScrollRate;
        if (Input.GetKey(KeyCode.DownArrow))
            cam.transform.position += Vector3.down / ScrollRate;
        if (Input.GetKey(KeyCode.LeftArrow))
            cam.transform.position += Vector3.left / ScrollRate;
        if (Input.GetKey(KeyCode.RightArrow))
            cam.transform.position += Vector3.right / ScrollRate;
    }
}
