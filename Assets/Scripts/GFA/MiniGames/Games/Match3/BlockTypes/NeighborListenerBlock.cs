using UnityEngine;

namespace GFA.MiniGames.Games.Match3.BlockTypes
{
	[CreateAssetMenu(menuName = "Game Data/Match3/Neighbor Listener Block")]
	public class NeighborListenerBlock : BlockType
	{
		public override bool UseGravity => true;

		public override void ExecuteClickInteraction(BlockInstance blockInstance)
		{
		}

		public override void OnAssigned(BlockInstance blockInstance)
		{
			blockInstance.LevelData.RegisterNeighborDestructionListener(new LevelData.NeighborDestructionListener(
				blockInstance,
				() => OnNeighborDestroyed(blockInstance)
			));
		}

		private void OnNeighborDestroyed(BlockInstance blockInstance)
		{
			Debug.Log("Neighbor destroyed");
		}
	}
}