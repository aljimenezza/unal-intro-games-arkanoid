using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArkanoidController : MonoBehaviour
{
    private const string BALL_PREFAB_PATH = "Prefabs/Ball";
    private readonly Vector2 BALL_INIT_POSITION = new Vector2(0, -0.86f);

    private int _totalScore = 0;
    
    [SerializeField]
    private GridController _gridController;
    
    [Space(20)]
    [SerializeField]
    private List<LevelData> _levels = new List<LevelData>();
    
    private int _currentLevel = 0;

    private Ball _ballPrefab = null;
    private List<Ball> _balls = new List<Ball>();

    private const string POWERUP_PREFAB_PATH = "Prefabs/PowerUp";
    private PowerUp _powerupPrefab = null;
    public Dictionary<int, PowerUp> _powerups = new Dictionary<int, PowerUp>();

    private PowerUpType _type = PowerUpType.Small;
    
    private void Start()
    {
        ArkanoidEvent.OnBallReachDeadZoneEvent += OnBallReachDeadZone;
        ArkanoidEvent.OnBlockDestroyedEvent += OnBlockDestroyed;
        ArkanoidEvent.OnPowerUpReachPaddleEvent += OnPowerUpReachPaddle;
    }

    private void OnDestroy()
    {
        ArkanoidEvent.OnBallReachDeadZoneEvent -= OnBallReachDeadZone;
        ArkanoidEvent.OnBlockDestroyedEvent -= OnBlockDestroyed;
        ArkanoidEvent.OnPowerUpReachPaddleEvent -= OnPowerUpReachPaddle;
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            InitGame();
        }
    }
    
    private void InitGame()
    {
        _currentLevel = 0;
        _totalScore = 0;
        
        _gridController.BuildGrid(_levels[0]);
        SetInitialBall();

        ArkanoidEvent.OnGameStartEvent?.Invoke();
        ArkanoidEvent.OnScoreUpdatedEvent?.Invoke(0, _totalScore);
    }
    
    private void SetInitialBall()
    {
        ClearBalls();

        Ball ball = CreateBallAt(BALL_INIT_POSITION);
        ball.Init();
        _balls.Add(ball);
    }
    
    private Ball CreateBallAt(Vector2 position)
    {
        if (_ballPrefab == null)
        {
            _ballPrefab = Resources.Load<Ball>(BALL_PREFAB_PATH);
        }

        return Instantiate(_ballPrefab, position, Quaternion.identity);
    }
    
    private void ClearBalls()
    {
        for (int i = _balls.Count - 1; i >= 0; i--)
        {
            _balls[i].gameObject.SetActive(false);
            Destroy(_balls[i]);
        }
        
        _balls.Clear();
    }
    
    private void OnBallReachDeadZone(Ball ball)
    {
        ball.Hide();
        _balls.Remove(ball);
        Destroy(ball.gameObject);

        CheckGameOver();
    }

    private float _RandomValue;

    int id = 0;
    private PowerUp CreatePowerUpAt(Vector2 position)
    {
        _RandomValue = Random.value;
        if (_RandomValue < 0.125f)
        {
            _type = PowerUpType.Small;
        }

        if (_RandomValue > 0.125f && _RandomValue < 0.25f)
        {
            _type = PowerUpType.Large;
        }

        if (_RandomValue > 0.25f && _RandomValue < 0.375f) 
        {
            _type = PowerUpType.Fast;
        }

        if (_RandomValue > 0.375f && _RandomValue < 0.5f)
        {
            _type = PowerUpType.Slow;
        }

        if (_RandomValue > 0.5f && _RandomValue < 0.625f)
        {
            _type = PowerUpType.Cincuenta;
        }

        if (_RandomValue > 0.625f && _RandomValue < 0.75f)
        {
            _type = PowerUpType.Cien;
        }

        if (_RandomValue > 0.75f && _RandomValue < 0.875f)
        {
            _type = PowerUpType.DoscientosCincuenta;
        }

        if (_RandomValue > 0.875f)
        {
            _type = PowerUpType.Quinientos;
        }

        if (_powerupPrefab == null)
        {
            _powerupPrefab = Resources.Load<PowerUp>(POWERUP_PREFAB_PATH);
        }
        _powerups.Add(id, _powerupPrefab);
        _powerupPrefab.SetData(id, _type);
        _powerupPrefab.Init();
        id++;
        return Instantiate(_powerupPrefab, position, Quaternion.identity);
    }

    public PowerUp GetPowerUpBy(int id)
    {
        if (_powerups.TryGetValue(id, out PowerUp power))
        {
            return power;
        }

        return null;
    }

    private void OnPowerUpReachPaddle(int powerupId)
    {
        PowerUp pickedPowerUp = GetPowerUpBy(powerupId);
        pickedPowerUp.HidePowerUp();
        pickedPowerUp.gameObject.SetActive(false);
        // _powerups.Remove(powerupId);
        // Destroy(pickedPowerUp.gameObject);

    }
    
    private void CheckGameOver()
    {
        if (_balls.Count == 0)
        {
            //Game over
            ClearBalls();
            
            Debug.Log("Game Over: LOSE!!!");
            ArkanoidEvent.OnGameOverEvent?.Invoke();
        }
    }
    
    private void OnBlockDestroyed(int blockId)
    {

        BlockTile blockDestroyed = _gridController.GetBlockBy(blockId);
        if (blockDestroyed != null)
        {
            _totalScore += blockDestroyed.Score;
            ArkanoidEvent.OnScoreUpdatedEvent?.Invoke(blockDestroyed.Score, _totalScore);
            if(Random.value < 0.35f)
            {
                CreatePowerUpAt(blockDestroyed.transform.position);
            }
        }

        if (_gridController.GetBlocksActive() == 0)
        {
            _currentLevel ++;
            ArkanoidEvent.OnLevelUpdatedEvent?.Invoke(_currentLevel);
            if (_currentLevel >= _levels.Count)
            {
                ClearBalls();
                Debug.LogError("Game Over: WIN!!!!");
            }
            else
            {
                SetInitialBall();
                _gridController.BuildGrid(_levels[_currentLevel]);
            }

        }
    }
}