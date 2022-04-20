using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using EZUI.Animation;
using EZUI;

namespace EZUI_Editor
{
	public class EZUIEditorBase : Editor
	{
		public static readonly Color DefaultColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
		
		public static readonly Color ExpandColor = new Color(0.3f, 0.3f, 0.65f, 1.0f);
		public static readonly Color MoveColor = new Color(0.26f, 0.74f, 0.19f, 1.0f);
		public static readonly Color RotateColor = new Color(0.75f, 0.35f, 0.1f, 1.0f);
		public static readonly Color ScaleColor = new Color(1.0f, 0.0f, 0.0f, 1.0f);
		public static readonly Color FadeColor = new Color(0.5f, 0.15f, 0.5f, 1.0f);

		public static GUIStyle HeadingStyle;

		public const float RepaintTime = 0.016f;
		public static float NextRepaintTime;
		
		public static string HeadingTemplate(string label, int size = 20)
		{
			string color = EditorGUIUtility.isProSkin ? "#cccccc" : "#000000";
			return $"<size={size}><color={color}><b>{label}</b></color></size>";
		}

		public static Color GetColor(Color color)
		{
			color.a = EditorGUIUtility.isProSkin ? color.a : color.a * 0.5f;
			return color;
		}

		protected virtual void OnEnable()
		{
			HeadingStyle = new GUIStyle() { richText = true };
		}

		public void DrawAnimationData(string label, bool previewEnabled, EZUIBase ezuiBase, SerializedProperty serializedProperty, AnimBoolGroup animBoolsGroup)
		{
			SerializedProperty expandedProperty = serializedProperty.FindPropertyRelative("expanded");
			SerializedProperty isAnyAnimationActive = serializedProperty.FindPropertyRelative("isAnyAnimationActive");
			
			GUI.backgroundColor = isAnyAnimationActive.boolValue ? ExpandColor : DefaultColor;
			if (GUILayout.Button(label, EZUISkin.FoldoutButton))
				expandedProperty.boolValue = !expandedProperty.boolValue;
			GUI.backgroundColor = DefaultColor;
			
			animBoolsGroup.expanded.target = expandedProperty.boolValue;

			if (EditorGUILayout.BeginFadeGroup(animBoolsGroup.expanded.faded))
			{
				using (new EditorGUILayout.VerticalScope(EZUISkin.Box))
				{
					EditorGUILayout.BeginHorizontal();
					if (GUILayout.Button("Animations", animBoolsGroup.events.value ? EZUISkin.TabButtonDefault : EZUISkin.TabButtonSelected))
						animBoolsGroup.events.target = false;
					if (GUILayout.Button("Events", !animBoolsGroup.events.value ? EZUISkin.TabButtonDefault : EZUISkin.TabButtonSelected))
						animBoolsGroup.events.target = true;
					EditorGUILayout.EndHorizontal();
					
					EditorGUILayout.Space();

					if (animBoolsGroup.events.target)
					{
						EditorUtils.BeginDrawFieldsVertical(serializedProperty, true, 0, "onStarted", "onCompleted");
					}
					else
					{
						if (previewEnabled)
							previewEnabled = !Application.isPlaying;
						GUI.enabled = previewEnabled;
						if (GUILayout.Button("Preview", EZUISkin.MediumButtonDefault))
							EZUIPreviewHelper.Preview(ezuiBase, (AnimationData) EditorUtils.GetTargetObjectOfProperty(serializedProperty));
						if (!previewEnabled)
							GUI.enabled = true;
							
						EditorGUILayout.Space();
							
						DrawData(serializedProperty, animBoolsGroup);
					}
				}
			}
			EditorGUILayout.EndFadeGroup();
			
			EditorGUILayout.Space();

			if (EditorApplication.timeSinceStartup > NextRepaintTime)
			{
				NextRepaintTime = (float) EditorApplication.timeSinceStartup + RepaintTime;
				Repaint();
			}
		}

		private static void DrawData(SerializedProperty animationData, AnimBoolGroup animBoolsGroup)
		{
			SerializedProperty defaultStateProperty = animationData.FindPropertyRelative("defaultState");
			
			SerializedProperty moveProperty = animationData.FindPropertyRelative("move");
			SerializedProperty rotationProperty = animationData.FindPropertyRelative("rotation");
			SerializedProperty scaleProperty = animationData.FindPropertyRelative("scale");
			SerializedProperty fadeProperty = animationData.FindPropertyRelative("fade");
			
			State defaultState = (State) defaultStateProperty.intValue;
			string defaultStateString = defaultState != State.None ? defaultState.ToString() : string.Empty;

			EditorGUILayout.BeginHorizontal();
			DrawToggleButton(moveProperty, $"Move {defaultStateString}", GetColor(MoveColor));
			DrawToggleButton(rotationProperty, $"Rotate {defaultStateString}", GetColor(RotateColor));
			DrawToggleButton(scaleProperty, $"Scale {defaultStateString}", GetColor(ScaleColor));
			DrawToggleButton(fadeProperty, $"Fade {defaultStateString}", GetColor(FadeColor));
			EditorGUILayout.EndHorizontal();
			
			animationData.FindPropertyRelative("isAnyAnimationActive").boolValue = 
				moveProperty.boolValue || rotationProperty.boolValue ||
				scaleProperty.boolValue || fadeProperty.boolValue;

			DrawButtonFadeGroup(moveProperty, animBoolsGroup.move, () =>
			{
				DrawMoveData(defaultState, animationData.FindPropertyRelative("moveData"), MoveColor);
			});
			
			DrawButtonFadeGroup(rotationProperty, animBoolsGroup.rotate, () =>
			{
				DrawRotationData(defaultState, animationData.FindPropertyRelative("rotationData"), RotateColor);
			});
			
			DrawButtonFadeGroup(scaleProperty, animBoolsGroup.scale, () =>
			{
				DrawScaleData(defaultState, animationData.FindPropertyRelative("scaleData"), ScaleColor);
			});
			
			DrawButtonFadeGroup(fadeProperty, animBoolsGroup.fade, () =>
			{
				DrawFloatData(defaultState, animationData.FindPropertyRelative("fadeData"), FadeColor);
			});
		}
		
		private static void DrawButtonFadeGroup(SerializedProperty boolProperty, AnimBool animBool, Action onExpanded)
		{
			animBool.target = boolProperty.boolValue;
			if (EditorGUILayout.BeginFadeGroup(animBool.faded))
				onExpanded?.Invoke();
			EditorGUILayout.EndFadeGroup();
		}
		
		private static void DrawFloatData(State defaultState, SerializedProperty floatData, Color boxColor)
		{
			SerializedProperty stateProperty = floatData.FindPropertyRelative("state");
			
			GUI.backgroundColor = GetColor(boxColor);
			EditorGUILayout.BeginVertical("Box");
			{
				EditorUtils.BeginDrawFieldsHorizontal(floatData, false, 70, "startDelay", "duration");
				{
					if (defaultState != State.None)
					{
						EditorGUILayout.Space();
						EditorGUILayout.LabelField("Custom From & To", GUILayout.Width(120));
						bool stateChanged = EditorGUILayout.Toggle((State) stateProperty.intValue != defaultState);
						stateProperty.intValue = (int) (stateChanged ? State.None : defaultState);
					}
				}
				EditorUtils.EndDrawFieldsHorizontal();

				EditorGUILayout.Space();

				switch ((State) stateProperty.intValue)
				{
					case State.None:
						EditorUtils.BeginDrawFieldsHorizontal(floatData, true, 70, "from", "to");
						break;
					case State.In:
						EditorUtils.DrawField(floatData, 0, "from");
						break;
					case State.Out:
						EditorUtils.DrawField(floatData, 0, "to");
						break;
				}

				EditorUtils.DrawField(floatData, 0, "ease");
			}
			EditorGUILayout.EndVertical();
			GUI.backgroundColor = DefaultColor;
		}

		private static void DrawMoveData(State defaultState, SerializedProperty moveData, Color boxColor)
		{
			SerializedProperty stateProperty = moveData.FindPropertyRelative("state");
			SerializedProperty modeProperty = moveData.FindPropertyRelative("mode");
			SerializedProperty moveDirectionProperty = moveData.FindPropertyRelative("moveDirection");

			GUI.backgroundColor = GetColor(boxColor);
			EditorGUILayout.BeginVertical("Box");
			{
				EditorUtils.DrawField(moveData, 0, "moveMode");
				
				EditorUtils.BeginDrawFieldsHorizontal(moveData, false, 70, "startDelay", "duration");
				{
					if (defaultState != State.None)
					{
						EditorGUILayout.Space();
						EditorGUILayout.LabelField("Custom From & To", GUILayout.Width(120));
						bool stateChanged = EditorGUILayout.Toggle((State) stateProperty.intValue != defaultState);
						stateProperty.intValue = (int) (stateChanged ? State.None : defaultState);
					}
					EditorUtils.EndDrawFieldsHorizontal();
				}

				EditorGUILayout.Space();
				
				if (stateProperty.intValue == (int) State.None)
					EditorUtils.DrawField(moveData, 0, "mode");

				bool customPosition = (MoveDirection) moveDirectionProperty.intValue == MoveDirection.CustomPosition;
				switch ((State) stateProperty.intValue)
				{
					case State.None:
						switch ((Mode) modeProperty.intValue)
						{
							case Mode.None:
								EditorUtils.BeginDrawFieldsVertical(moveData, true, 0, "from", "to");
								break;
							case Mode.By:
								EditorUtils.DrawField(moveData, 0, "by");
								break;
							case Mode.Punch:
								EditorUtils.DrawField(moveData, 0, "by");
								EditorUtils.BeginDrawFieldsHorizontal(moveData, true, 70, "vibrato", "elasticity");
								break;
						}
						break;
					case State.In:
						EditorUtils.DrawField(moveData, 0, "moveDirection", "From");
						if (customPosition)
							EditorUtils.DrawField(moveData, 0, "from", "Custom");
						break;
					case State.Out:
						EditorUtils.DrawField(moveData, 0, "moveDirection", "To");
						if (customPosition)
							EditorUtils.DrawField(moveData, 0, "to", "Custom");
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
				
				EditorUtils.DrawField(moveData, 0, "ease");
			}
			EditorGUILayout.EndVertical();
			
			GUI.backgroundColor = DefaultColor;
		}
		
		private static void DrawRotationData(State defaultState, SerializedProperty rotationData, Color boxColor)
		{
			SerializedProperty stateProperty = rotationData.FindPropertyRelative("state");
			SerializedProperty modeProperty = rotationData.FindPropertyRelative("mode");

			GUI.backgroundColor = GetColor(boxColor);
			EditorGUILayout.BeginVertical("Box");
			{
				EditorUtils.BeginDrawFieldsHorizontal(rotationData, false, 70, "startDelay", "duration");
				{
					if (defaultState != State.None)
					{
						EditorGUILayout.Space();
						EditorGUILayout.LabelField("Custom From & To", GUILayout.Width(120));
						bool stateChanged = EditorGUILayout.Toggle((State) stateProperty.intValue != defaultState);
						stateProperty.intValue = (int) (stateChanged ? State.None : defaultState);
					}
				}
				EditorUtils.EndDrawFieldsHorizontal();
				
				EditorGUILayout.Space();
				
				if (stateProperty.intValue == (int) State.None)
					EditorUtils.DrawField(rotationData, 0, "mode");
				
				switch ((State) stateProperty.intValue)
				{
					case State.None:
						switch ((Mode) modeProperty.intValue)
						{
							case Mode.None:
								EditorUtils.BeginDrawFieldsVertical(rotationData, true, 0, "from", "to");
								break;
							case Mode.By:
								EditorUtils.DrawField(rotationData, 0, "by");
								break;
							case Mode.Punch:
								EditorUtils.DrawField(rotationData, 0, "by");
								EditorUtils.BeginDrawFieldsHorizontal(rotationData, true, 70, "vibrato", "elasticity");
								break;
						}
						break;
					case State.In:
						EditorUtils.DrawField(rotationData, 0, "from");
						break;
					case State.Out:
						EditorUtils.DrawField(rotationData, 0, "to");
						break;
				}

				EditorUtils.BeginDrawFieldsHorizontal(rotationData, true, 75, "ease", "rotateMode");
			}
			EditorGUILayout.EndVertical();
			
			GUI.backgroundColor = DefaultColor;
		}
		
		private static void DrawScaleData(State defaultState, SerializedProperty scaleData, Color boxColor)
		{
			SerializedProperty stateProperty = scaleData.FindPropertyRelative("state");
			SerializedProperty modeProperty = scaleData.FindPropertyRelative("mode");

			GUI.backgroundColor = GetColor(boxColor);
			EditorGUILayout.BeginVertical("Box");
			{
				EditorUtils.BeginDrawFieldsHorizontal(scaleData, false, 70, "startDelay", "duration");
				{
					if (defaultState != State.None)
					{
						EditorGUILayout.Space();
						EditorGUILayout.LabelField("Custom From & To", GUILayout.Width(120));
						bool stateChanged = EditorGUILayout.Toggle((State) stateProperty.intValue != defaultState);
						stateProperty.intValue = (int) (stateChanged ? State.None : defaultState);
					}
				}
				EditorUtils.EndDrawFieldsHorizontal();

				EditorGUILayout.Space();

				if (stateProperty.intValue == (int) State.None)
					EditorUtils.DrawField(scaleData, 0, "mode");
				
				switch ((State) stateProperty.intValue)
				{
					case State.None:
						switch ((Mode) modeProperty.intValue)
						{
							case Mode.None:
								EditorUtils.BeginDrawFieldsVertical(scaleData, true, 0, "from", "to");
								break;
							case Mode.By:
								EditorUtils.DrawField(scaleData, 0, "by");
								break;
							case Mode.Punch:
								EditorUtils.DrawField(scaleData, 0, "by");
								EditorUtils.BeginDrawFieldsHorizontal(scaleData, true, 70, "vibrato", "elasticity");
								break;
						}
						break;
					case State.In:
						EditorUtils.DrawField(scaleData, 0, "from");
						break;
					case State.Out:
						EditorUtils.DrawField(scaleData, 0, "to");
						break;
				}

				EditorUtils.DrawField(scaleData,0, "ease");
			}
			EditorGUILayout.EndVertical();
			
			GUI.backgroundColor = DefaultColor;
		}
		
		private static void DrawToggleButton(SerializedProperty serializedProperty, string customName, Color activeColor)
		{
			if (string.IsNullOrEmpty(customName))
				customName = serializedProperty.displayName;
			
			GUI.backgroundColor = serializedProperty.boolValue ? activeColor : DefaultColor;
			if (GUILayout.Button(customName, serializedProperty.boolValue ? EZUISkin.LargeButtonSelected : EZUISkin.LargeButtonDefault))
				serializedProperty.boolValue = !serializedProperty.boolValue;
		}
	}
}