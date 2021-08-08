using DG.Tweening;
using UnityEngine;

namespace EZUI.Animation
{
	public static class AnimationProcessor
	{
		#region Move
		
		public static Sequence ProcessMove(this RectTransform rectTransform, MoveData moveData, bool immediate = false)
		{
			if (moveData.moveMode == MoveMode.Anchored)
				return rectTransform.ProcessRelativeMove(moveData, immediate);
			
			Sequence sequence = DOTween.Sequence();
			if (!immediate)
			{
				if (moveData.state == State.None)
				{
					return moveData.mode switch
					{
						Mode.None => sequence.Append(rectTransform.Move(moveData)).Play(),
						Mode.By => sequence.Append(rectTransform.MoveBy(moveData)).Play(),
						Mode.Punch => sequence.Append(rectTransform.MovePunch(moveData)).Play(),
						_ => sequence
					};
				}

				return moveData.state switch
				{
					State.In => sequence.Append(rectTransform.MoveIn(moveData)).Play(),
					State.Out => sequence.Append(rectTransform.MoveOut(moveData)).Play(),
					_ => sequence
				};
			}
			
			rectTransform.localPosition = moveData.to;
			return sequence;
		}

		private static Tween Move(this Transform transform, MoveData moveData)
		{
			transform.localPosition = moveData.from;
			return transform.DOLocalMove(moveData.to, moveData.duration)
				.SetDelay(moveData.startDelay)
				.SetEase(moveData.ease);
		}
		
		private static Tween MoveIn(this Transform transform, MoveData moveData)
		{
			transform.localPosition = moveData.from;
			return transform.DOLocalMove(moveData.initialValue, moveData.duration)
				.SetDelay(moveData.startDelay)
				.SetEase(moveData.ease);
		}
		
		private static Tween MoveOut(this Transform transform, MoveData moveData)
		{
			return transform.DOLocalMove(moveData.to, moveData.duration)
				.SetDelay(moveData.startDelay)
				.SetEase(moveData.ease);
		}
		
		private static Tween MoveBy(this Transform transform, MoveData moveData)
		{
			return transform.DOLocalMove(transform.localPosition + moveData.by, moveData.duration)
				.SetDelay(moveData.startDelay)
				.SetEase(moveData.ease);
		}
		
		private static Tween MovePunch(this Transform transform, MoveData moveData)
		{
			return transform.DOPunchPosition(moveData.by, moveData.duration, moveData.vibrato, moveData.elasticity)
				.SetDelay(moveData.startDelay)
				.SetEase(moveData.ease);
		}
		
		#endregion

		#region RelativeMove
		
		public static Sequence ProcessRelativeMove(this RectTransform rectTransform, MoveData moveData, bool immediate)
		{
			Sequence sequence = DOTween.Sequence();
			if (!immediate)
			{
				if (moveData.state == State.None)
				{
					return moveData.mode switch
					{
						Mode.None => sequence.Append(rectTransform.AnchoredMove(moveData)).Play(),
						Mode.By => sequence.Append(rectTransform.AnchoredMoveBy(moveData)).Play(),
						Mode.Punch => sequence.Append(rectTransform.AnchoredMovePunch(moveData)).Play(),
						_ => sequence
					};
				}
				
				return moveData.state switch
				{
					State.In => sequence.Append(rectTransform.AnchoredMoveIn(moveData)).Play(),
					State.Out => sequence.Append(rectTransform.AnchoredMoveOut(moveData)).Play(),
					_ => sequence
				};
			}
			rectTransform.anchoredPosition = moveData.to;
			return sequence;
		}

		private static Tween AnchoredMove(this RectTransform rectTransform, MoveData moveData)
		{
			rectTransform.anchoredPosition = moveData.from;
			return rectTransform.DOAnchorPos(moveData.to, moveData.duration)
				.SetDelay(moveData.startDelay)
				.SetEase(moveData.ease);
		}
		
		private static Tween AnchoredMoveIn(this RectTransform rectTransform, MoveData moveData)
		{
			bool customPosition = moveData.moveDirection == MoveDirection.CustomPosition;
			Vector2 anchoredPosition = moveData.moveDirection.GetAnchoredPosition(rectTransform);
			
			rectTransform.anchoredPosition = customPosition ? (Vector2) moveData.from : anchoredPosition;
			return rectTransform.DOAnchorPos(moveData.initialValue, moveData.duration)
				.SetDelay(moveData.startDelay)
				.SetEase(moveData.ease);
		}
		
		private static Tween AnchoredMoveOut(this RectTransform rectTransform, MoveData moveData)
		{
			bool customPosition = moveData.moveDirection == MoveDirection.CustomPosition;
			Vector3 anchoredPosition = moveData.moveDirection.GetAnchoredPosition(rectTransform);
			
			return rectTransform.DOAnchorPos(customPosition ? moveData.to : anchoredPosition, moveData.duration)
				.SetDelay(moveData.startDelay)
				.SetEase(moveData.ease);
		}
		
		private static Tween AnchoredMoveBy(this RectTransform rectTransform, MoveData moveData)
		{
			return rectTransform.DOAnchorPos(rectTransform.anchoredPosition + (Vector2) moveData.by, moveData.duration)
				.SetDelay(moveData.startDelay)
				.SetEase(moveData.ease);
		}
		
		private static Tween AnchoredMovePunch(this RectTransform rectTransform, MoveData moveData)
		{
			return rectTransform.DOPunchAnchorPos(moveData.by, moveData.duration, moveData.vibrato, moveData.elasticity)
				.SetDelay(moveData.startDelay)
				.SetEase(moveData.ease);
		}

		#endregion

		#region Rotate

		public static Sequence ProcessRotation(this Transform transform, RotationData rotationData, bool immediate = false)
		{
			Sequence sequence = DOTween.Sequence();
			if (!immediate)
			{
				if (rotationData.state == State.None)
				{
					return rotationData.mode switch
					{
						Mode.None => sequence.Append(transform.Rotate(rotationData)).Play(),
						Mode.By => sequence.Append(transform.RotateBy(rotationData)).Play(),
						Mode.Punch => sequence.Append(transform.RotatePunch(rotationData)).Play(),
						_ => sequence
					};
				}
				
				return rotationData.state switch
				{
					State.In => sequence.Append(transform.RotateIn(rotationData)).Play(),
					State.Out => sequence.Append(transform.RotateOut(rotationData)).Play(),
					_ => sequence
				};
			}
			transform.localEulerAngles = rotationData.to;
			return sequence;
		}
		
		private static Tween Rotate(this Transform transform, RotationData rotationData)
		{
			transform.localEulerAngles = rotationData.from;
			return transform.DOLocalRotate(rotationData.to, rotationData.duration, rotationData.rotateMode)
				.SetDelay(rotationData.startDelay)
				.SetEase(rotationData.ease);
		}
		
		private static Tween RotateIn(this Transform transform, RotationData rotationData)
		{
			transform.localEulerAngles = rotationData.from;
			return transform.DOLocalRotate(rotationData.initialValue, rotationData.duration, rotationData.rotateMode)
				.SetDelay(rotationData.startDelay)
				.SetEase(rotationData.ease);
		}
		
		private static Tween RotateOut(this Transform transform, RotationData rotationData)
		{
			return transform.DOLocalRotate(rotationData.to, rotationData.duration, rotationData.rotateMode)
				.SetDelay(rotationData.startDelay)
				.SetEase(rotationData.ease);
		}
		
		private static Tween RotateBy(this Transform transform, RotationData rotationData)
		{
			return transform.DOLocalRotate(transform.localEulerAngles + rotationData.by, rotationData.duration, rotationData.rotateMode)
				.SetDelay(rotationData.startDelay)
				.SetEase(rotationData.ease);
		}
		
		private static Tween RotatePunch(this Transform transform, RotationData rotationData)
		{
			return transform.DOPunchRotation(rotationData.by, rotationData.duration, rotationData.vibrato, rotationData.elasticity)
				.SetDelay(rotationData.startDelay)
				.SetEase(rotationData.ease);
		}
		
		#endregion

		#region Scale
		
		public static Sequence ProcessScale(this Transform transform, ScaleData scaleData, bool immediate = false)
		{
			Sequence sequence = DOTween.Sequence();

			if (!immediate)
			{
				if (scaleData.state == State.None)
				{
					return scaleData.mode switch
					{
						Mode.None => sequence.Append(transform.Scale(scaleData)).Play(),
						Mode.By => sequence.Append(transform.ScaleBy(scaleData)).Play(),
						Mode.Punch => sequence.Append(transform.ScalePunch(scaleData)).Play(),
						_ => sequence
					};
				}
				
				return scaleData.state switch
				{
					State.In => sequence.Append(transform.ScaleIn(scaleData)).Play(),
					State.Out => sequence.Append(transform.ScaleOut(scaleData)).Play(),
					_ => sequence
				};
			}
			
			transform.localScale = scaleData.to;
			return sequence;
		}

		private static Tween Scale(this Transform transform, ScaleData scaleData)
		{
			transform.localScale = scaleData.from;
			return transform.DOScale(scaleData.to, scaleData.duration)
				.SetDelay(scaleData.startDelay)
				.SetEase(scaleData.ease);
		}
		
		private static Tween ScaleIn(this Transform transform, ScaleData scaleData)
		{
			transform.localScale = scaleData.from;
			return transform.DOScale(scaleData.initialValue, scaleData.duration)
				.SetDelay(scaleData.startDelay)
				.SetEase(scaleData.ease);
		}
		
		private static Tween ScaleOut(this Transform transform, ScaleData scaleData)
		{
			return transform.DOScale(scaleData.to, scaleData.duration)
				.SetDelay(scaleData.startDelay)
				.SetEase(scaleData.ease);
		}
		
		private static Tween ScaleBy(this Transform transform, ScaleData scaleData)
		{
			return transform.DOScale(transform.localScale + scaleData.by, scaleData.duration)
				.SetDelay(scaleData.startDelay)
				.SetEase(scaleData.ease);
		}
		
		private static Tween ScalePunch(this Transform transform, ScaleData scaleData)
		{
			return transform.DOPunchScale(scaleData.by, scaleData.duration, scaleData.vibrato, scaleData.elasticity)
				.SetDelay(scaleData.startDelay)
				.SetEase(scaleData.ease);
		}
		
		#endregion

		#region Fade
		
		public static Sequence ProcessFade(this CanvasGroup canvasGroup, FloatData floatData, bool immediate = false)
		{
			Sequence sequence = DOTween.Sequence();
			if (!immediate)
			{
				return floatData.state switch
				{
					State.None => sequence.Append(canvasGroup.Fade(floatData)).Play(),
					State.In => sequence.Append(canvasGroup.FadeIn(floatData)).Play(),
					State.Out => sequence.Append(canvasGroup.FadeOut(floatData)).Play(),
					_ => sequence
				};
			}

			canvasGroup.alpha = floatData.to;
			return sequence;
		}

		private static Tween Fade(this CanvasGroup canvasGroup, FloatData floatData)
		{
			canvasGroup.alpha = floatData.from;
			return canvasGroup.DOFade(floatData.to, floatData.duration)
				.SetDelay(floatData.startDelay)
				.SetEase(floatData.ease);
		}

		private static Tween FadeIn(this CanvasGroup canvasGroup, FloatData floatData)
		{
			canvasGroup.alpha = floatData.from;
			return canvasGroup.DOFade(floatData.initialValue, floatData.duration)
				.SetDelay(floatData.startDelay)
				.SetEase(floatData.ease);
		}

		private static Tween FadeOut(this CanvasGroup canvasGroup, FloatData floatData)
		{
			return canvasGroup.DOFade(floatData.to, floatData.duration)
				.SetDelay(floatData.startDelay)
				.SetEase(floatData.ease);
		}
		
		#endregion
	}
}