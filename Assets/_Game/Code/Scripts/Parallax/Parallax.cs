using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float _objectSizeX;
    private float _objectStartPosition;

    [SerializeField] private Transform _target;
    [SerializeField] private Vector2 _offset;

    [SerializeField][Range(0f, 1f)] private float _parallaxFaxtor;

    private void Start() {
        if (_target == null)
        {
            _target = Camera.main.transform;
        }

        _objectStartPosition = transform.position.x;
        _objectSizeX = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void LateUpdate() {
        if (_target == null) return;

        float parallaxDistance = _target.position.x * _parallaxFaxtor;

        transform.position = new Vector3(
            _objectStartPosition + parallaxDistance,
            _target.position.y - _offset.y,
            transform.position.z);

        float objectToScreenRelation = _target.position.x * (1 - _parallaxFaxtor);
        if (objectToScreenRelation > _objectStartPosition + _objectSizeX) _objectStartPosition += _objectSizeX;
        else if (objectToScreenRelation < _objectStartPosition - _objectSizeX) _objectStartPosition -= _objectSizeX;
    }
}
