using UnityEngine;
using TMPro;

public class UILevelScore : MonoBehaviour
{
    private const string SCORE_TEXT_TEMPLATE = "{0} pts";
    private const string LEVEL_TEXT_TEMPLATE = "Level {0}";
    
    private CanvasGroup _canvasGroup;
    private TextMeshProUGUI _scoreText;
    private TextMeshProUGUI _levelText;
    
    void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0;

        _scoreText = transform.Find("ScoreText").GetComponent<TextMeshProUGUI>();
        _levelText = transform.Find("LevelText").GetComponent<TextMeshProUGUI>();
        
        ArkanoidEvent.OnScoreUpdatedEvent += OnScoreUpdated;
        ArkanoidEvent.OnLevelUpdatedEvent += OnLevelUpdated;
        ArkanoidEvent.OnGameStartEvent += OnGameStart;
        ArkanoidEvent.OnGameOverEvent += OnGameOver;
    }
    
    private void OnDestroy()
    {
        ArkanoidEvent.OnScoreUpdatedEvent -= OnScoreUpdated;
        ArkanoidEvent.OnLevelUpdatedEvent -= OnLevelUpdated;
        ArkanoidEvent.OnGameStartEvent -= OnGameStart;
        ArkanoidEvent.OnGameOverEvent -= OnGameOver;
    }

    private void OnGameStart()
    {
        _canvasGroup.alpha = 1;
    }
   
    private void OnGameOver()
    {
        _canvasGroup.alpha = 0;
    }
    
    private void OnScoreUpdated(int score, int totalScore)
    {
        _scoreText.text = string.Format(SCORE_TEXT_TEMPLATE, totalScore);
    }

    private void OnLevelUpdated(int currentLevel)
    {
        _levelText.text = string.Format(LEVEL_TEXT_TEMPLATE, currentLevel);
    }
}