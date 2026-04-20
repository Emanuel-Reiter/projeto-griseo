using UnityEditor.Purchasing;
using UnityEngine;

public class NpcTargetDetection : MonoBehaviour
{
    private Transform _targetRef;
    public Transform TargetRef => _targetRef;
    public bool HasTarget { get; private set; }

    [Header("Target search params")]
    [SerializeField] private float _searchRadius = 8f;
    [SerializeField] private LayerMask _targetLayer;

    private void Update()
    {
        SearchTargets();
    }

    private void SearchTargets()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, _searchRadius, _targetLayer);
        if (hit != null)
        {
            _targetRef = hit.transform;
            HasTarget = true;
        }
        else
        {
            _targetRef = null;
            HasTarget = false;
        }
    }
}
