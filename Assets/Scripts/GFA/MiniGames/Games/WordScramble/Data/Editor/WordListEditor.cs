using System.Linq;
using UnityEditor;
using UnityEngine;

namespace GFA.MiniGames.Games.WordScramble.Data.Editor
{
    [CustomEditor(typeof(WordList))]
    public class WordListEditor : UnityEditor.Editor
    {
        private string _data;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            _data = EditorGUILayout.TextArea(_data);
            if (GUILayout.Button("Save"))
            {
                var processedData = _data.Split('\n').Where(x => x.Length > 3).ToArray();
                (target as WordList).SetWords(processedData);
                EditorUtility.SetDirty(target);
            }
        }
    }
}
