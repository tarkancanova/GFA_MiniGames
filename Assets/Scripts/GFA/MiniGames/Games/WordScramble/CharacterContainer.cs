using System;
using UnityEngine;

namespace GFA.MiniGames.Games.WordScramble
{
    public class CharacterContainer : MonoBehaviour
    {
        public string Word
        {
            get
            {
                var str = "";
                foreach (var character in _characters)
                {
                    str += character.Character;
                }
                return str;
            }
        }

        public event Action<string> WordChanged;

        [SerializeField]
        private WordCharacter _characterPrefab;

        [SerializeField]
        private Transform _content;

        private WordCharacter[] _characters;

        public void CreateCharacters(string word)
        {
            foreach (Transform c in _content)
            {
                Destroy(c.gameObject);
            }

            _characters = new WordCharacter[word.Length];
            for (int i = 0; i < word.Length; i++)
            {
                var character = word[i];
                var inst = Instantiate(_characterPrefab, _content);
                inst.CharacterUpdated += OnCharacterUpdated;
                inst.Character = character;
                _characters[i] = inst;
            }
        }

        private void OnCharacterUpdated()
        {
            WordChanged?.Invoke(Word);
        }
    }
}
