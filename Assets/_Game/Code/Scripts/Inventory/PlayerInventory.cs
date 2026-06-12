using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private LayerMask _itemLayer;
    [SerializeField] private float _detectionRadius = 1f;

    private PlayerDependencies _deps;

    private int _healthPotAmountMax = 3;
    private int _healthPotAmount = 0;
    public int HealthPotAmount
    {
        get => _healthPotAmount;
        set
        {
            _healthPotAmount = value;
            _healthPotAmount = Mathf.Clamp(_healthPotAmount, 0, _healthPotAmountMax);
        }
    }

    private void Start()
    {
        _deps = GetComponent<PlayerDependencies>();
    }

    private void DetectItems()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, _detectionRadius, transform.right, 0f);

        if (hit.collider == null) return;

        if (hit.collider.gameObject.TryGetComponent<ItemPickup>(out ItemPickup item))
        {
            item.Collect(this);
            HealthPotAmount++;
        }
    }

    public void UseHealthPotion()
    {
        _healthPotAmount--;
        _deps.Attributes.CurrentHealth += 60;
    }
}
