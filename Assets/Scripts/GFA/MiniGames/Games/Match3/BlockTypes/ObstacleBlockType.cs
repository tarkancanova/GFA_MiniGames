using UnityEngine;

namespace GFA.MiniGames.Games.Match3.BlockTypes
{
	[CreateAssetMenu(menuName = "Game Data/Match3/Obstacle Block")]
    public class ObstacleBlockType : BlockType
    {
        public override bool UseGravity => false;

        public override void ExecuteClickInteraction(BlockInstance blockInstance)
        {
        }

        public override void OnAssigned(BlockInstance blockInstance)
        {
        }
    }
}
