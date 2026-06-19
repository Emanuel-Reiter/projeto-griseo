using DG.Tweening;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    private bool _hasBeenCollected = false;
    private Collider2D _collider;

    private void Start()
    {
        _collider = GetComponent<Collider2D>();
    }

    public void Collect(PlayerInventory playerInventory)
    {
        if (_hasBeenCollected) return;

        _hasBeenCollected = true;
        _collider.enabled = false;

        // TODO: Refactor item pickup
        Sequence pickupSequence = DOTween.Sequence();

        Vector3 targetPos = playerInventory.transform.position + Vector3.up * 2f;

        pickupSequence.Append(transform.DOScale(Vector3.zero, 0.5f)).SetEase(Ease.InOutQuad);
        pickupSequence.Join(transform.DOMove(targetPos, 0.5f)).SetEase(Ease.InOutQuad);
        pickupSequence.OnComplete(() => { gameObject.SetActive(false); });

        playerInventory.HealthPotAmount++;
    }
}
