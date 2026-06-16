using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public void Collect(PlayerInventory playerInventory)
    {
        // TODO: Refactor item pickup
        gameObject.SetActive(false);
        playerInventory.HealthPotAmount++;
    }
}
