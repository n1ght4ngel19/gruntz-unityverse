using System;
using GruntzUnityverse;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;


namespace NightAngelGames.GruntzUnityverse.Scripts.GruntzUnityverse.CameraMovement {
public class CameraTarget : MonoBehaviour {
    public CinemachineCamera controlCamera;

    private Camera _controlledCamera;

    private Camera _interfaceCamera;

    public Vector2 input;

    private Vector3 positionDelta => new(input.x, input.y, 0);

    public BoxCollider2D boundingBox;

    public Tilemap boundsMap;

    public BoundsInt bounds;

    [Range(5, 30)]
    public float moveSpeed;

    [Range(5, 20)]
    public float zoomSpeed;

    public bool keyZoomIn;

    public bool keyZoomOut;

    public bool scrollZoomIn;

    public bool scrollZoomOut;

    private float maxVerticalOrthoSize => bounds.size.y / 2;

    private float maxHorizontalOrthoSize => bounds.size.x / (2 * controlCamera.Lens.Aspect);

    private float minOrthoSize => 6;

    private float maxOrthoSize => Mathf.Min(maxVerticalOrthoSize, maxHorizontalOrthoSize);

    private GameActions _actions;
    
    private static readonly float _interfaceCameraOffsetScale = 1.775f;

    private void OnDrawGizmosSelected() {
        transform.position = new(
            ClampedInBox(transform.position.x, true, bounds, 6, _controlledCamera.aspect),
            ClampedInBox(transform.position.y, false, bounds, 6, _controlledCamera.aspect),
            -10
        );
    }

    private void OnValidate() {
        // If the object is not in any scene, return
        if (gameObject.scene.name == transform.parent.name || gameObject.scene.name is Namez.MainMenuName or Namez.LoadMenuName) {
            return;
        }

        boundsMap = GameObject.Find("MainMap").GetComponent<Tilemap>();
        boundsMap.CompressBounds();
        bounds = boundsMap.cellBounds;

        boundingBox = !boundsMap.TryGetComponent(out BoxCollider2D box) ? boundsMap.gameObject.AddComponent<BoxCollider2D>() : box;
        FindFirstObjectByType<CinemachineConfiner2D>().BoundingShape2D = boundingBox;

        _interfaceCamera = GameObject.Find(Namez.InterfaceCameraName).GetComponent<Camera>();
        GameObject.Find(Namez.SidebarUIName).GetComponent<Canvas>().worldCamera = _interfaceCamera;

        _controlledCamera = GameObject.Find(Namez.ControlledCameraName).GetComponent<Camera>();
        GameObject.Find(Namez.HelpboxUIName).GetComponent<Canvas>().worldCamera = _controlledCamera;
    }

    private void Awake() {
        _actions = new();
        _actions.Camera.Move.performed += MoveCamera;
        _actions.Camera.Move.canceled += StopCamera;
    }

    private void OnEnable() {
        _actions.Camera.ZoomIn.performed += StartZoomIn;
        _actions.Camera.ZoomIn.canceled += StopZoomIn;

        _actions.Camera.ZoomOut.performed += StartZoomOut;
        _actions.Camera.ZoomOut.canceled += StopZoomOut;

        _actions.Camera.ScrollZoom.performed += StartScrollZoom;
        _actions.Camera.ScrollZoom.canceled += StopScrollZoom;

        _actions.Enable();
    }

    private void Update() {
        HandleKeyZoom();

        HandleScrollZoom();

        transform.position += positionDelta * (moveSpeed * Time.deltaTime);

        transform.position = new(
            ClampedInBox(transform.position.x, true, bounds, _controlledCamera.orthographicSize, _controlledCamera.aspect),
            ClampedInBox(transform.position.y, false, bounds, _controlledCamera.orthographicSize, _controlledCamera.aspect),
            -10
        );

        _interfaceCamera.orthographicSize = controlCamera.Lens.OrthographicSize;
        _interfaceCamera.transform.localPosition = new(controlCamera.Lens.OrthographicSize * _interfaceCameraOffsetScale, 0, 0);
    }

    private void OnDisable() {
        _actions.Disable();
    }


    #region Input Action Callbacks

    private void MoveCamera(InputAction.CallbackContext ctx) {
        input = ctx.ReadValue<Vector2>();
    }

    private void StopCamera(InputAction.CallbackContext ctx) {
        input = Vector2.zero;
    }

    private void StartZoomIn(InputAction.CallbackContext ctx) {
        keyZoomIn = true;
    }

    private void StopZoomIn(InputAction.CallbackContext ctx) {
        keyZoomIn = false;
    }

    private void StartZoomOut(InputAction.CallbackContext ctx) {
        keyZoomOut = true;
    }

    private void StopZoomOut(InputAction.CallbackContext ctx) {
        keyZoomOut = false;
    }

    private void StartScrollZoom(InputAction.CallbackContext ctx) {
        if (ctx.ReadValue<Vector2>().y > 0) {
            scrollZoomIn = true;
        }
        else {
            scrollZoomOut = true;
        }
    }

    private void StopScrollZoom(InputAction.CallbackContext ctx) {
        scrollZoomIn = false;
        scrollZoomOut = false;
    }

    #endregion


    private void HandleKeyZoom() {
        if (keyZoomIn) {
            controlCamera.Lens.OrthographicSize = Mathf.Clamp(controlCamera.Lens.OrthographicSize - zoomSpeed * Time.deltaTime, minOrthoSize, maxOrthoSize);
        }
        else if (keyZoomOut) {
            controlCamera.Lens.OrthographicSize = Mathf.Clamp(controlCamera.Lens.OrthographicSize + zoomSpeed * Time.deltaTime, minOrthoSize, maxOrthoSize);
        }
    }

    private void HandleScrollZoom() {
        if (scrollZoomIn) {
            controlCamera.Lens.OrthographicSize = Mathf.Clamp(controlCamera.Lens.OrthographicSize - zoomSpeed * Time.deltaTime, minOrthoSize, maxOrthoSize);
        }
        else if (scrollZoomOut) {
            controlCamera.Lens.OrthographicSize = Mathf.Clamp(controlCamera.Lens.OrthographicSize + zoomSpeed * Time.deltaTime, minOrthoSize, maxOrthoSize);
        }
    }

    private float ClampedInBox(float value, bool horizontal, BoundsInt box, float cameraSize, float cameraAspect) {
        const float displacement = 0.5f;

        return horizontal
            ? Mathf.Clamp(value, box.min.x - displacement + cameraSize * cameraAspect, box.max.x - displacement - cameraSize * cameraAspect)
            : Mathf.Clamp(value, box.min.y - displacement + cameraSize, box.max.y - displacement - cameraSize);
    }
}
}
