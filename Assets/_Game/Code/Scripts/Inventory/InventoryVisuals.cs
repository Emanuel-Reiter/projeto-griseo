using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryVisuals : MonoBehaviour
{
    private PlayerDependencies _deps;

    [SerializeField] private TMP_Text _potAmountLabel;
    [SerializeField] private Image _potImage;
 
    private void Start()
    {
        _deps = GetComponent<PlayerDependencies>();

        if(_deps != null)
        {
            _deps.Inventory.OnHealthPotAmountChange += UpdateHealthPotAmount;
        }
    }

    private void UpdateHealthPotAmount(int newAmount)
    {
        if(_potAmountLabel != null) _potAmountLabel.text = $"{newAmount}";

        if (_potImage != null)
        {
            Color defaultColor = new Color(1f, 1f, 1f, 1f);
            Color disabledColor = new Color(1f, 1f, 1f, 0.05f);

            if(newAmount > 0) _potImage.color = defaultColor;
            else _potImage.color = disabledColor;
        }
    }
}
