using EZUI;
using UnityEditor;
using UnityEngine;

namespace EZUI_Editor
{
	[CustomEditor(typeof(EZUIProgressor))]
	internal class EZUIProgressorEditor : Editor
	{
		private SerializedProperty _minValue;
		private SerializedProperty _maxValue;
		private SerializedProperty _value;
		private SerializedProperty _percent;

		private float _inspectorWidth;
		
		private void OnEnable()
		{
			_minValue = serializedObject.FindProperty("minValue");
			_maxValue = serializedObject.FindProperty("maxValue");
			_value = serializedObject.FindProperty("currentValue");
			_percent = serializedObject.FindProperty("currentPercent");
		}

		public override void OnInspectorGUI()
		{
			EZUIProgressor progressor = (EZUIProgressor) target;

			if (!progressor)
				return;
			
			serializedObject.UpdateIfRequiredOrScript();
			
			EditorUtils.InspectorWidth(_inspectorWidth, out _inspectorWidth);
			
			EditorGUILayout.Space(10);
			
			EditorUtils.DrawField(serializedObject, 0, "progressorTargets");
			
			EditorGUILayout.Space(10);
			
			EditorGUILayout.BeginHorizontal();
			{
				GUIStyle style = new GUIStyle
				{
					alignment = TextAnchor.MiddleCenter,
					fontStyle = FontStyle.Bold,
					normal = {textColor = Color.white},
				};
				
				EditorUtils.DrawField(serializedObject, 60, "minValue");
				EditorGUILayout.LabelField($"{progressor.CurrentValue} ({progressor.CurrentProgress} %)", style);
				EditorUtils.DrawField(serializedObject, 60, "maxValue");
			}
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.Space(20);
			
			const float barHeight = 20;
			GUILayoutOption barHeightOption = GUILayout.Height(barHeight);
			Color backgroundColor = EditorGUIUtility.isProSkin ? Color.black : Color.white;

			GUILayout.BeginVertical(barHeightOption);
			{
				GUI.color = new Color(backgroundColor.r, backgroundColor.g, backgroundColor.b, 0.2f);
				GUILayout.Label(GUIContent.none, new GUIStyle {normal = {background = EditorGUIUtility.whiteTexture}}, barHeightOption); //draw progress bar background
				GUILayout.Space(-barHeight);
				GUI.color = EZUIEditorBase.MoveColor;
				float progressBarWidth = _inspectorWidth * _percent.floatValue;
				GUILayout.Label(GUIContent.none, new GUIStyle {normal = {background = EditorGUIUtility.whiteTexture}}, barHeightOption, GUILayout.Width(progressBarWidth)); //draw progress bar (dynamic width)
				GUILayout.Space(-barHeight);
				GUI.color = EZUIEditorBase.DefaultColor;
			}
			GUILayout.EndVertical();
			
			EditorGUILayout.Space(10);
			
			float value = _value.floatValue;
			EditorGUI.BeginChangeCheck();
			value = EditorGUILayout.Slider(value, _minValue.floatValue, _maxValue.floatValue);
			if (EditorGUI.EndChangeCheck())
				EZUIPreviewHelper.ScrubProgressor(progressor, value);
			
			EditorGUILayout.Space(20);
			
			EditorUtils.BeginDrawFieldsHorizontal(serializedObject, true, 60, "duration", "ease");
			EditorUtils.DrawField(serializedObject, 0, "useWholeNumbers");
			
			EditorGUILayout.Space(20);
			
			if (GUILayout.Button("Simulate"))
				EZUIPreviewHelper.SimulateProgressor(progressor, _maxValue.floatValue);
			
			EditorGUILayout.Space(20);
			
			EditorUtils.DrawField(serializedObject, 0, "OnPercentChanged");
			EditorUtils.DrawField(serializedObject, 0, "OnValueChanged");
			
			serializedObject.ApplyModifiedProperties();
			Repaint();
		}
	}
}