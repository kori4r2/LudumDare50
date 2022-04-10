using UnityEditor;
using UnityEngine;

namespace LudumDare50.EditorScripts {
    [CustomEditor(typeof(StarEvent), editorForChildClasses: true)]
    public class StarEventEditor : Editor {
        private Star debugValue = null;
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            if (Application.isPlaying) {
                debugValue = EditorGUILayout.ObjectField(debugValue, typeof(Debug), true) as Star;
            } else {
                debugValue = null;
            }

            GUI.enabled = Application.isPlaying;
            if (GUILayout.Button("Force Raise")) {
                (target as StarEvent).Raise(debugValue);
            }
        }
    }
}
