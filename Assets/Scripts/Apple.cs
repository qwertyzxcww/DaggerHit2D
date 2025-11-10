using UnityEngine;

/// <summary>
/// Яблоко: уничтожается при попадании ножа и сообщает KnifeThrower о сборе.
/// </summary>
public sealed class Apple : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.CompareTag("Knife")) return;

        KnifeThrower thrower = Object.FindFirstObjectByType<KnifeThrower>();
        if (thrower != null)
            thrower.OnAppleCollected();

        Destroy(gameObject);
    }
}