using System;
using UnityEngine;
using EZUI.Animation;
using DG.Tweening;

namespace EZUI
{
	public class EZUIPanel : EZUIBase
	{
		public string key;
		public bool useCustomStartPosition = true;
		public Vector3 customStartPosition;
		
		public AnimationData showData = new AnimationData(State.In, Mode.None);
		public AnimationData hideData = new AnimationData(State.Out, Mode.None);

		public static int RunningAnimations { get; private set; }

		public void SetVisible(bool active, bool immidiate = false)
		{
			if (active == gameObject.activeSelf)
				return;
			
			if (!gameObject.activeSelf)
				gameObject.SetActive(true);

			if (active && useCustomStartPosition)
				rectTransform.localPosition = customStartPosition;
			
			StartTransition(active ? showData : hideData, immidiate, () =>
			{
				if (!active)
					gameObject.SetActive(false);
			});
		}
		
		public override bool UseCustomStartPosition(out Vector3 customPosition)
		{
			customPosition = useCustomStartPosition ? customStartPosition : (Vector3) rectTransform.anchoredPosition;
			return useCustomStartPosition;
		}

		public override Tween StartTransition(AnimationData animationData, bool immidiate = false, Action callback = null)
		{
			RunningAnimations++;
			
			Tween tween = base.StartTransition(animationData, immidiate, () =>
			{
				RunningAnimations--;
				callback?.Invoke();
			});
			
			return tween;
		}
	}
}
