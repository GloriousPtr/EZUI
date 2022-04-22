using UnityEditor;
using UnityEngine;

namespace Plugins.EZUI.Scripts.Editor
{
	public class EditorMenu : MonoBehaviour
	{
		[MenuItem("GameObject/EZUI/EZUIManager", false, 1)]
		public static void AddEZUIManager(MenuCommand menuCommand)
		{
			SpawnObject("EZUI/Prefabs/EZUIManager");
		}
		
		[MenuItem("GameObject/EZUI/EZUIPanel", false, 1)]
		public static void AddEZUIPanel(MenuCommand menuCommand)
		{
			SpawnObject("EZUI/Prefabs/EZUIPanel");
		}
		
		[MenuItem("GameObject/EZUI/EZUIPopup", false, 1)]
		public static void AddEZUIPopup(MenuCommand menuCommand)
		{
			SpawnObject("EZUI/Prefabs/EZUIPopup");
		}
		
		[MenuItem("GameObject/EZUI/UI/Linear Progress Bar", false, 1)]
		public static void AddLinearProgressBar(MenuCommand menuCommand)
		{
			SpawnObject("EZUI/Prefabs/LinearProgressBar");
		}
		
		[MenuItem("GameObject/EZUI/UI/Radial Progress Bar", false, 1)]
		public static void AddRadialProgressBar(MenuCommand menuCommand)
		{
			SpawnObject("EZUI/Prefabs/RadialProgressBar");
		}

		private static void SpawnObject(string path)
		{
			Transform selected = Selection.activeTransform;
			GameObject prefab = Resources.Load<GameObject>(path);
			string name = prefab.name;
			prefab = selected != null
				? Instantiate(prefab, selected, false)
				: Instantiate(prefab);
			prefab.name = name;
			Selection.activeGameObject = prefab;
		}
	}
}