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
- Import EZUI.unitypackage
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

WIP
