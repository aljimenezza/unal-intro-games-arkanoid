using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickPowerUp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
   {
      if (other.transform.TryGetComponent<PowerUp>(out PowerUp powerup))
      {
          int id = powerup._id;
          ArkanoidEvent.OnPowerUpReachPaddleEvent?.Invoke(id);
      }
   }
}
