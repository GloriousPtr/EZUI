using System.Collections.Generic;
using UnityEngine;

namespace EZUI
{
	[CreateAssetMenu(fileName = "EZUIData", menuName = "EZUI/EZUIData", order = 0)]
	public class EZUIData : ScriptableObject
	{
		[SerializeField] private List<Page> pages = new List<Page>();
		[SerializeField] private Page getDefaultPage;
		
		public List<string> panels = new List<string>();
		public Dictionary<string, Page> pagesDictionary = new Dictionary<string, Page>();
		
		public Page GetDefaultPage() => getDefaultPage;
		
		internal void Init()
		{
			for (int i = 0; i < pages.Count; i++)
			{
				Page page = pages[i];
				if (page.defaultPage)
					getDefaultPage = page;
				
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