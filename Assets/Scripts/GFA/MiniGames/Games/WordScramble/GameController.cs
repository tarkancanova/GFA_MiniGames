using System;
using System.Linq;
using GFA.MiniGames.Games.WordScramble.Data;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GFA.MiniGames.Games.WordScramble
{
    public class GameController : MonoBehaviour
    {
        [SerializeField]
        private WordList _wordList;

        private string _activeWord;

        [SerializeField]
        private CharacterContainer _characterContainer;

        private void Start()
        {
            PickWord();
        }

        private void OnEnable()
        {
            _characterContainer.WordChanged += OnWordChanged;
        }
        
        private void OnDisable()
        {
            _characterContainer.WordChanged -= OnWordChanged;
        }

        private void OnWordChanged(string obj)
        {
            if (obj.ToLowerInvariant() == _activeWord.ToLowerInvariant())
            {
                Debug.Log("Congrats!");
                PickWord();
            }
        }

        public void PickWord()
        {
            _activeWord = _wordList.Words[Random.Range(0, _wordList.Words.Length)];
            _characterContainer.CreateCharacters(String.Join("",_activeWord.OrderBy(x => Random.value)));
        }
    }
}
