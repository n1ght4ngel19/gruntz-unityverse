using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

namespace Bewildered.SmartLibrary
{
    public class PrefabPreviewGenerator : PreviewGeneratorBase<GameObject>
    {
        private GameObject _targetInstance;
        private GameObject _canvasGameObject;
        private CanvasScaler _scaler;

        private List<MonoBehaviour> _components = new List<MonoBehaviour>();
        private List<bool> _cachedComponentStates = new List<bool>();

        public PrefabPreviewGenerator(PreviewRenderer renderer) : base(renderer)
        {
            _canvasGameObject = renderer.Stage.CreateGameObject("Canvas");
            Canvas canvas = _canvasGameObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = Renderer.Camera;
            canvas.planeDistance = 5;

            _scaler = _canvasGameObject.AddComponent<CanvasScaler>();
            _scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        }

        protected override bool BeforeRender(GameObject target)
        {
            if (target.transform is RectTransform)
            {
                return BeforeRenderGUI(target);
            }
            else
            {
                return BeforeRenderObject(target);
            }
        }

        private bool BeforeRenderGUI(GameObject target)
        {
            // Note: We don't need to add the target instance to the renderer scene
            // because it is instantiated as a child of the canvas.
            
            _targetInstance = Object.Instantiate(target, _canvasGameObject.transform);
            RectTransform rectTransform = _targetInstance.GetComponent<RectTransform>();
            
            // The Canvas must be enabled before getting the render bounds
            // otherwise layout components will not layout children correctly.
            _canvasGameObject.SetActive(true);

            // We get the renderable bounds after creating the instance because we need
            // to parent it to the canvas so we can force rebuild its layout (e.g. VerticalLayoutGroup component)
            
            var bounds = PreviewEditorUtility.GetGUIRenderableBounds(rectTransform);
            if (bounds == new Bounds(Vector3.zero, Vector3.zero))
                return false;
            
            _targetInstance.transform.localPosition = new Vector3(-bounds.center.x, -bounds.center.y);
            
            _scaler.referenceResolution = new Vector2(bounds.size.x, bounds.size.y);
            
            // Canvas' are not rendered in the Preview CameraType so we need to change it to the Game type.
            Renderer.Camera.cameraType = CameraType.Game;
            Renderer.Camera.nearClipPlane = 0.001f;
            Renderer.Camera.farClipPlane = 10;

            return true;
        }

        private bool BeforeRenderObject(GameObject target)
        {
            CacheAndDisableComponents(target);
            
            // Note: We use the target instance to get the renderable bounds because to get the bounds
            // of a particle system it needs to be played, and cannot be played on prefab assets.
            _targetInstance = Object.Instantiate(target, Vector3.zero, Quaternion.identity);
            Renderer.AddGameObject(_targetInstance);

            Bounds bounds = PreviewEditorUtility.GetRenderableBounds(_targetInstance, out bool has2DRenderer);

            if (bounds.size == Vector3.zero)
                return false;

            if (EditorSettings.defaultBehaviorMode == EditorBehaviorMode.Mode2D && has2DRenderer)
                PreviewEditorUtility.PositionCamera2D(Renderer.Camera, bounds, 7.5f);
            else
                PreviewEditorUtility.PositionCamera3D(Renderer.Camera, bounds, 7.5f);

            return true;
        }

        protected override void AfterRender()
        {
            Renderer.Camera.cameraType = CameraType.Preview;
            _canvasGameObject.SetActive(false);

            RevertComponentsFromCache();
            
            if (_targetInstance != null)
                Object.DestroyImmediate(_targetInstance);
        }

        public override void Cleanup()
        {
            if (_canvasGameObject != null)
                Object.DestroyImmediate(_canvasGameObject);
        }

        private void CacheAndDisableComponents(GameObject target)
        {
            // We get all components and disable them so that if they have the [ExecuteAlways] attribute
            // it will not run when we instantiate it for the preview.
            _components.AddRange(target.GetComponentsInChildren<MonoBehaviour>());
            for (int i = 0; i < _components.Count; i++)
            {
                _cachedComponentStates.Add(_components[i].enabled);
                _components[i].enabled = false;
            }
        }

        private void RevertComponentsFromCache()
        {
            for (int i = 0; i < _components.Count; i++)
            {
                _components[i].enabled = _cachedComponentStates[i];
            }
            _components.Clear();
            _cachedComponentStates.Clear();
        }
    }
}
