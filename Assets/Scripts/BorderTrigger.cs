using UnityEngine;

/// <summary>
/// Завершает игру, если нож выходит за пределы игровой зоны.
/// </summary>
public sealed class BorderTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Knife")) return;

        Destroy(col.gameObject);
        GameManager.Instance.GameOver();
    }
}