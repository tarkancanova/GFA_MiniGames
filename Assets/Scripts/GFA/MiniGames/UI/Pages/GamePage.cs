using GFA.Core.UI.Pagination;
using UnityEngine;
using UnityEngine.UI;

namespace GFA.MiniGames.UI.Pages
{
    public class GamePage : Page, IRouterContainer
    {
        public Router Router { get; set; }
        
        [SerializeField] private RectTransform _viewport;
        public RectTransform Viewport => _viewport;
        
        [SerializeField]
        private MiniGamePlayer _miniGamePlayer;
        
        [SerializeField]
        private Button _closeButton;
        
        protected override void OnOpened()
        {
            _closeButton.onClick.AddListener(OnCloseButtonClicked);
        }

        protected override void OnClosed()
        {
            _closeButton.onClick.RemoveListener(OnCloseButtonClicked);
        }

        private void OnCloseButtonClicked()
        {
            _miniGamePlayer.EndGame();
            Router.SetPage<GameListPage>();
        }
    }
}
