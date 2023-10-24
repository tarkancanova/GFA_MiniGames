using System;
using UnityEngine;

namespace GFA.MiniGames.Games.WordScramble.Data
{
    [CreateAssetMenu(menuName = "Game Data/WordScramble/Word List")]
    public class WordList : ScriptableObject
    {
        [SerializeField]
        private string[] _words;
        public string[] Words => _words;

#if UNITY_EDITOR
        public void SetWords(string[] words)
        {
            _words = words;
        }
#endif
    }
}
