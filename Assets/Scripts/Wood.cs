using UnityEngine;

/// <summary>
/// Бревно: постоянно вращается с заданной скоростью.
/// </summary>
public sealed class Wood : MonoBehaviour
{
    private float _rotationSpeed = 150f;

    private void FixedUpdate()
    {
        transform.Rotate(0f, 0f, _rotationSpeed * Time.fixedDeltaTime);
    }
}