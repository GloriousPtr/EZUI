using System.Collections.Generic;
using UnityEngine;
using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo(EZUI.StaticData.EZUI_EditorAssemblyName)]
namespace EZUI
{
	[CreateAssetMenu(fileName = "EZUIData", menuName = "EZUI/EZUIData", order = 0)]
	public class EZUIData : ScriptableObject
	{
		[SerializeField] private List<Page> pages = new List<Page>();
		[SerializeField] private Page defaultPage;
		
		[SerializeField] internal List<string> panels = new List<string>();
		
		internal Dictionary<string, Page> pagesDictionary = new Dictionary<string, Page>();
		
		public Page GetDefaultPage() => defaultPage;
		
		internal void Init()
		{
			for (int i = 0; i < pages.Count; i++)
			{
				Page page = pages[i];
				if (page.defaultPage)
					defaultPage = page;
				
				pagesDictionary.Add(page.key, page);
			}
		}
		
		[System.Serializable]
		public class Page
		{
			public string key;
			public List<string> showingPanels = new List<string>();
			public List<string> ignoringPanels = new List<string>();
			public bool defaultPage;
		}
	}
}