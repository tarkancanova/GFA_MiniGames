using GFA.MiniGames.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GFA.MiniGames.Games.Match3
{
    public class Player : MonoBehaviour
    {
        private GameInput _gameInput;

        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
            _gameInput = new GameInput();
        }
        
        private void OnEnable()
        {
            _gameInput.Enable();
            _gameInput.Generic.Click.performed += OnClicked;
        }
        
        private void OnDisable()
        {
            _gameInput.Disable();
            _gameInput.Generic.Click.performed -= OnClicked;
        }

        private void OnClicked(InputAction.CallbackContext obj)
        {

            var worldPosition = _camera.ScreenToWorldPoint(_gameInput.Generic.PointerPosition.ReadValue<Vector2>());
            var hit = Physics2D.Raycast(worldPosition, Vector2.zero);
            if (hit.collider)
            {
                if (hit.collider.TryGetComponent<BlockInstance>(out var blockInstance))
                {
                    blockInstance.OnClicked();
                }
            }
        }
    }
}
