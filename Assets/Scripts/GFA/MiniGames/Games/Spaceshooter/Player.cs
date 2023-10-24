using System;
using GFA.MiniGames.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GFA.MiniGames.Games.Spaceshooter
{
    public class Player : MonoBehaviour
    {
        private GameInput _gameInput;
        private ShipMovement _shipMovement;
        private ShipShooter _shooter;
        
        private Camera _camera;
        
        [SerializeField]
        private int _health;

        private void Awake()
        {
            _gameInput = new GameInput();
            _shipMovement = GetComponent<ShipMovement>();
            _shooter = GetComponent<ShipShooter>();
            _camera = Camera.main;
        }
        
        private void OnEnable()
        {
            _gameInput.Enable();
        }

        private void OnDisable()
        {
            _gameInput.Disable();
        }

        private void Update()
        {
            _shipMovement.MovementInput = _gameInput.Generic.Navigation.ReadValue<Vector2>();
            var pointerPosition = _gameInput.Generic.PointerPosition.ReadValue<Vector2>();
            
            var worldPoint = _camera.ScreenToWorldPoint(pointerPosition);
            
            var dir = (worldPoint - transform.position).normalized;
            var rot = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;

            _shipMovement.Rotation = rot;

            if (_gameInput.Generic.Click.IsPressed())
            {
                _shooter.Shoot();
            }
        }

        public void ApplyDamage(int damage)
        {
            _health -= damage;
            if (_health <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
