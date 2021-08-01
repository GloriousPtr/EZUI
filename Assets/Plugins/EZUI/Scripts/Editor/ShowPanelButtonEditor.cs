using EZUI;
using UnityEditor;

namespace EZUI_Editor
{
	[CustomEditor(typeof(ShowPanelButton))]
	public class ShowPanelButtonEditor : Editor
	{
		private static readonly string[] Type = {"Page", "Panel"};

		public SerializedProperty isPage;
		public SerializedProperty pageName;
		public SerializedProperty panelName;
		public SerializedProperty closePanel;

		private void OnEnable()
		{
			isPage = serializedObject.FindProperty("isPage");
			pageName = serializedObject.FindProperty("pageName");
			panelName = serializedObject.FindProperty("panelName");
			closePanel = serializedObject.FindProperty("closePanel");
		}

		/*
		public override void OnInspectorGUI()
		{
			serializedObject.UpdateIfRequiredOrScript();

			if (!EZUIManager.Instance)
			{
				EditorGUILayout.HelpBox("UIManager is missing", MessageType.Error, true);
				return;
			}

			int index = isPage.boolValue ? 0 : 1;
			index = EditorGUILayout.Popup("Type", index, Type);
			isPage.boolValue = index == 0;

			List<string> list = new List<string> {"-"};

			if (isPage.boolValue)
			{
				list.Add("<-");
				list.AddRange(EZUIManager.Instance.pages.Select(x => x.name).ToList());
				int selected = -1;
				selected = list.FindIndex(x => x == pageName.stringValue);
				if (selected == -1)
					selected = 0;

				selected = EditorGUILayout.Popup("Panel Group Name", selected, list.ToArray());
				pageName.stringValue = list[selected];
			}
			else
			{
				list.AddRange(EZUIManager.Instance.panels.Select(x => x.name).ToList());
				int selected = -1;
				selected = list.FindIndex(x => x == panelName.stringValue);
				if (selected == -1)
					selected = 0;

				selected = EditorGUILayout.Popup("Panel Name", selected, list.ToArray());
				
				panelName.stringValue = list[selected];
			}

			if (!isPage.boolValue)
				EditorGUILayout.PropertyField(closePanel);
			
			serializedObject.ApplyModifiedProperties();
		}
		
		*/
	}
}