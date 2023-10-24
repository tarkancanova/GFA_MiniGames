using System;
using System.Collections.Generic;
using DG.Tweening;
using GFA.MiniGames.Games.Match3.BlockTypes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GFA.MiniGames.Games.Match3
{
	public class LevelData
	{
		private BlockInstance[] _blocks;

		public static EmptyBlockType EmptyBlock { get; } = ScriptableObject.CreateInstance<EmptyBlockType>();

		public Vector2Int GridSize { get; private set; }

		public event Action<BlockInstance, Vector2Int> BlockMoved;

		private List<NeighborDestructionListener> _neighborDestructionListeners =
			new List<NeighborDestructionListener>();

		private BlockInstance CreateBlockInstanceAt(int index, BlockType blockType)
		{
			var inst = new GameObject("BlockInstance", typeof(BoxCollider2D), typeof(BlockInstance));
			var blockInstance = inst.GetComponent<BlockInstance>();
			blockInstance.Position = GetPositionOfIndex(index);
			blockInstance.BlockType = blockType;
			blockInstance.LevelData = this;
			blockInstance.BlockType.OnAssigned(blockInstance);


			_blocks[index] = blockInstance;

			return blockInstance;
		}

		public void RegisterNeighborDestructionListener(NeighborDestructionListener listener)
		{
			if (!_neighborDestructionListeners.Contains(listener))
				_neighborDestructionListeners.Add(listener);
		}

		public void UnregisterNeighborDestructionListener(NeighborDestructionListener listener)
		{
			_neighborDestructionListeners.Remove(listener);
		}

		public BlockInstance GetBlock(Vector2Int position)
		{
			return _blocks[GetIndexOfPosition(position)];
		}

		public int GetIndexOfPosition(Vector2Int position)
		{
			return LevelUtility.GetIndexOfPosition(position, GridSize.x);
		}

		public Vector2Int GetPositionOfIndex(int index)
		{
			return LevelUtility.GetPositionOfIndex(index, GridSize.x);
		}

		public BlockInstance[] GetVertical(Vector2Int position)
		{
			var ret = new BlockInstance[GridSize.y];

			for (int y = 0; y < GridSize.y; y++)
			{
				var newPosition = new Vector2Int(position.x, y);
				ret[y] = GetBlock(newPosition);
			}

			return ret;
		}

		public void RemoveBlock(Vector2Int position)
		{
			var block = GetBlock(position);
			block.BlockType = EmptyBlock;

			foreach (NeighborDestructionListener listener in _neighborDestructionListeners)
			{
				if (Mathf.Abs(listener.TargetBlock.Position.x - position.x) == 1 &&
				    Mathf.Abs(listener.TargetBlock.Position.y - position.y) == 1)
				{
					listener.Execute();
				}
			}
		}


		public void Swap(Vector2Int from, Vector2Int to)
		{
			int fromIndex = GetIndexOfPosition(from);
			int toIndex = GetIndexOfPosition(to);

			BlockInstance fromBlock = _blocks[fromIndex];
			BlockInstance toBlock = _blocks[toIndex];

			_blocks[fromIndex] = toBlock;
			_blocks[toIndex] = fromBlock;

			fromBlock.Position = to;
			toBlock.Position = from;

			BlockMoved?.Invoke(fromBlock, to);
			BlockMoved?.Invoke(toBlock, from);
		}

		public BlockInstance[] GetHorizontal(Vector2Int position)
		{
			var ret = new BlockInstance[GridSize.x];

			for (int x = 0; x < GridSize.x; x++)
			{
				var newPosition = new Vector2Int(x, position.y);
				ret[x] = GetBlock(newPosition);
			}

			return ret;
		}

		public static LevelData CreateFromTemplate(LevelTemplate template)
		{
			var levelData = new LevelData();
			levelData.GridSize = template.GridSize;
			levelData._blocks = new BlockInstance[template.GridSize.x * template.GridSize.y];

			for (var i = 0; i < levelData._blocks.Length; i++)
			{
				var blockType = template.BlockTypes[i];
				if (blockType == null)
				{
					if (template.FillNullFromRandom)
					{
						var randomIndex = Random.Range(0, template.RandomBlock.Length);
						blockType = template.RandomBlock[randomIndex];
					}
					else
					{
						blockType = EmptyBlock;
					}
				}

				levelData.CreateBlockInstanceAt(i, blockType);
			}

			levelData.ApplyGravity();
			
			return levelData;
		}

		public static LevelData CreateRandom(Vector2Int gridSize, params BlockType[] blockTypes)
		{
			var levelData = new LevelData();
			levelData.GridSize = gridSize;
			levelData._blocks = new BlockInstance[gridSize.x * gridSize.y];

			for (var i = 0; i < levelData._blocks.Length; i++)
			{
				var randomBlockType = blockTypes[Random.Range(0, blockTypes.Length)];
				levelData.CreateBlockInstanceAt(i, randomBlockType);
			}
			

			return levelData;
		}


		public bool IsValidPosition(Vector2Int position)
		{
			return position.x >= 0 && position.y >= 0 && position.x < GridSize.x && position.y < GridSize.y;
		}

		private void ApplyColumnGravity(int columnIndex, int rowIndex = 0)
		{
			int lastEmptyBlockPosition = -1;

			for (int i = GridSize.y - 1 - rowIndex; i >= 0; i--)
			{
				var block = GetBlock(new Vector2Int(columnIndex, i));
				if (lastEmptyBlockPosition == -1)
				{
					if (block.BlockType == EmptyBlock)
					{
						lastEmptyBlockPosition = i;
						continue;
					}
				}
				else if (block.BlockType != EmptyBlock)
				{
					if (block.BlockType.UseGravity)
					{
						Swap(new Vector2Int(columnIndex, lastEmptyBlockPosition), block.Position);
						ApplyColumnGravity(columnIndex, rowIndex);
					}
					else
					{
						ApplyColumnGravity(columnIndex, GridSize.y - 1 - i);
					}

					break;
				}
			}
		}


		public void ApplyGravity()
		{
			for (int i = 0; i < GridSize.x; i++)
			{
				ApplyColumnGravity(i);
			}
		}

		public BlockInstance[] GetAllMatchingNeighbors(Vector2Int position)
		{
			List<BlockInstance> validBlocks = new List<BlockInstance>();
			List<int> visitedIndices = new List<int>();

			var targetBlockType = GetBlock(position).BlockType;

			void CheckNeighbors(Vector2Int position)
			{
				if (!IsValidPosition(position)) return;


				var block = GetBlock(position);
				var index = GetIndexOfPosition(position);

				if (targetBlockType != block.BlockType) return;
				if (visitedIndices.Contains(index)) return;

				visitedIndices.Add(index);
				validBlocks.Add(block);

				CheckNeighbors(position + new Vector2Int(-1, 0));
				CheckNeighbors(position + new Vector2Int(1, 0));
				CheckNeighbors(position + new Vector2Int(0, 1));
				CheckNeighbors(position + new Vector2Int(0, -1));
			}

			CheckNeighbors(position);

			return validBlocks.ToArray();
		}

		public BlockInstance[] GetInRange(Vector2Int position, int range)
		{
			List<BlockInstance> ret = new List<BlockInstance>();

			foreach (var blockInstance in _blocks)
			{
				if (Vector2Int.Distance(blockInstance.Position, position) < range)
				{
					ret.Add(blockInstance);
				}
			}

			return ret.ToArray();
		}

		public class LevelDataBuilder
		{
			private Vector2Int _size;
			private BlockType[] _blocks;

			public static LevelDataBuilder Create()
			{
				return new LevelDataBuilder();
			}

			public LevelDataBuilder SetGridSize(Vector2Int size)
			{
				_size = size;
				_blocks = new BlockType[_size.x * _size.y];
				return this;
			}

			public LevelDataBuilder SetBlock(Vector2Int position, BlockType blockType)
			{
				_blocks[GetIndexOfPosition(position)] = blockType;
				return this;
			}

			public LevelDataBuilder SetRemainingRandomly(params BlockType[] blocks)
			{
				for (var i = 0; i < _blocks.Length; i++)
				{
					var block = _blocks[i];
					if (block == null)
					{
						_blocks[i] = blocks[Random.Range(0, blocks.Length)];
					}
				}

				return this;
			}

			public int GetIndexOfPosition(Vector2Int position)
			{
				return position.x + position.y * _size.x;
			}

			public LevelData Build()
			{
				var ret = new LevelData();
				ret.GridSize = _size;
				ret._blocks = new BlockInstance[_size.x * _size.y];

				for (var i = 0; i < _blocks.Length; i++)
				{
					var blockType = _blocks[i];
					ret.CreateBlockInstanceAt(i, blockType);
				}

				return ret;
			}
		}

		public class NeighborDestructionListener
		{
			public BlockInstance TargetBlock;

			private Action Action;

			public NeighborDestructionListener(BlockInstance targetBlock, Action action)
			{
				TargetBlock = targetBlock;
				Action = action;
			}

			internal void Execute()
			{
				Action?.Invoke();
			}
		}
	}
}