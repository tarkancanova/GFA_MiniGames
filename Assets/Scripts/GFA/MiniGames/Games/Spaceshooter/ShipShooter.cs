using System;
using DG.Tweening;
using UnityEngine;

namespace GFA.MiniGames.Games.Spaceshooter
{
    public class ShipShooter : MonoBehaviour
    {
        [SerializeField]
        private Transform[] _shootPoints;

        [SerializeField]
        private GameObject _projectilePrefab;

        private float _lastShootTime;
        
        [SerializeField]
        private float _shootRate;

        [SerializeField]
        private float _projectileSpeed;

        [SerializeField]
        private Transform _graphics;

        public void Shoot()
        {
            if (_lastShootTime + _shootRate < Time.time)
            {
                foreach (var shootPoint in _shootPoints)
                {
                    var inst = Instantiate(_projectilePrefab, shootPoint.position, shootPoint.rotation);
                    if (inst.TryGetComponent<Rigidbody2D>(out var rigidbody2D))
                    {
                        rigidbody2D.velocity = shootPoint.up * _projectileSpeed;
                        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), inst.GetComponent<Collider2D>(), true);
                        Destroy(inst, 10);
                    }
                }

                _graphics.DOKill(true);
                _graphics.DOLocalMove(new Vector3(0,-.2f,0), 0.05f).From(Vector3.zero);
                _graphics.DOLocalMove(Vector2.zero, 0.05f).SetDelay(0.1f);
                _lastShootTime = Time.time;
            }
        }
    }
}
