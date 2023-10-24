using GFA.MiniGames.Games.MemoryMatch;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace GFA.MiniGames.Data.Games
{
    [CreateAssetMenu(menuName = "Games/Memory Match", fileName = "MemoryMatch", order = 0)]
    public class MemoryMatch : MiniGame
    {
        [SerializeField]
        private MemoryMatchUI _uiPrefab;

        private MemoryMatchUI _uiInstance;
        
        
		[SerializeField]
		private Sprite[] _cardSprites;


		[SerializeField]
		private Vector2Int _gameSize;
        
        protected override void OnBegin()
        {
            _uiInstance = Instantiate(_uiPrefab, Context.Viewport);

            _uiInstance.CardSprites = _cardSprites;
            _uiInstance.GameSize = _gameSize;

            _uiInstance.GenerateCards();
        }

        protected override void OnTick()
        {
        }

        protected override void OnEnd()
        {
            Destroy(_uiInstance.gameObject);
        }
    }
}
