﻿using System;
using UnityEngine;
using EZUI.Animation;
using DG.Tweening;
using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo(EZUI.StaticData.EZUI_EditorAssemblyName)]
namespace EZUI
{
	public class EZUIPanel : EZUIBase
	{
		[SerializeField] internal string key;
		[SerializeField] private AnimationData showData = new AnimationData(State.In, Mode.None);
		[SerializeField] private AnimationData hideData = new AnimationData(State.Out, Mode.None);
		
		[SerializeField] private bool useCustomStartPosition = true;
		[SerializeField] private Vector3 customStartPosition;
		
		internal static int RunningAnimations { get; private set; }

		internal void SetVisible(bool active, bool immediate = false)
		{
			if (active)
				ResetState();
			
			if (active == gameObject.activeSelf)
				return;
			
			if (immediate)
			{
				gameObject.SetActive(active);
				return;
			}
			
			if (!gameObject.activeSelf)
				gameObject.SetActive(true);

			if (active && useCustomStartPosition)
				rectTransform.localPosition = customStartPosition;
			
			StartTransition(active ? showData : hideData, immediate, () =>
			{
				if (!active)
					gameObject.SetActive(false);
			});
		}
		
		protected override bool UseCustomStartPosition(out Vector3 customPosition)
		{
			customPosition = useCustomStartPosition ? customStartPosition : (Vector3) rectTransform.anchoredPosition;
			return useCustomStartPosition;
		}

		protected override void SetInitialState()
		{
			showData.moveData.initialValue = rectTransform.localPosition;
			showData.rotationData.initialValue = rectTransform.localEulerAngles;
			showData.scaleData.initialValue = rectTransform.localScale;
			showData.fadeData.initialValue = canvasGroup.alpha;
		}

		protected override Tween StartTransition(AnimationData animationData, bool immidiate = false, Action callback = null)
		{
			RunningAnimations++;
			
			Tween tween = base.StartTransition(animationData, immidiate, () =>
			{
				RunningAnimations--;
				callback?.Invoke();
			});
			
			return tween;
		}
		
		private void ResetState()
		{
			rectTransform.localPosition = StaticData.ZeroVector;
			rectTransform.localRotation = Quaternion.identity;
			rectTransform.localScale = StaticData.UnitVector;
			canvasGroup.alpha = 1.0f;
		}
	}
}
