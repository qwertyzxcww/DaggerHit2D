using UnityEngine;

/// <summary>
/// Нож: фиксируется при попадании в дерево, вызывает GameOver при столкновении с другим ножом.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public sealed class Knife : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidBody;
    private bool _isAttached;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag(nameof(Wood)))
        {
            StickToWood(collider.transform);
        }
        else if (collider.CompareTag("Knife"))
        {
            Knife other = collider.GetComponent<Knife>();
            if (other != null && other._isAttached)
            {
                GameManager.Instance.GameOver();
                Destroy(gameObject);
            }
        }
    }

    private void StickToWood(Transform wood)
    {
        _rigidBody.linearVelocity = Vector2.zero;
        transform.SetParent(wood);
        _isAttached = true;
    }
}