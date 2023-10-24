using GFA.MiniGames.Games.Match3.BlockTypes;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;

namespace GFA.MiniGames.Games.Match3.Editor
{
	[CustomEditor(typeof(LevelTemplate))]
	public class LevelTemplateEditor : UnityEditor.Editor
	{
		private static bool _isGridEditorOpen = true;

		private Vector2Int _selectedCell;

		public override void OnInspectorGUI()
		{
			var template = target as LevelTemplate;
			
			var gridSizeX = serializedObject.FindProperty("_gridSizeX");
			var gridSizeY = serializedObject.FindProperty("_gridSizeY");
			var fillNullRandomProperty = serializedObject.FindProperty("_fillNullFromRandom");

			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(gridSizeX, new GUIContent("Grid Width"));
			EditorGUILayout.PropertyField(gridSizeY, new GUIContent("Grid Height"));
			if (EditorGUI.EndChangeCheck())
			{
				serializedObject.FindProperty("_blockTypes").ClearArray();
				serializedObject.FindProperty("_blockTypes").arraySize =
					gridSizeX.intValue * gridSizeY.intValue;
				
				serializedObject.ApplyModifiedProperties();
				return;
			}

			EditorGUILayout.PropertyField(serializedObject.FindProperty("_randomBlocks"),
				new GUIContent("Random Blocks"));


			_isGridEditorOpen = EditorGUILayout.Foldout(_isGridEditorOpen, "Grid Editor");

			if (_isGridEditorOpen)
			{
				EditorGUILayout.PropertyField(fillNullRandomProperty,
					new GUIContent("Fill empties from random blocks"));
				for (int y = 0; y < gridSizeY.intValue; y++)
				{
					EditorGUILayout.BeginHorizontal();
					for (int x = 0; x < gridSizeX.intValue; x++)
					{

						var blockType =
							template.BlockTypes[LevelUtility.GetIndexOfPosition(new Vector2Int(x, y), gridSizeX.intValue)];
						var displayName = "None";

						if (blockType)
						{
							displayName = blockType.name;
							GUI.color = Color.gray;
						}
						else if (fillNullRandomProperty.boolValue)
						{
							displayName = "Random";
						}

						if (_selectedCell.x == x && _selectedCell.y == y)
						{
							GUI.color = Color.green;
						}

						if (GUILayout.Button(displayName, GUILayout.Width(64), GUILayout.Height(64)))
						{
							_selectedCell = new Vector2Int(x, y);
						}

						GUI.color = Color.white;
					}

					EditorGUILayout.EndHorizontal();
				}

				var index = LevelUtility.GetIndexOfPosition(_selectedCell, gridSizeX.intValue);
				var property = serializedObject.FindProperty("_blockTypes").GetArrayElementAtIndex(index);

				EditorGUILayout.PropertyField(property, new GUIContent("Selected Cell"));
			}


			serializedObject.ApplyModifiedProperties();
		}
	}
}