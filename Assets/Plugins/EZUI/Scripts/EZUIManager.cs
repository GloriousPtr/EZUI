using UnityEngine;
using System.Collections.Generic;
using System;

namespace EZUI
{
	public class EZUIManager : MonoBehaviour
	{
		public static EZUIManager Instance { get; private set; }
		
		[SerializeField] private bool debug;
		[SerializeField] private List<Transform> UImodules = new List<Transform>();
		
		private List<EZUIPanel> panels = new List<EZUIPanel>();
		private List<EZUIPopup> popups = new List<EZUIPopup>();
		private Dictionary<string, EZUIPopup> popupsDictionary = new Dictionary<string, EZUIPopup>();
		private readonly Stack<EZUIPopup> popupStack = new Stack<EZUIPopup>();
		private readonly Stack<EZUIData.Page> pageStack = new Stack<EZUIData.Page>();
		
		public event Action<string> OnShowPage;
		public event Action<string> OnShowPopup;
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
			Data.Init();
			
			panels.Clear();
			panels.AddRange(GetComponentsInChildren<EZUIPanel>(true));

			for (int i = 0; i < UImodules.Count; i++)
				panels.AddRange(UImodules[i].GetComponentsInChildren<EZUIPanel>(true));
			
			popups.Clear();
			popups.AddRange(GetComponentsInChildren<EZUIPopup>(true));

			for (int i = 0; i < UImodules.Count; i++)
				popups.AddRange(UImodules[i].GetComponentsInChildren<EZUIPopup>(true));

			for (int i = 0; i < popups.Count; i++)
			{
				EZUIPopup popup = popups[i];
				popupsDictionary.Add(popup.key, popup);
			}
		}

		public void ShowPage(string key)
		{
			ShowPage(key, false);
		}

		public void ShowPage(string key, bool immediate)
		{
			if (debug)
			{
				if (!Data.pagesDictionary.ContainsKey(key))
				{
					Debug.LogError($"Page not found with key: {key}");
					return;
				}
			}
			
			ShowPage(Data.pagesDictionary[key], immediate);
		}
		
		private bool ShowPage(EZUIData.Page page, bool immediate, bool addToBackStack = true)
		{
			if (debug)
			{
				if (page == null)
				{
					Debug.LogError("Page is null!");
					return false;
				}
			}
			
			if (EZUIPanel.RunningAnimations > 0 || CurrentPage == page.key)
				return false;
			
			CurrentPage = page.key;

			for (int i = 0; i < popups.Count; i++)
				popups[i].SetVisible(false);

			for (int i = 0; i < panels.Count; i++)
			{
				EZUIPanel panel = panels[i];
				if (page.showingPanels.Contains(panel.key))
					panel.SetVisible(true, immediate);
				else if (!page.ignoringPanels.Contains(panel.key))
					panel.SetVisible(false, immediate);
			}

			OnShowPage?.Invoke(page.key);
			
			if (!addToBackStack)
				return true;
			
			popupStack.Clear();
			while (pageStack.Contains(page))
				pageStack.Pop();
			
			pageStack.Push(page);
			
			if (debug)
				PrettyPrintStack();
			
			return true;
		}

		public void ShowPopup(string key)
		{
			ShowPopup(key, false);
		}
		
		public void ShowPopup(string key, bool immediate)
		{
			if (debug)
			{
				if (!popupsDictionary.ContainsKey(key))
				{
					Debug.LogError($"Popup not found with key: {key}");
					return;
				}
			}
			
			ShowPopup(popupsDictionary[key], immediate);
		}
		
		public void ShowPopup(EZUIPopup popup, bool immediate)
		{
			if (popup == null)
				return;
			
			popupStack.Push(popup);
			popup.SetVisible(true, immediate);
			OnShowPopup?.Invoke(popup.key);
			
			if (debug)
				PrettyPrintStack();
		}
		
		public void HidePopup(string key)
		{
			HidePopup(key, false);
		}
		
		public void HidePopup(string key, bool immediate)
		{
			if (debug)
			{
				if (!popupsDictionary.ContainsKey(key))
				{
					Debug.LogError($"Popup not found with key: {key}");
					return;
				}
			}

			HidePopup(popupsDictionary[key], immediate);
		}
		
		public void HidePopup(EZUIPopup popup, bool immediate)
		{
			if (popup == null)
				return;
			
			while (popupStack.Contains(popup))
				popupStack.Pop();
			popup.SetVisible(false, immediate);
			
			if (debug)
				PrettyPrintStack();
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
			if (!debug)
				return;
			
			string stack = "Popup Stack:\n";
			foreach (EZUIPopup popup in popupStack)
				stack += $"\t{popup.key}\n";
			
			stack = "\nPage Stack:\n";
			foreach (EZUIData.Page page in pageStack)
				stack += $"\t{page.key}\n";
			
			Debug.Log(stack);
		}
	}
}
