using GFA.MiniGames.Games.WordScramble;
using UnityEngine;

namespace GFA.MiniGames.Data.Games
{
    [CreateAssetMenu(menuName = "Games/WordScramble", fileName = "WordScramble", order = 0)]
    public class WordScramble : MiniGame
    {
        [SerializeField]
        private GameObject _gamePrefab;
        
        private GameObject _gameInstance;
        
        protected override void OnBegin()
        {
            _gameInstance = Instantiate(_gamePrefab, Context.Viewport);
        }

        protected override void OnTick()
        {
        }

        protected override void OnEnd()
        {
            Destroy(_gameInstance);
        }
    }
}
