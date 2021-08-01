using UnityEngine;
using UnityEngine.UI;

namespace EZUI
{
	[RequireComponent(typeof(Button))]
	public class ShowPanelButton : MonoBehaviour
	{
		public bool isPage = true;
		public string pageName;
		public string panelName;
		public bool closePanel;

		private void Start()
		{
			GetComponent<Button>().onClick.AddListener(OnShow);
		}

		private void OnShow()
		{
			/*
			if (isPage)
			{
				if (pageName == "<-")
					EZUIManager.instance.ShowPreviousPage();
				else
					EZUIManager.instance.ShowPage(pageName);
			}
			else
			{
				EZUIManager.instance.SetPanelVisible(panelName, !closePanel);
			}
			*/
		}
	}
}
