using UnityEngine;
using System.Collections.Generic;
using System;
using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo(EZUI.StaticData.EZUI_EditorAssemblyName)]
namespace EZUI
{
	public class EZUIManager : MonoBehaviour
	{
		/// <summary>
		/// Singleton Instance for EZUIManager
		/// </summary>
		public static EZUIManager Instance { get; private set; }
		
		[SerializeField] private bool debug;
		[SerializeField] internal EZUIData data;
		[SerializeField] private List<Transform> UImodules = new List<Transform>();
		
		private readonly List<EZUIPanel> panels = new List<EZUIPanel>();
		private readonly List<EZUIPopup> popups = new List<EZUIPopup>();
		private readonly Dictionary<string, EZUIPopup> popupsDictionary = new Dictionary<string, EZUIPopup>();
		private readonly Stack<EZUIPopup> popupStack = new Stack<EZUIPopup>();
		private readonly Stack<EZUIData.Page> pageStack = new Stack<EZUIData.Page>();
		
		/// <summary>
		/// Triggered when a Page is shown
		/// </summary>
		/// /// <param name="key">Page Key</param>
		public event Action<string> OnShowPage;
		
		/// <summary>
		/// Triggered when a Popup is shown
		/// </summary>
		/// <param name="key">Popup Key</param>
		public event Action<string> OnShowPopup;
		
		/// <summary>
		/// Triggers on back press when there is no
		/// Popups and Panels left in the stack.
		/// </summary>
		public event Action ShouldQuit;
		
		/// <summary>
		/// Currently active page key
		/// </summary>
		public string CurrentPageKey { get; private set; }
		
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
			
			EZUIData.Page defaultPage = data.GetDefaultPage();
			if (defaultPage != null)
			{
				HideAllNoClearStack(true);
				ShowPage(defaultPage, true);
			}
		}
		
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
				GoBack();
		}

		/// <summary>
		/// Initialize
		/// </summary>
		private void Init()
		{
			data.Init();
			
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

		/// <summary>
		/// Shows the Page and add to stack
		/// </summary>
		/// <param name="key">Page key</param>
		public void ShowPage(string key)
		{
			ShowPage(key, false);
		}

		/// <summary>
		/// Shows the Page and add to stack
		/// </summary>
		/// <param name="key">Page key</param>
		/// <param name="immediate">true- no animations, false- with animations</param>
		public void ShowPage(string key, bool immediate)
		{
			if (debug)
			{
				if (!data.pagesDictionary.ContainsKey(key))
				{
					Debug.LogError($"Page not found with key: {key}");
					return;
				}
			}
			
			ShowPage(data.pagesDictionary[key], immediate);
		}
		
		/// <summary>
		/// Shows the Page
		/// </summary>
		/// <param name="page">Page key</param>
		/// <param name="immediate">true- no animations, false- with animations</param>
		/// <param name="addToBackStack">FOR INTERNAL USE ONLY</param>
		/// <returns>If the requested page is shown or not</returns>
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
			
			if (EZUIPanel.RunningAnimations > 0 || CurrentPageKey == page.key)
				return false;
			
			CurrentPageKey = page.key;

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

		/// <summary>
		/// Shows the Popup and add to stack
		/// </summary>
		/// <param name="key">Popup key</param>
		public void ShowPopup(string key)
		{
			ShowPopup(key, false);
		}
		
		/// <summary>
		/// Shows the Popup and add to stack
		/// </summary>
		/// <param name="key">Popup key</param>
		/// <param name="immediate">true- no animations, false- with animations</param>
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
		
		/// <summary>
		/// Shows the Popup and add to stack
		/// </summary>
		/// <param name="popup">Popup object</param>
		/// <param name="immediate">true- no animations, false- with animations</param>
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
		
		/// <summary>
		/// Hides the Popup and remove from stack
		/// </summary>
		/// <param name="key">Popup key</param>
		public void HidePopup(string key)
		{
			HidePopup(key, false);
		}
		
		/// <summary>
		/// Hides the Popup and remove from stack
		/// </summary>
		/// <param name="key">Popup key</param>
		/// <param name="immediate">true- no animations, false- with animations</param>
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
		
		/// <summary>
		/// Hides the Popup and remove from stack
		/// </summary>
		/// <param name="popup">Popup object</param>
		/// <param name="immediate">true- no animations, false- with animations</param>
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
		
		/// <summary>
		/// Hide All the Popups and Pages and clear the stack
		/// </summary>
		/// <param name="immediate">true- no animations, false- with animations</param>
		public void HideAllAndClearStack(bool immediate)
		{
			popupStack.Clear();
			pageStack.Clear();
			HideAllNoClearStack(immediate);
		}
		
		/// <summary>
		/// Hide All the Popups and Pages
		/// This does not remove anything from stack
		/// </summary>
		/// <param name="immediate">true- no animations, false- with animations</param>
		private void HideAllNoClearStack(bool immediate)
		{
			foreach (EZUIPanel panel in panels)
				panel.SetVisible(false, immediate);
			
			foreach (EZUIPopup popup in popups)
				popup.SetVisible(false, immediate);
		}

		/// <summary>
		/// Go back once, Precedence: Popup > Page
		/// Also removes the current Popup/Page from stack
		/// </summary>
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

		/// <summary>
		/// Print the stack
		/// </summary>
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
