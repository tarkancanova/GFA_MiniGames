using UnityEngine;

namespace GFA.MiniGames.Games.Match3
{
	public abstract class BlockType : ScriptableObject
	{
		[SerializeField] private GameObject _graphics;

		public abstract bool UseGravity { get; }

		public virtual GameObject CreateGraphics()
		{
			return Instantiate(_graphics);
		}

		public abstract void ExecuteClickInteraction(BlockInstance blockInstance);

		public abstract void OnAssigned(BlockInstance blockInstance);
	}
}