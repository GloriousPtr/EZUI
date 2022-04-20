using UnityEngine;

namespace EZUI_Editor
{
	internal static class EZUISkin
	{
		private static GUISkin _skin;

		private static GUIStyle _box;
		private static GUIStyle _largeButtonSelected;
		private static GUIStyle _largeButtonDefault;
		private static GUIStyle _mediumButtonDefault;
		private static GUIStyle _foldoutButton;
		private static GUIStyle _tabButtonDefault;
		private static GUIStyle _tabButtonSelected;
		
		private static GUISkin Skin
		{
			get
			{
				if (_skin != null)
					return _skin;
				
				_skin = (GUISkin) Resources.Load("EditorSkins/EZUIEditorSkin");
				if (_skin == null)
					Debug.LogError("Skin not found at: EditorSkins/EZUIEditorSkin");
				
				return _skin;
			}
		}
		
		internal static GUIStyle Box
		{
			get
			{
				if (_box != null)
					return _box;
				
				_box = Skin.GetStyle("Box");
				return _box;
			}
		}
		
		internal static GUIStyle LargeButtonSelected
		{
			get
			{
				if (_largeButtonSelected != null)
					return _largeButtonSelected;
				
				_largeButtonSelected = Skin.GetStyle("LargeButtonSelected");
				return _largeButtonSelected;
			}
		}
		
		internal static GUIStyle LargeButtonDefault
		{
			get
			{
				if (_largeButtonDefault != null)
					return _largeButtonDefault;
				
				_largeButtonDefault = Skin.GetStyle("LargeButtonDefault");
				return _largeButtonDefault;
			}
		}
		
		internal static GUIStyle MediumButtonDefault
		{
			get
			{
				if (_mediumButtonDefault != null)
					return _mediumButtonDefault;
				
				_mediumButtonDefault = Skin.GetStyle("MediumButtonDefault");
				return _mediumButtonDefault;
			}
		}
		
		internal static GUIStyle FoldoutButton
		{
			get
			{
				if (_foldoutButton != null)
					return _foldoutButton;
				
				_foldoutButton = Skin.GetStyle("FoldoutButton");
				return _foldoutButton;
			}
		}
		
		internal static GUIStyle TabButtonDefault
		{
			get
			{
				if (_tabButtonDefault != null)
					return _tabButtonDefault;
				
				_tabButtonDefault = Skin.GetStyle("TabButtonDefault");
				return _tabButtonDefault;
			}
		}
		
		internal static GUIStyle TabButtonSelected
		{
			get
			{
				if (_tabButtonSelected != null)
					return _tabButtonSelected;
				
				_tabButtonSelected = Skin.GetStyle("TabButtonSelected");
				return _tabButtonSelected;
			}
		}
	}
}
