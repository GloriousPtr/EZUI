using System;
using DG.Tweening;
using EZUI.Animation;
using UnityEngine;

namespace EZUI
{
	[RequireComponent(typeof(RectTransform))]
	[RequireComponent(typeof(CanvasGroup))]
	public abstract class EZUIBase : MonoBehaviour
	{
		protected CanvasGroup canvasGroup;
		protected RectTransform rectTransform;
		
		private float _originalAlpha = 1.0f;
		
		public abstract bool UseCustomStartPosition(out Vector3 customPosition);
		
		protected virtual void Awake()
		{
			Init();
		}
		
		public virtual void Init()
		{
			rectTransform = GetComponent<RectTransform>();
			canvasGroup = GetComponent<CanvasGroup>();
			
			_originalAlpha = canvasGroup.alpha;
		}
		
		public virtual Tween StartTransition(AnimationData animationData, bool immidiate = false, Action callback = null)
		{
			animationData.isAnimating = true;
			
			canvasGroup.alpha = _originalAlpha;
			
			animationData.onStarted?.Invoke();
			
			Sequence sequence = DOTween.Sequence();
			
			SetCustomPosition();
			
			if (animationData.move)
				sequence.Join(rectTransform.ProcessMove(animationData.moveData, immidiate));

			if (animationData.rotation)
				sequence.Join(rectTransform.ProcessRotation(animationData.rotationData, immidiate));

			if (animationData.scale)
				sequence.Join(rectTransform.ProcessScale(animationData.scaleData, immidiate));

			if (animationData.fade)
				sequence.Join(canvasGroup.ProcessFade(animationData.fadeData, immidiate));

			sequence.AppendCallback(() =>
			{
				callback?.Invoke();
				animationData.onCompleted?.Invoke();
				animationData.isAnimating = false;
				sequence.Kill(true);
			});

			sequence.Play();
			return sequence;
		}

		private void SetCustomPosition()
		{
			bool use = UseCustomStartPosition(out Vector3 customPosition);
			
			if (use)
				rectTransform.localPosition = customPosition;
		}
	}
}