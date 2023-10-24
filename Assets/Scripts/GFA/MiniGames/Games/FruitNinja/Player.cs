using System;
using GFA.MiniGames.Input;
using UnityEngine;

namespace GFA.MiniGames.Games.FruitNinja
{
    public class Player : MonoBehaviour
    {
        private GameInput _gameInput;
        private Camera _camera;
        

        private void Awake()
        {
            _gameInput = new GameInput();
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
            if (!_gameInput.Generic.Click.IsPressed()) return;
            
            var mousePosition = _gameInput.Generic.PointerPosition.ReadValue<Vector2>();
            var ray = _camera.ScreenPointToRay(mousePosition);

            var mouseDelta = _gameInput.Generic.PointerDelta.ReadValue<Vector2>();

            if (mouseDelta.magnitude < 0.001f)
            {
                mouseDelta = Vector3.right;
            }
            
            
            if (Physics.Raycast(ray, out var hit))
            {
                if (hit.rigidbody.TryGetComponent(out ICuttable cuttable))
                {
                    if (!cuttable.IsCut)
                    {
                        Vector3 normal = mouseDelta.normalized;
                        var planeNormal = Vector3.Cross(normal, Vector3.forward);
                        var dist = Vector3.Dot(hit.point - hit.transform.position, planeNormal); 

                        // unsigned
                        // Vector3.Distance(Vector3.zero, Vector3.up) == 1;
                        // signed
                        // Vector3.Dot(Vector3.up - Vector3.zero, -Vector3.up) == 1;

                        // example:
                        //(0, 1, 0) - (0,0,0) = (0,1,0) * (0,1,0) = 1
                        //(0, 1, 0) - (0,0,0) = (0,1,0) * (0,-1,0) = -1

                        cuttable.Cut(planeNormal, dist);
                        cuttable.IsCut = true;
                    }
                }
            }
        }
    }
}
