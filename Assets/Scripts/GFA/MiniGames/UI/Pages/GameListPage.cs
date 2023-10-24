using GFA.Core.UI.Pagination;
using GFA.MiniGames.Data;
using GFA.MiniGames.UI.Elements;
using UnityEngine;

namespace GFA.MiniGames.UI.Pages
{
    public class GameListPage : Page, IRouterContainer
    {
        public Router Router { get; set; }
        
        [SerializeField]
        private GameButton _gameButtonPrefab;

        [SerializeField]
        private Transform _container;
        
        [SerializeField]
        private MiniGame[] _miniGames;

        [SerializeField]
        private MiniGamePlayer _miniGamePlayer;

        protected override void OnAwake()
        {
            foreach (var minigame in _miniGames)
            {
                var gameButtonInstance = Instantiate(_gameButtonPrefab, _container);
                gameButtonInstance.MiniGame = minigame;
                gameButtonInstance.Clicked += OnMiniGameButtonClicked;
                minigame.Reset();
            }
        }

        private void OnMiniGameButtonClicked(MiniGame miniGame)
        {
            _miniGamePlayer.MiniGame = miniGame;
            var page = Router.GetPage<GamePage>();
            _miniGamePlayer.StartGame(page.Viewport);
            Router.ActivePage = page;
        }

        protected override void OnOpened()
        {
        }

        protected override void OnClosed()
        {
        }

    }
}
