using UnityEngine;
using UnityEditor;

namespace LudumDare50.EditorScripts { 
    [CustomEditor(typeof(EventSO), editorForChildClasses:true)]
    public class EventSOEditor : Editor {
        public override void OnInspectorGUI(){
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;
            if(GUILayout.Button("Force Raise")){
                (target as EventSO).Raise();
            }
        }
    }
}
