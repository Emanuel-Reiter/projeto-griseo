using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [System.Serializable]
    public class LayerData
    {
        public GameObject layerObject;

        [Range(0f, 2f)] public float ParallaxFactor = 0.5f;
        public float ScrollSpeed = 0f;
    }

    [Header("Parallax Layers")]
    [SerializeField] private List<LayerData> _layers = new List<LayerData>();

    private Camera _mainCamera;
    private Transform _cameraTransform;

    private struct RuntimeLayer
    {
        public List<Transform> Transforms;
        public float SpriteWidth;
        public float ParallaxFactor;
        public float ScrollSpeed;
        public float OriginalY;
        public float OriginalZ;
    }
    private List<RuntimeLayer> _runtimeLayers = new List<RuntimeLayer>();

    private void Start()
    {
        _mainCamera = GetComponent<Camera>();
        if (_mainCamera == null)
        {
            Debug.LogError("ParallaxBackground must be attached to a Camera object!");
            enabled = false;
            return;
        }
        _cameraTransform = _mainCamera.transform;

        foreach (LayerData layerData in _layers)
        {
            if (layerData.layerObject == null)
            {
                Debug.LogWarning("Parallax layer has no assigned GameObject. Skipping.");
                continue;
            }

            SpriteRenderer originalRenderer = layerData.layerObject.GetComponent<SpriteRenderer>();
            if (originalRenderer == null)
            {
                Debug.LogWarning($"GameObject '{layerData.layerObject.name}' has no SpriteRenderer. Skipping.");
                continue;
            }

            float spriteWidth = originalRenderer.bounds.size.x;
            if (spriteWidth <= 0.001f)
            {
                Debug.LogWarning($"Sprite '{layerData.layerObject.name}' has zero width. Skipping.");
                continue;
            }

            float cameraWidth = _mainCamera.orthographicSize * 2f * _mainCamera.aspect;
            int neededCopies = Mathf.CeilToInt(cameraWidth / spriteWidth) + 2;
            neededCopies = Mathf.Max(neededCopies, 2);

            List<Transform> allTransforms = new List<Transform> { layerData.layerObject.transform };
            Transform parent = layerData.layerObject.transform.parent;

            for (int i = 1; i < neededCopies; i++)
            {
                GameObject clone = Instantiate(layerData.layerObject, parent);
                Destroy(clone.GetComponent<Parallax>());
                allTransforms.Add(clone.transform);
            }

            float originalY = layerData.layerObject.transform.position.y;
            float originalZ = layerData.layerObject.transform.position.z;

            _runtimeLayers.Add(new RuntimeLayer
            {
                Transforms = allTransforms,
                SpriteWidth = spriteWidth,
                ParallaxFactor = layerData.ParallaxFactor,
                ScrollSpeed = layerData.ScrollSpeed,
                OriginalZ = originalZ
            });
        }
    }

    private void LateUpdate()
    {
        if (_mainCamera == null) return;

        float currentCameraX = _cameraTransform.position.x;
        float currentCameraY = _cameraTransform.position.y;

        foreach (RuntimeLayer layer in _runtimeLayers)
        {
            float targetX = (currentCameraX * layer.ParallaxFactor) + (layer.ScrollSpeed * Time.time);
            float startX = Mathf.Floor(targetX / layer.SpriteWidth) * layer.SpriteWidth;

            for (int i = 0; i < layer.Transforms.Count; i++)
            {
                Vector3 newPos = new Vector3(
                    startX + (i * layer.SpriteWidth),
                    currentCameraY,
                    layer.OriginalZ
                );
                layer.Transforms[i].position = newPos;
            }
        }
    }
}