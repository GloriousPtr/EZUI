using System.Collections.Generic;
using EZUI;
using EZUI_Editor;
using UnityEditor;
using UnityEngine;

namespace EZUIEditor
{
	[CustomEditor(typeof(EZUIData))]
	internal class EZUIDataEditor : Editor
	{
		private SerializedProperty pagesProperty;
		private EZUIData.Page edit;
		
		private void OnEnable()
		{
			pagesProperty = serializedObject.FindProperty("pages");
		}
		
		public override void OnInspectorGUI()
		{
			EZUIData data = (EZUIData) target;

			EditorGUILayout.LabelField("Panels");
			DrawTextFields(data.panels, out bool duplicatesExist);
			
			EditorGUILayout.Space(20);
			
			#region Pages

			GUILayout.Space(20);
			GUILayout.Label("Panel Groups", GUILayout.ExpandWidth(true));

			if (duplicatesExist)
			{
				EditorGUILayout.HelpBox("Duplicates exists in panels, remove the duplicates or change the key",
					MessageType.Error, true);
				return;
			}

			EditorGUILayout.BeginVertical();

			EditorGUILayout.BeginHorizontal();

			GUILayout.Space(10);
			GUILayout.Label("Edit", EditorStyles.centeredGreyMiniLabel, GUILayout.Width(35));
			GUILayout.Label("Name", EditorStyles.centeredGreyMiniLabel, GUILayout.Width(200));

			EditorGUILayout.EndHorizontal();
			
			List<EZUIData.Page> pages = (List<EZUIData.Page>) EditorUtils.GetTargetObjectOfProperty(pagesProperty);
			bool foundDuplicate = false;
			for (int i = 0; i < pages.Count; i++)
			{
				EZUIData.Page page = pages[i];
				EditorGUILayout.BeginHorizontal();
				if (GUILayout.Button("X", GUILayout.Width(20)))
				{
					pages.Remove(page);
					break;
				}

				List<EZUIData.Page> duplicateKeys = pages.FindAll(x => x.key == page.key);
				duplicatesExist = duplicateKeys.Count > 1;

				GUI.enabled = !duplicatesExist;
				if (GUILayout.Button("Edit", GUILayout.Width(50)))
				{
					if (edit == page)
						edit = null;
					else
						edit = page;
				}

				GUI.enabled = true;

				if (duplicatesExist)
				{
					foundDuplicate = true;
					GUI.color = Color.red;
				}
				page.key = EditorGUILayout.TextField(page.key, GUILayout.Width(200));
				GUI.color = EZUIEditorBase.DefaultColor;

				GUI.enabled = !duplicatesExist;
				EZUIData.Page defaultGroup = pages.Find(x => x.defaultPage);

				if (defaultGroup == null)
				{
					defaultGroup = page;
					page.defaultPage = true;
				}

				if (page.defaultPage && defaultGroup != page)
					page.defaultPage = false;

				if (page.defaultPage)
				{
					bool active = GUI.enabled;
					GUI.enabled = false;
					GUI.color = Color.green;
					GUILayout.Button("DEFAULT", EditorStyles.miniButton, GUILayout.Width(100));
					GUI.color = EZUIEditorBase.DefaultColor;
					GUI.enabled = active;
				}
				else if (GUILayout.Button("Make default", EditorStyles.miniButton, GUILayout.Width(100)))
				{
					defaultGroup.defaultPage = false;
					defaultGroup = page;
					page.defaultPage = true;
				}

				GUI.enabled = true;

				EditorGUILayout.EndHorizontal();

				if (edit == page)
				{
					EditorGUILayout.BeginHorizontal();
					GUILayout.Space(40);
					EditorGUILayout.BeginVertical(EditorStyles.textArea, GUILayout.Width(440));

					EditorGUILayout.Space();

					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.Space();
					GUILayout.Label("Name", EditorStyles.boldLabel, GUILayout.Width(150));
					GUILayout.Label("Show", EditorStyles.boldLabel, GUILayout.Width(90));
					GUILayout.Label("Ignore", EditorStyles.boldLabel, GUILayout.Width(90));
					GUILayout.Label("Hide", EditorStyles.boldLabel, GUILayout.Width(90));
					EditorGUILayout.EndHorizontal();

					EditorGUILayout.Space();

					EditorGUILayout.BeginVertical();
					Dictionary<string, int> mask = new Dictionary<string, int>();

					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.Space();

					EditorGUILayout.LabelField("All", GUILayout.Width(150));
					if (GUILayout.Button("Show All", GUILayout.Width(90)))
					{
						page.showingPanels.Clear();
						page.ignoringPanels.Clear();
						foreach (string panel in data.panels)
							page.showingPanels.Add(panel);
					}

					if (GUILayout.Button("Ignore All", GUILayout.Width(90)))
					{
						page.showingPanels.Clear();
						page.ignoringPanels.Clear();
						foreach (string panel in data.panels)
							page.ignoringPanels.Add(panel);
					}

					if (GUILayout.Button("Hide All", GUILayout.Width(90)))
					{
						page.showingPanels.Clear();
						page.ignoringPanels.Clear();
					}

					EditorGUILayout.EndHorizontal();

					foreach (string panel in data.panels)
					{
						if (string.IsNullOrEmpty(panel))
							continue;

						if (mask.ContainsKey(panel))
							continue;

						mask.Add(panel, -1);
						if (page.showingPanels.Contains(panel))
							mask[panel] = 1;
						else if (page.ignoringPanels.Contains(panel))
							mask[panel] = 0;
					}

					foreach (string panel in data.panels)
					{
						if (string.IsNullOrEmpty(panel))
							continue;

						EditorGUILayout.BeginHorizontal();
						switch (mask[panel])
						{
							case -1:
								GUI.color = Color.red;
								break;
							case 0:
								GUI.color = Color.yellow;
								break;
							case 1:
								GUI.color = Color.green;
								break;
						}

						EditorGUILayout.Space();

						EditorGUILayout.LabelField(panel, GUILayout.Width(150));
						GUI.color = EZUIEditorBase.DefaultColor;

						if (EditorGUILayout.Toggle(mask[panel] == 1, GUILayout.Width(90)))
							mask[panel] = 1;
						if (EditorGUILayout.Toggle(mask[panel] == 0, GUILayout.Width(90)))
							mask[panel] = 0;
						if (EditorGUILayout.Toggle(mask[panel] == -1, GUILayout.Width(90)))
							mask[panel] = -1;
						EditorGUILayout.EndHorizontal();
					}

					EditorGUILayout.EndVertical();

					page.showingPanels.Clear();
					page.ignoringPanels.Clear();
					foreach (KeyValuePair<string, int> pair in mask)
					{
						if (pair.Value == 1)
							page.showingPanels.Add(pair.Key);
						else if (pair.Value == 0)
							page.ignoringPanels.Add(pair.Key);
					}

					EditorGUILayout.Space(20);

					EditorGUILayout.EndVertical();

					EditorGUILayout.EndHorizontal();
				}
			}

			GUI.enabled = !foundDuplicate;
			if (GUILayout.Button("Add", GUILayout.Width(60)))
				pages.Add(new EZUIData.Page());
			GUI.enabled = true;

			EditorGUILayout.EndVertical();

			if (foundDuplicate)
			{
				EditorGUILayout.HelpBox("Duplicates exists in pages, remove the duplicates or change the key",
					MessageType.Error, true);
				return;
			}
			
			#endregion
			
			serializedObject.ApplyModifiedProperties();

			EditorGUILayout.Space(20);
			
			if (GUILayout.Button("Save"))
			{
				EditorUtility.SetDirty(data);
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
			}
		}

		private static void DrawTextFields(List<string> list, out bool duplicatesExist)
		{
			duplicatesExist = false;
			for (int i = 0; i < list.Count; i++)
			{
				EditorGUILayout.BeginHorizontal();
				if (GUILayout.Button("x", GUILayout.Width(20)))
				{
					list.RemoveAt(i);
					break;
				}

				var duplicates = list.FindAll(x => x == list[i]);

				if (duplicates.Count > 1)
				{
					duplicatesExist = true;
					GUI.color = Color.red;
				}
				
				list[i] = EditorGUILayout.TextField(list[i]);
				
				GUI.color = EZUIEditorBase.DefaultColor;
				EditorGUILayout.EndHorizontal();
			}

			GUI.enabled = !list.Contains("") && !duplicatesExist;
			if (GUILayout.Button("Add", GUILayout.Width(50)))
				list.Add("");
			GUI.enabled = true;
		}
	}
}
