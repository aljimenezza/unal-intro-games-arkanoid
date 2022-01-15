using UnityEngine;
using Random = UnityEngine.Random;

public enum PowerUpType
{
    Small,
    Large,
    Fast,
    Slow,
    Cincuenta,
    Cien,
    DoscientosCincuenta,
    Quinientos
}

public class PowerUp : MonoBehaviour
{ 
    // [SerializeField]
    // private float _Speed = 0.02f;

    private const string POWERUP_SMALL_PATH = "Sprites/PowerUps/Small";
    private const string POWERUP_LARGE_PATH = "Sprites/PowerUps/Large";
    private const string POWERUP_FAST_PATH = "Sprites/PowerUps/Fast";
    private const string POWERUP_SLOW_PATH = "Sprites/PowerUps/Slow";
    private const string POWERUP_CINCUENTA_PATH = "Sprites/PowerUps/Cincuenta";
    private const string POWERUP_CIEN_PATH = "Sprites/PowerUps/Cien";
    private const string POWERUP_DOSCIENTOSCINCUENTA_PATH = "Sprites/PowerUps/DoscientosCincuenta";
    private const string POWERUP_QUINIENTOS_PATH = "Sprites/PowerUps/Quinientos";

    public int _id;
    private PowerUpType _type = PowerUpType.Large;
    private Rigidbody2D _rb;
    private SpriteRenderer _renderer;
    private Collider2D _collider;

    private float _RandomValue;

    public void SetData(int id, PowerUpType type)
    {
        _id = id;
        _type = type;
    }

    public void Init()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        
        _collider.enabled = true;
        gameObject.SetActive(true);

        _renderer = GetComponentInChildren<SpriteRenderer>();

        _renderer.sprite =GetPowerUpSprite(_type, 0);
    }


    static Sprite GetPowerUpSprite(PowerUpType type, int state)
    {
        string path = string.Empty;
        if (type == PowerUpType.Small)
        {
            path = string.Format(POWERUP_SMALL_PATH, state);
        }

        if (type == PowerUpType.Large)
        {
            path = string.Format(POWERUP_LARGE_PATH, state);
        }

        if (type == PowerUpType.Fast)
        {
            path = string.Format(POWERUP_FAST_PATH, state);
        }

        if (type == PowerUpType.Slow)
        {
            path = string.Format(POWERUP_SLOW_PATH, state);
        }

        if (type == PowerUpType.Cincuenta)
        {
            path = string.Format(POWERUP_CINCUENTA_PATH, state);
        }

        if (type == PowerUpType.Cien)
        {
            path = string.Format(POWERUP_CIEN_PATH, state);
        }

        if (type == PowerUpType.DoscientosCincuenta)
        {
            path = string.Format(POWERUP_DOSCIENTOSCINCUENTA_PATH, state);
        }

        if (type == PowerUpType.Quinientos)
        {
            path = string.Format(POWERUP_QUINIENTOS_PATH, state);
        }

        if (string.IsNullOrEmpty(path))
        {
            return null;
        }

        return Resources.Load<Sprite>(path);
    }


    public void HidePowerUp()
    {
        _collider.enabled = false;
        gameObject.SetActive(false);
    }
}
