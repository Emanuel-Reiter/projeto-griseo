using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    [SerializeField] private AudioClip _bossTheme;
    [SerializeField] private NpcStateManager _bossRef;

    private Collider2D _collider;

    [Header("Doors")]
    [SerializeField] private GameObject _doorEntrance;
    [SerializeField] private GameObject _doorExit;


    private void Start()
    {
        if (_bossRef != null)
        {
            _bossRef.ToggleNpc(false);
            _bossRef.Deps.Attributes.OnDieEvent += StopBossFight;
        }

        _collider = GetComponent<Collider2D>();

        OpenDoors();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        CloseDoor();

        if (_bossTheme != null) MusicManager.I.Play(_bossTheme);
        if (_bossRef != null) _bossRef.ToggleNpc(true);

        _collider.enabled = false;
    }

    private void StopBossFight()
    {
        MusicManager.I.Stop();
        OpenDoors();
    }

    private void OpenDoors()
    {
        if (_doorEntrance != null) _doorEntrance.gameObject.SetActive(false);
        if (_doorExit != null) _doorExit.gameObject.SetActive(false);
    }

    private void CloseDoor()
    {
        if(_doorEntrance != null) _doorEntrance.gameObject.SetActive(true);
        if (_doorExit != null) _doorExit.gameObject.SetActive(true);
    }
}
