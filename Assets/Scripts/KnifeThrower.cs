using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

/// <summary>
/// Управляет бросками ножей, сбором яблок и переходами между этапами.
/// Контейнер Woods двигается, яблоки суммируются, кинжалы обновляются.
/// </summary>
public sealed class KnifeThrower : MonoBehaviour
{
    [Header("Knife Settings")]
    [SerializeField] private GameObject _knifePrefab;
    [SerializeField] private Transform _spawnPoint;

    [Header("Stage Settings")]
    [SerializeField] private Transform _woodParent;
    
    private float _throwForce = 150f;
    private int[] _appleGoals = { 2, 4, 5, 8 };
    
    private float _transitionDuration = 1.2f;
    private float _preTransitionDelay = 0.6f;
    private float _postTransitionDelay = 0.3f;
    
    private GameObject _currentKnife;
    private int _currentStageIndex;
    private int _collectedApplesStage;
    private bool _isTransitioning;

    private void Start()
    {
        _currentStageIndex = 0;
        _collectedApplesStage = 0;
        SpawnKnife();
        GameManager.Instance.ResetAppleCounter();
    }

    private void Update()
    {
        if (_isTransitioning) return;

        if (Mouse.current.leftButton.wasPressedThisFrame && GameManager.Instance.GetKnifeCount() > 0)
            ThrowKnife();

        if (_collectedApplesStage >= _appleGoals[_currentStageIndex] ||
            GameManager.Instance.GetKnifeCount() <= 0)
            StartCoroutine(MoveToNextStage());
    }

    private void ThrowKnife()
    {
        GameManager.Instance.RegisterKnifeUsed();
        Rigidbody2D rb = _currentKnife.GetComponent<Rigidbody2D>();
        rb.AddForce(Vector2.up * _throwForce, ForceMode2D.Impulse);
        _currentKnife.transform.parent = null;
        SpawnKnife();
    }

    private void SpawnKnife()
    {
        _currentKnife = Instantiate(_knifePrefab, _spawnPoint.position, Quaternion.identity, transform);
    }

    private IEnumerator MoveToNextStage()
    {
        _isTransitioning = true;

        GameObject currentWood = _woodParent.GetChild(_currentStageIndex).gameObject;
        _currentStageIndex++;

        if (_currentStageIndex >= _appleGoals.Length)
        {
            yield return new WaitForSeconds(_preTransitionDelay);
            GameManager.Instance.GameOver();
            yield break;
        }

        yield return new WaitForSeconds(_preTransitionDelay);

        Transform nextWood = _woodParent.GetChild(_currentStageIndex);
        float deltaX = currentWood.transform.position.x - nextWood.position.x;

        Vector3 startPos = _woodParent.position;
        Vector3 targetPos = startPos + new Vector3(deltaX, 0f, 0f);

        float elapsed = 0f;
        while (elapsed < _transitionDuration)
        {
            float t = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(elapsed / _transitionDuration));
            _woodParent.position = Vector3.Lerp(startPos, targetPos, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        _woodParent.position = targetPos;
        yield return new WaitForSeconds(_postTransitionDelay);

        GameManager.Instance.RefreshKnives();
        _collectedApplesStage = 0;
        _isTransitioning = false;
    }

    public void OnAppleCollected()
    {
        _collectedApplesStage++;
        GameManager.Instance.RegisterAppleHit();
    }
}
