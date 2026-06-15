using UnityEngine;

public class CloudsEffect : MonoBehaviour
{
    [SerializeField] private Transform _parallaxObject1;
    [SerializeField] private Transform _parallaxObject2;

    [SerializeField] private float _effectSpeed;

    [SerializeField] private Transform _target;
    [SerializeField] private Vector2 _offset;

    private void LateUpdate() {
        Vector3 targetPosition = new Vector3(_target.position.x + _offset.x, _target.position.y + _offset.y, 0.0f);
        transform.position = targetPosition;

        Vector3 parallaxMovement = new Vector3(_effectSpeed * Time.deltaTime, 0.0f, 0.0f);
        if (_parallaxObject1.localPosition.x > 40.0f) _parallaxObject1.localPosition = new Vector3(-40.0f, 0.0f, 0.0f);
        _parallaxObject1.localPosition += parallaxMovement;

        if (_parallaxObject2.localPosition.x > 40.0f) _parallaxObject2.localPosition = new Vector3(-40.0f, 0.0f, 0.0f);
        _parallaxObject2.localPosition += parallaxMovement;
    }
}
