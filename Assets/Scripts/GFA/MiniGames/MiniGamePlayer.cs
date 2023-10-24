using System;
using GFA.MiniGames.Data;
using UnityEngine;

namespace GFA.MiniGames
{
	public class MiniGamePlayer : MonoBehaviour
	{
		private MiniGame _miniGame;

		public MiniGame MiniGame
		{
			get => _miniGame;
			set
			{
				EndGame();

				_miniGame = value;
			}
		}

		public void StartGame(RectTransform viewport)
		{
			if (_miniGame)
			{
				_miniGame.Context = new MiniGameContext { Viewport = viewport };
				_miniGame.Begin();
			}
		}

		public void EndGame()
		{
			if (_miniGame)
			{
				_miniGame.End();
			}
		}

		private void Update()
		{
			if (_miniGame)
			{
				_miniGame.Tick();
			}
		}

		private void OnDisable()
		{
			EndGame();
		}
	}
}