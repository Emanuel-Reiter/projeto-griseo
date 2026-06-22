using UnityEngine;

public class LevelPortal : MonoBehaviour
{
    [SerializeField] private LevelData _nextLevel;

    [SerializeField] private bool _returnToMainMenu = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        if (_returnToMainMenu)
        {
            _ = LevelManager.I.ReturnToMainMenu();
        }
        else
        {
            if (_nextLevel == null) return;
            _ = LevelManager.I.LoadLevel(_nextLevel);
        }

    }
}
