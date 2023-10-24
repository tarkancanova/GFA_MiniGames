using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GFA.MiniGames.Games.WordScramble
{
	public class WordCharacter : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
	{
		private char _character = 'a';

		public char Character
		{
			get => _character;
			set
			{
				_character = value;
				UpdateText();
			}
		}

		public event Action CharacterUpdated;

		[SerializeField] private TMP_Text _text;

		private Vector3 _dragStartLocalPosition;

		private void Awake()
		{
			UpdateText();
		}

		private void UpdateText()
		{ 
			_text.text = _character.ToString().ToUpperInvariant();
		}

		public void OnDrag(PointerEventData eventData)
		{
			_text.transform.position = eventData.position;
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
			_dragStartLocalPosition = _text.transform.localPosition;
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			_text.transform.localPosition = _dragStartLocalPosition;
		}

		public void OnDrop(PointerEventData eventData)
		{
			var obj = eventData.pointerDrag;
			if (obj.TryGetComponent<WordCharacter>(out var t))
			{
				var oldCharacter = _character;
				Character = t.Character;
				t.Character = oldCharacter;
				CharacterUpdated?.Invoke();
			}
		}
	}
}