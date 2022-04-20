using TMPro;
using UnityEditor;
using UnityEngine;
using EZUI;

namespace EZUI_Editor
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(EZUIButton))]
	internal class EZUIButtonEditor : EZUIEditorBase
	{
		private static Color _defaultColor;
		
		private static AnimBoolGroup _onPointerEnterAnimBoolGroup;
		private static AnimBoolGroup _onPointerExitAnimBoolGroup;
		private static AnimBoolGroup _onPointerDownAnimBoolGroup;
		private static AnimBoolGroup _onPointerUpAnimBoolGroup;
		private static AnimBoolGroup _onClickAnimBoolGroup;

		private EZUIButton Target;
		private TextMeshProUGUI textMeshPro;
		
		private SerializedProperty onPointerEnter;
		private SerializedProperty onPointerExit;
		private SerializedProperty onPointerDown;
		private SerializedProperty onPointerUp;
		private SerializedProperty onClick;
		private SerializedProperty displayText;
		private SerializedProperty textLabel;

		protected override void OnEnable()
		{
			base.OnEnable();
			
			Target = (EZUIButton) target;
			Target.textLabel = Target.GetComponentInChildren<TextMeshProUGUI>();
			textMeshPro = Target.textLabel;
			
			_onPointerEnterAnimBoolGroup = new AnimBoolGroup(Repaint);
			_onPointerExitAnimBoolGroup = new AnimBoolGroup(Repaint);
			_onPointerDownAnimBoolGroup = new AnimBoolGroup(Repaint);
			_onPointerUpAnimBoolGroup = new AnimBoolGroup(Repaint);
			_onClickAnimBoolGroup = new AnimBoolGroup(Repaint);

			onPointerEnter = serializedObject.FindProperty("onPointerEnter");
			onPointerExit = serializedObject.FindProperty("onPointerExit");
			onPointerDown = serializedObject.FindProperty("onPointerDown");
			onPointerUp = serializedObject.FindProperty("onPointerUp");
			onClick = serializedObject.FindProperty("onClick");
			
			displayText = serializedObject.FindProperty("displayText");
			textLabel = serializedObject.FindProperty("textLabel");
		}
		
		public override void OnInspectorGUI()
		{
			serializedObject.UpdateIfRequiredOrScript();
			
			EZUIButton button = (EZUIButton) target;
			bool previewEnabled = targets.Length == 1;
			
			_defaultColor = GUI.backgroundColor;

			EditorGUILayout.BeginVertical("Box", GUILayout.Height(50));
			EditorGUILayout.LabelField(HeadingTemplate("EZUI Button", 35), HeadingStyle);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.Space(20);
			DrawAnimationData("On Pointer Enter", previewEnabled, button, onPointerEnter, _onPointerEnterAnimBoolGroup);
			DrawAnimationData("On Pointer Exit", previewEnabled, button, onPointerExit, _onPointerExitAnimBoolGroup);
			DrawAnimationData("On Pointer Down", previewEnabled, button, onPointerDown, _onPointerDownAnimBoolGroup);
			DrawAnimationData("On Pointer Up", previewEnabled, button, onPointerUp, _onPointerUpAnimBoolGroup);
			DrawAnimationData("On Click", previewEnabled, button, onClick, _onClickAnimBoolGroup);
			
			EditorGUILayout.Space(20);
			EditorGUILayout.LabelField(HeadingTemplate("Label"), HeadingStyle);
			EditorGUILayout.Space(10);
			EditorGUILayout.PropertyField(textLabel);
			if (textMeshPro)
			{
				displayText.stringValue = textMeshPro.text;
				displayText.stringValue = EditorGUILayout.TextArea(displayText.stringValue, GUILayout.Height(80));
				textMeshPro.text = displayText.stringValue;
			}
			
			GUI.backgroundColor = _defaultColor;
			
			serializedObject.ApplyModifiedProperties();
		}
	}
}