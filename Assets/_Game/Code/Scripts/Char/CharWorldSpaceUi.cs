using UnityEngine;

public class CharWorldSpaceUi : MonoBehaviour
{
    [SerializeField] private Transform _followTarget;
    [SerializeField] private Vector3 _offset = new Vector3(0f, -0.5f, 0f);

    private void Awake()
    {
        transform.SetParent(null, false);
    }

    private void LateUpdate()
    {
        transform.position = _followTarget.position + _offset;
    }
}
