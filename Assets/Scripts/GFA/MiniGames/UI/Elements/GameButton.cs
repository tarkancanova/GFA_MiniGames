using System;
using GFA.MiniGames.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GFA.MiniGames.UI.Elements
{
    [RequireComponent(typeof(Button))]
    public class GameButton : MonoBehaviour
    {
        private Button _button;

        [SerializeField]
        private MiniGame _miniGame;

        public MiniGame MiniGame
        {
            get => _miniGame;
            set
            {
                _miniGame = value; 
                UpdateUI();
            }
        }
        
        public event Action<MiniGame> Clicked;

        [SerializeField]
        private TMP_Text _gameTitle;
        
        [SerializeField]
        private Image _gameIcon;

        private void Awake()
        {
            _button = GetComponent<Button>();
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (_miniGame)
            {
                _gameTitle.text = _miniGame.MiniGameName;
                _gameIcon.sprite = _miniGame.Icon;
            }
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClicked);
        }
        
        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            Clicked?.Invoke(_miniGame);
        }
    }
}
