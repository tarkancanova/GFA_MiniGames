using UnityEngine;

namespace GFA.MiniGames.Games.FruitNinja
{
	public interface ICuttable
	{
		public void Cut(Vector3 normal, float distance);
		public GameObject gameObject { get; }
		public bool IsCut { get; set; }
	}
}
