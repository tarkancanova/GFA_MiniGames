using System;
using UnityEngine;

namespace GFA.MiniGames.Games.Spaceshooter
{
    public class Enemy : MonoBehaviour
    {
        public Player Player { get; set; }

        private ShipMovement _shipMovement;

        [SerializeField]
        private int _health;

        private void Awake()
        {
            _shipMovement = GetComponent<ShipMovement>();
        }

        private void Update()
        {
            var dir = (Player.transform.position - transform.position).normalized;

            _shipMovement.MovementInput = dir;
            _shipMovement.Rotation = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject == Player.gameObject)
            {
                Player.ApplyDamage(1);
                Destroy(gameObject);
            }
        }

        public void ApplyDamage(int damage)
        {
            _health -= damage;
            if (_health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
