using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GFA.MiniGames.Games.MemoryMatch
{
	public class MemoryMatchCard: Selectable, IPointerClickHandler
	{
		[SerializeField]
		private Image _cardImage;
		public Sprite Sprite { get; set; }

		private bool _isOpened;

		public event Action CardOpened;

		[SerializeField]
		private GameObject _frontFace;
		
		[SerializeField]
		private GameObject _backFace;

		public void UpdateUI()
		{
			_cardImage.sprite = Sprite;
		}

		public void Open()
		{
			_frontFace.SetActive(true);
			_backFace.SetActive(false);
			_isOpened = true;
			CardOpened?.Invoke();
		}

		public void Close()
		{
			_frontFace.SetActive(false);
			_backFace.SetActive(true);
			_isOpened = false;
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			if (!_isOpened)
			{
				Open();
			}
		}
	}
}