using UnityEditor;
using UnityEngine;
using EZUI;

namespace EZUI_Editor
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(EZUIPopup))]
	internal class EZUIPopupEditor : EZUIEditorBase
	{
		private static Color _defaultColor;

		private static AnimBoolGroup _showAnimBoolsGroup;
		private static AnimBoolGroup _hideAnimBoolsGroup;

		private SerializedProperty key;
		private SerializedProperty showData;
		private SerializedProperty hideData;

		protected override void OnEnable()
		{
			base.OnEnable();
			
			_showAnimBoolsGroup = new AnimBoolGroup(Repaint);
			_hideAnimBoolsGroup = new AnimBoolGroup(Repaint);

			Init();
		}

		private void Init()
		{
			key = serializedObject.FindProperty("key");
			showData = serializedObject.FindProperty("showData");
			hideData = serializedObject.FindProperty("hideData");
		}
		
		public override void OnInspectorGUI()
		{
			serializedObject.UpdateIfRequiredOrScript();
			
			EZUIPopup popup = (EZUIPopup) target;
			bool previewEnabled = targets.Length == 1;
			_defaultColor = GUI.backgroundColor;
			
			EditorGUILayout.BeginVertical("Box", GUILayout.Height(50));
			EditorGUILayout.LabelField(HeadingTemplate("EZUI Popup", 35), HeadingStyle);
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.Space(20);
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Key", GUILayout.Width(135));
			EditorGUILayout.PropertyField(key, GUIContent.none);
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.Space(20);
			
			DrawAnimationData("Show Panel", previewEnabled, popup, showData, _showAnimBoolsGroup);
			DrawAnimationData("Hide Panel", previewEnabled, popup, hideData, _hideAnimBoolsGroup);
			
			GUI.backgroundColor = _defaultColor;
			
			serializedObject.ApplyModifiedProperties();
		}
	}
}
