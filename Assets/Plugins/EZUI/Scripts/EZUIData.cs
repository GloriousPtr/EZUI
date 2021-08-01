using System.Collections.Generic;
using UnityEngine;

namespace EZUI
{
	[CreateAssetMenu(fileName = "EZUIData", menuName = "EZUI/EZUIData", order = 0)]
	public class EZUIData : ScriptableObject
	{
		public List<string> panels = new List<string>();
		public List<Page> pages = new List<Page>();
		
		[System.Serializable]
		public class Page
		{
			public string name;
			public List<string> showingPanels = new List<string>();
			public List<string> ignoringPanels = new List<string>();
			public bool defaultPage;
		}
	}
}