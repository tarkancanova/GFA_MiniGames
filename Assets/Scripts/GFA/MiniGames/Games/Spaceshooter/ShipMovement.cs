using System;
using UnityEngine;

namespace GFA.MiniGames.Games.Spaceshooter
{
    public class ShipMovement : MonoBehaviour
    {
        public Vector2 MovementInput { get; set; }

        [SerializeField]
        private float _acceleration = 4;

        [SerializeField]
        private float _backwardForceMultiplier = 2;
        
        [SerializeField]
        private float _maxSpeed = 5;
        
        private Rigidbody2D _rigidbody;

        public float Rotation { get; set; }
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            var dot = Vector2.Dot(MovementInput, _rigidbody.velocity.normalized);
            float extraAcceleration = 0;
            if (dot < 0)
            {
                extraAcceleration = Mathf.Abs(dot) * _backwardForceMultiplier;
            }
            _rigidbody.AddForce(MovementInput * (_acceleration + extraAcceleration), ForceMode2D.Force);

            _rigidbody.velocity = Vector2.ClampMagnitude(_rigidbody.velocity, _maxSpeed);

            _rigidbody.rotation = Rotation;
        }
    }
}
