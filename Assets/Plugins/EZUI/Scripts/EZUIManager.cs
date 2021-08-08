using UnityEngine;
using System.Collections.Generic;
using System;

namespace EZUI
{
	public class EZUIManager : MonoBehaviour
	{
		public static EZUIManager Instance { get; private set; }
		
		[SerializeField] private List<Transform> UImodules = new List<Transform>();
		
		private List<EZUIPanel> panels = new List<EZUIPanel>();
		private List<EZUIPopup> popups = new List<EZUIPopup>();
		private readonly Stack<EZUIPopup> popupStack = new Stack<EZUIPopup>();
		private readonly Stack<EZUIData.Page> pageStack = new Stack<EZUIData.Page>();
		
		public event Action<string> OnShowPage;
		public event Action ShouldQuit;
		
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
		
		private static EZUIData data;
		
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
			
			EZUIData.Page defaultPage = Data.GetDefaultPage();
			if (defaultPage != null)
			{
				HideAll(true);
				ShowPage(defaultPage, true);
			}
		}
		
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
				GoBack();
		}

		private void Init()
		{
			panels.Clear();
			panels.AddRange(GetComponentsInChildren<EZUIPanel>(true));

			foreach (Transform module in UImodules)
				panels.AddRange(module.GetComponentsInChildren<EZUIPanel>(true));

			if (Application.isEditor)
				panels.Sort((a, b) => { return string.Compare(a.name, b.name); });
			
			
			popups.Clear();
			popups.AddRange(GetComponentsInChildren<EZUIPopup>(true));

			foreach (Transform module in UImodules)
				popups.AddRange(module.GetComponentsInChildren<EZUIPopup>(true));

			if (Application.isEditor)
				popups.Sort((a, b) => { return string.Compare(a.name, b.name); });
		}

		private bool ShowPage(EZUIData.Page page, bool immediate = false, bool addToBackStack = true)
		{
			if (EZUIPanel.RunningAnimations > 0 || CurrentPage == page.name)
				return false;
			
			CurrentPage = page.name;

			foreach (EZUIPopup popup in popups)
				popup.SetVisible(false);
			
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
			
			popupStack.Clear();
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

		public void ShowPopup(string popupName)
		{
			EZUIPopup popup = popups.Find(x => x.key == popupName);
			
			if (popup == null)
				return;
			
			popupStack.Push(popup);
			popup.SetVisible(true);
		}
		
		public void HidePopup(string popupName)
		{
			EZUIPopup popup = popups.Find(x => x.key == popupName);
			
			if (popup == null)
				return;
			
			while (popupStack.Contains(popup))
				popupStack.Pop();
			
			popup.SetVisible(false);
		}
		
		public void HideAll(bool immediate, bool clearBackStack)
		{
			if (clearBackStack)
			{
				popupStack.Clear();
				pageStack.Clear();
			}
			
			HideAll(immediate);
		}
		
		public void HideAll(bool immediate)
		{
			foreach (EZUIPanel panel in panels)
				panel.SetVisible(false, immediate);
			
			foreach (EZUIPopup popup in popups)
				popup.SetVisible(false, immediate);
		}

		public void GoBack()
		{
			if (popupStack.Count > 0)
			{
				popupStack.Pop().SetVisible(false);
				return;
			}
			
			if (pageStack.Count <= 1)
			{
				ShouldQuit?.Invoke();
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
