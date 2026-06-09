using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class EnemyDamage : MonoBehaviour
{
    [SerializeField] private int _damage = 10;
    [SerializeField] private int _knockback = 2;
    [SerializeField] private float _cooldown = 0.5f;
    [SerializeField] private LayerMask _targetLayer;

    private BoxCollider2D _col;

    private Dictionary<GameObject, float> _nextDamageAllowed = new Dictionary<GameObject, float>();

    private void Start()
    {
        _col = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        Vector2 center = new Vector2(transform.position.x, transform.position.y + (_col.size.y / 2f));

        Collider2D[] hits = Physics2D.OverlapBoxAll(center, _col.size, 0f, _targetLayer);

        foreach (Collider2D hit in hits)
        {
            GameObject player = hit.gameObject;

            if (!_nextDamageAllowed.ContainsKey(player) || Time.time >= _nextDamageAllowed[player])
            {
                CharAttributesManager playerHealth = player.GetComponent<CharAttributesManager>();
                if (playerHealth != null)
                {
                    Vector2 targetPos = hit.transform.position;
                    Vector2 dmgPos = transform.position;
                    Vector2 dirKb = new Vector2(targetPos.x - dmgPos.x, 0f) * _knockback;

                    DamageData damageData = new DamageData(
                    _damage,
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    16,
                    dirKb
                );

                    playerHealth.TakeDamage(damageData);
                }

                _nextDamageAllowed[player] = Time.time + _cooldown;
            }
        }

        List<GameObject> toRemove = new List<GameObject>();

        foreach (var kvp in _nextDamageAllowed)
        {
            if (kvp.Key == null) toRemove.Add(kvp.Key);
        }

        foreach (var key in toRemove) _nextDamageAllowed.Remove(key);
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;
        Gizmos.color = Color.red;
        Vector2 center = new Vector2(transform.position.x, transform.position.y + (_col.size.y / 2f));
        Gizmos.DrawWireCube(center, _col.size);
    }
}