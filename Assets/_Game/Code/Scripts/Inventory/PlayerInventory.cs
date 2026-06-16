using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private LayerMask _itemLayer;
    [SerializeField] private float _detectionRadius = 1f;

    private PlayerDependencies _deps;

    private int _healthPotAmountMax = 9;
    private int _healthPotAmount = 0;
    public int HealthPotAmount
    {
        get => _healthPotAmount;
        set
        {
            _healthPotAmount = value;
            _healthPotAmount = Mathf.Clamp(_healthPotAmount, 0, _healthPotAmountMax);
            
            OnHealthPotAmountChange?.Invoke(HealthPotAmount);
        }
    }

    public delegate void OnHealthPotAmountChangeDelegate(int amount);
    public event OnHealthPotAmountChangeDelegate OnHealthPotAmountChange;

    private void Start()
    {
        _deps = GetComponent<PlayerDependencies>();

        HealthPotAmount = 1;
    }

    private void Update()
    {
        DetectItems();
    }

    private void DetectItems()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, _detectionRadius, transform.right, 0f, _itemLayer);

        if (hit.collider == null) return;

        if (hit.collider.gameObject.TryGetComponent<ItemPickup>(out ItemPickup item))
        {
            item.Collect(this);
        }
    }

    public void UseHealthPotion()
    {
        HealthPotAmount--;
        _deps.Attributes.CurrentHealth += 60;
    }
}
