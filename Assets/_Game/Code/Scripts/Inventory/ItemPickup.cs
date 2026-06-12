using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    //[SerializeField] private Item _item;

    public void Collect(PlayerInventory playerInventory)
    {
        gameObject.SetActive(false);
    }
}
