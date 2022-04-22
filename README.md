# EZUI: UI Solution for Unity

[![Build Status](https://github.com/MohitSethi99/EZUI/workflows/build/badge.svg)](https://github.com/MohitSethi99/EZUI/actions?workflow=build)
[![CodeFactor](https://www.codefactor.io/repository/github/mohitsethi99/ezui/badge)](https://www.codefactor.io/repository/github/mohitsethi99/ezui)

![Unity](https://img.shields.io/badge/platform-Unity-blue?style=flat-square)
![GitHub](https://img.shields.io/github/license/MohitSethi99/EZUI?color=blue&style=flat-square)
![Size](https://img.shields.io/github/repo-size/MohitSethi99/EZUI?style=flat-square)

Unity Animation system is not meant to work with UI.
The problem here is if one panel is updated every child and subsequent children in the tree which must be UI element (quad) are marked dirty. This leads to poor performance because of the resubmission each frame CPU has to do and Unity Animation system does this even when animations are not running.

The solution to this is using tweens. So I created this project along with UI Management features (Panels, Popups, back stack, and many more)

# Features

- Easy To Use UI Solution.
- Uses DOTween for UI animations.

# Demo

Check out Assets/Plugins/EZUI/Scenes/Demo.unity scene.

# Documentation

## Prerequisites
- Import DOTween and set it up
- Import EZUI.unitypackage from [here](https://github.com/MohitSethi99/EZUI/releases)
- If using assembly definitions for DOTween, add the reference to Plugins/EZUI/Scripts/EZUI.Core.asmdef

## Setting Up
- In Hierarchy, Right Click&#8594;EZUI&#8594;EZUIManager.
- In project window, Right Click&#8594;Create&#8594;EZUI&#8594;EZUIData.
- Assign newly created EZUIData to EZUIManager&#8594;Data.
- Each canvas should be assigned to `EZUIManager>UIModules` list for the system to work correctly.
- Now EZUI&#8594;EZUIPanel or EZUI&#8594;EZUIPopup can be created inside their respective canvas.

## Video Tutorial

[![EZUI](https://yt-embed.herokuapp.com/embed?v=wslE4BnRLk8)](https://youtube.com/playlist?list=PLoY41wN_Nn7ia87iad-Z7kMlNxKdrE0zD "EZUI")

## API Documentation

The API is exposed through EZUIManager.Instance which is a singleton.
```csharp
/// <summary>
/// Shows the Page and add to stack
/// </summary>
/// <param name="key">Page key</param>
EZUIManager.Instance.ShowPage(string key);

/// <summary>
/// Shows the Page and add to stack
/// </summary>
/// <param name="key">Page key</param>
/// <param name="immediate">true- no animations, false- with animations</param>
EZUIManager.Instance.ShowPage(string key, bool immediate);

/// <summary>
/// Shows the Popup and add to stack
/// </summary>
/// <param name="key">Popup key</param>
EZUIManager.Instance.ShowPopup(string key);

/// <summary>
/// Shows the Popup and add to stack
/// </summary>
/// <param name="key">Popup key</param>
/// <param name="immediate">true- no animations, false- with animations</param>
EZUIManager.Instance.ShowPopup(string key, bool immediate);

/// <summary>
/// Shows the Popup and add to stack
/// </summary>
/// <param name="popup">Popup object</param>
/// <param name="immediate">true- no animations, false- with animations</param>
EZUIManager.Instance.ShowPopup(EZUIPopup popup, bool immediate);

/// <summary>
/// Hides the Popup and remove from stack
/// </summary>
/// <param name="key">Popup key</param>
EZUIManager.Instance.HidePopup(string key);

/// <summary>
/// Hides the Popup and remove from stack
/// </summary>
/// <param name="key">Popup key</param>
/// <param name="immediate">true- no animations, false- with animations</param>
EZUIManager.Instance.HidePopup(string key, bool immediate);

/// <summary>
/// Hides the Popup and remove from stack
/// </summary>
/// <param name="popup">Popup object</param>
/// <param name="immediate">true- no animations, false- with animations</param>
EZUIManager.Instance.HidePopup(EZUIPopup popup, bool immediate);

/// <summary>
/// Hide All the Popups and Pages and clear the stack
/// </summary>
/// <param name="immediate">true- no animations, false- with animations</param>
EZUIManager.Instance.HideAllAndClearStack(bool immediate)

/// <summary>
/// Go back once, Precedence: Popup > Page
/// Also removes the current Popup/Page from stack
/// </summary>
EZUIManager.Instance.GoBack();
```
