using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace GFA.MiniGames.Games.MemoryMatch
{
	public class MemoryMatchUI : MonoBehaviour
	{
		[SerializeField] private MemoryMatchCard _cardPrefab;

		[SerializeField] private GridLayoutGroup _container;

		private MemoryMatchCard _selectedCard;
		public Sprite[] CardSprites { get; set; }
		public Vector2Int GameSize { get; set; }

		public void GenerateCards()
		{
			var uniqueCards = CardSprites.OrderBy(x => Random.value).Take(GameSize.x * GameSize.y / 2).ToArray();
			uniqueCards = uniqueCards.Concat(uniqueCards.OrderBy(x=> Random.value)).ToArray();

			_container.constraintCount = GameSize.x;

			var i = 0;
			for (int x = 0; x < GameSize.x; x++)
			{
				for (int y = 0; y < GameSize.y; y++)
				{
					var inst = Instantiate(_cardPrefab, _container.transform);
					inst.Sprite = uniqueCards[i++];
					inst.UpdateUI();
					inst.Close();
					inst.CardOpened += () => { OnCardOpened(inst); };
				}
			}
		}


		private void OnCardOpened(MemoryMatchCard inst)
		{
			if (_selectedCard)
			{
				if (_selectedCard.Sprite == inst.Sprite)
				{
					Debug.Log("CORRECT!");
					_selectedCard = null;
				}
				else
				{
					_selectedCard.Close();
					inst.Close();
					_selectedCard = null;
				}
			}
			else
			{
				_selectedCard = inst;
			}
		}
	}
}