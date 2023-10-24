using UnityEngine;
using System;
using UnityEngine.UI;

namespace GFA.MiniGames.Games.Match3
{
	public class LevelGrid : MonoBehaviour
	{
		private LevelData _levelData;

		[SerializeField] private Vector2Int _gridSize;

		[SerializeField] private BlockType[] _blocks;

		[SerializeField] private Transform _content;

		[SerializeField] private BlockType _specialBlock;
		[SerializeField] private BlockType _obstacleBlock;
		[SerializeField] private BlockType _listenerBlock;

		[SerializeField] private LevelTemplate _levelTemplate;
		
		[SerializeField]
		private float _cellSize = 1;

		[SerializeField]
		private float _gravity = -9.81f;

		private void Start()
		{
			// _levelData = LevelData.CreateRandom(_gridSize, _blocks);
			var levelDataBuilder = LevelData.LevelDataBuilder.Create()
				.SetGridSize(_gridSize)
				.SetBlock(new Vector2Int(0, 5), _specialBlock)
				.SetBlock(new Vector2Int(5, 0), _specialBlock)
				.SetBlock(new Vector2Int(3, 10), _obstacleBlock)
				.SetBlock(new Vector2Int(4, 10), _listenerBlock)
				.SetRemainingRandomly(_blocks);

			// _levelData = levelDataBuilder.Build();
			
			_levelData = LevelData.CreateFromTemplate(_levelTemplate);
			_gridSize = _levelTemplate.GridSize;

			for (int y = 0; y < _gridSize.y; y++)
			{
				for (int x = 0; x < _gridSize.x; x++)
				{
					var blockPosition = new Vector2Int(x, y);
					var blockInstance = _levelData.GetBlock(blockPosition);
					blockInstance.transform.SetParent(_content, false);
					blockInstance.transform.localPosition = GetCellPosition(blockPosition);
					blockInstance.DisplayPosition = GetCellPosition(blockPosition);
				}
			}

			_levelData.BlockMoved += OnBlockMoved;
		}

		private void OnBlockMoved(BlockInstance block, Vector2Int from)
		{
			block.DisplayPosition = GetCellPosition(block.Position);
		}

		private void Update()
		{
			HandleBlockMovement();
		}

		private void HandleBlockMovement()
		{
			for (int y = 0; y < _gridSize.y; y++)
			{
				for (int x = 0; x < _gridSize.x; x++)
				{
					MoveBlockPosition(_levelData.GetBlock(new Vector2Int(x, y)));
				}
			}
		}

		private void MoveBlockPosition(BlockInstance blockInstance)
		{
			var blockTransform = blockInstance.transform;

			float vertical = 5;

			if (blockTransform.position.y > blockInstance.DisplayPosition.y)
			{
				blockInstance.VerticalVelocity -= _gravity * Time.deltaTime;
				vertical = blockInstance.VerticalVelocity;
			}
			else
			{
				blockInstance.VerticalVelocity = 0;
			}

			var newPosition = new Vector3();
			
			newPosition.x = Mathf.MoveTowards(blockTransform.position.x, blockInstance.DisplayPosition.x,
				5 * Time.deltaTime);
					
			newPosition.y = Mathf.MoveTowards(blockTransform.position.y, blockInstance.DisplayPosition.y,
				vertical * Time.deltaTime);

					
			blockTransform.position = newPosition;
		}

		private Vector3 GetCellPosition(Vector2Int position)
		{
			var center = new Vector3((_gridSize.x - 1) * _cellSize * 0.5f, -(_gridSize.y - 1) * _cellSize * 0.5f);
			return new Vector3(position.x * _cellSize, -position.y * _cellSize) - center;
		}
	}
}