using System;
using UnityEngine;

namespace GFA.MiniGames.Games.Spaceshooter
{
    public class Projectile : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent<Enemy>(out var enemy))
            {
                enemy.ApplyDamage(1);
                Destroy(gameObject);
            }
        }
    }
}
