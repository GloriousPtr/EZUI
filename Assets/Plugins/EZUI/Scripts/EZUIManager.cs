using UnityEngine;
using System.Collections.Generic;
using System;
using DG.Tweening;

namespace EZUI
{
	public class EZUIManager : MonoBehaviour
	{
		public static EZUIManager Instance { get; private set; }
		
		[SerializeField] private List<Transform> UImodules = new List<Transform>();
		
		private List<EZUIPanel> panels = new List<EZUIPanel>();
		private readonly Stack<EZUIData.Page> pageStack = new Stack<EZUIData.Page>();
		
		public string CurrentPage { get; private set; }

		public static EZUIData Data
		{
			get
			{
				if (data != null)
					return data;
				
				data = (EZUIData) Resources.Load("EZUI/Settings/EZUIData");
				if (data == null)
					Debug.LogError("EZUI data not found at: EZUI/Settings/EZUIData");
				
				return data;
			}
		}
		
		public static event Action<string> OnShowPage;

		private static EZUIData data;
		
		public EZUIData.Page GetDefaultPage() => Data.pages.Find(x => x.defaultPage);
		
		private void Awake()
		{
			if (Instance != null)
			{
				Debug.LogError("An instance of EZUIManager already exists, Destroying!");
				Destroy(this);
				return;
			}
			
			Instance = this;
		}
		
		private void Start()
		{
			Init();
			
			EZUIData.Page defaultPage = GetDefaultPage();
			if (defaultPage != null)
				ShowPage(defaultPage, true);
		}
		
		private void Init()
		{
			panels.Clear();
			panels.AddRange(GetComponentsInChildren<EZUIPanel>(true));

			foreach (Transform module in UImodules)
				panels.AddRange(module.GetComponentsInChildren<EZUIPanel>(true));

			if (Application.isEditor)
				panels.Sort((a, b) => { return string.Compare(a.name, b.name); });
		}

		private bool ShowPage(EZUIData.Page page, bool immediate = false, bool addToBackStack = true)
		{
			if (EZUIPanel.RunningAnimations > 0 || CurrentPage == page.name)
				return false;
			
			CurrentPage = page.name;
			
			foreach (EZUIPanel panel in panels)
			{
				if (page.showingPanels.Contains(panel.key))
					panel.SetVisible(true, immediate);
				else if (!page.ignoringPanels.Contains(panel.key))
					panel.SetVisible(false, immediate);
			}
			
			OnShowPage?.Invoke(page.name);
			
			if (!addToBackStack)
				return true;
			
			while (pageStack.Contains(page))
				pageStack.Pop();
			
			pageStack.Push(page);
			
			// PrettyPrintStack();
			
			return true;
		}

		public void ShowPage(string pageName)
		{
			ShowPage(pageName, false);
		}

		public void ShowPage(string pageName, bool immediate)
		{
			EZUIData.Page page = Data.pages.Find(x => x.name == pageName);
			if (page != null)
				ShowPage(page, immediate);
		}

		public void HideAll(bool clearBackStack = true)
		{
			foreach (EZUIPanel panel in panels)
				panel.SetVisible(false);
			
			pageStack.Clear();
		}

		public void ShowPreviousPage()
		{
			if (pageStack.Count < 2)
			{
				Debug.LogError("Can't go back, only page left is: " + pageStack.Peek().name);
				return;
			}

			EZUIData.Page poppedPage = pageStack.Pop();
			bool shown = ShowPage(pageStack.Peek(), false, false);
			if (!shown)
				pageStack.Push(poppedPage);
		}

		private void PrettyPrintStack()
		{
			string s = "Stack:\n";
			foreach (EZUIData.Page page in pageStack)
				s += $"\t{page.name}\n";
			Debug.LogError(s);
		}
	}
}
