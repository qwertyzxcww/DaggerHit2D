using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Управляет UI и логикой игры: меню, счётчики, рестарт, переходы.
/// </summary>
public sealed class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI Panels")]
    [SerializeField] private GameObject _startPanel;
    [SerializeField] private GameObject _gamePanel;
    [SerializeField] private GameObject _losePanel;

    [Header("UI Texts")]
    [SerializeField] private TMP_Text _knifeCountText;
    [SerializeField] private TMP_Text _appleCountText;
    [SerializeField] private TMP_Text _finalAppleCountText;

    [Header("Buttons")]
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _restartButton;

    private int _appleCount;
    private int _knifeCount = 8;
    private bool _isGameActive;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (_startButton != null)
        {
            _startButton.onClick.RemoveAllListeners();
            _startButton.onClick.AddListener(StartGame);
        }

        if (_restartButton != null)
        {
            _restartButton.onClick.RemoveAllListeners();
            _restartButton.onClick.AddListener(RestartGame);
        }
    }

    private void Start()
    {
        ShowStartMenu();
    }

    public void ShowStartMenu()
    {
        _startPanel.SetActive(true);
        _gamePanel.SetActive(false);
        _losePanel.SetActive(false);
        _isGameActive = false;
        ResetCounters();
    }

    public void StartGame()
    {
        _startPanel.SetActive(false);
        _gamePanel.SetActive(true);
        _losePanel.SetActive(false);
        _isGameActive = true;
        ResetCounters();
    }

    public void GameOver()
    {
        if (!_isGameActive) return;

        _isGameActive = false;
        _gamePanel.SetActive(false);
        _losePanel.SetActive(true);
        _finalAppleCountText.text = _appleCount.ToString();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RegisterAppleHit()
    {
        if (!_isGameActive) return;
        _appleCount++;
        UpdateCounters();
    }

    public void RegisterKnifeUsed()
    {
        if (!_isGameActive) return;
        _knifeCount--;
        UpdateCounters();
    }

    public int GetKnifeCount() => _knifeCount;

    public void ResetAppleCounter()
    {
        _appleCount = 0;
        UpdateCounters();
    }

    public void RefreshKnives()
    {
        _knifeCount = 8;
        UpdateCounters();
    }

    private void ResetCounters()
    {
        _appleCount = 0;
        _knifeCount = 8;
        UpdateCounters();
    }

    private void UpdateCounters()
    {
        if (_appleCountText != null)
            _appleCountText.text = _appleCount.ToString();

        if (_knifeCountText != null)
            _knifeCountText.text = _knifeCount.ToString();
    }
}
