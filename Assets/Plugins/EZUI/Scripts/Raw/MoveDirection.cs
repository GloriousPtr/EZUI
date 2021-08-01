using System;
using UnityEngine;

namespace EZUI.Animation
{
	public enum MoveDirection
	{
		Left,
		Right,
		Top,
		Bottom,
		
		TopLeft,
		TopCentre,
		TopRight,
		
		MiddleLeft,
		MiddleCentre,
		MiddleRight,
		
		BottomLeft,
		BottomCentre,
		BottomRight,
		
		CustomPosition
	}
	
	public static class MoveDirectionExtension
	{
		public static Vector2 GetAnchoredPosition(this MoveDirection moveDirection, RectTransform rectTransform)
		{
			Vector2 position = new Vector2();

			Rect rect = rectTransform.rect;

			switch (moveDirection)
			{
				case MoveDirection.Left:
					position.x = rectTransform.position.x - rect.width;
					position.y = rectTransform.anchoredPosition.y;
					break;
				case MoveDirection.Right:
					position.x = rectTransform.position.x + rect.width;
					position.y = rectTransform.anchoredPosition.y;
					break;
				case MoveDirection.Top:
					position.x = rectTransform.anchoredPosition.x;
					position.y = rectTransform.position.y + rect.height;
					break;
				case MoveDirection.Bottom:
					position.x = rectTransform.anchoredPosition.x;
					position.y = rectTransform.position.y - rect.height;
					break;
				case MoveDirection.TopLeft:
					position.x = rectTransform.position.x - rect.width;
					position.y = rectTransform.position.y + rect.height;
					break;
				case MoveDirection.TopCentre:
					position.y = rectTransform.position.y + rect.height;
					break;
				case MoveDirection.TopRight:
					position.x = rectTransform.position.x + rect.width;
					position.y = rectTransform.position.y + rect.height;
					break;
				case MoveDirection.MiddleLeft:
					position.x = rectTransform.position.x - rect.width;
					break;
				case MoveDirection.MiddleCentre:
					break;
				case MoveDirection.MiddleRight:
					position.x = rectTransform.position.x + rect.width;
					break;
				case MoveDirection.BottomLeft:
					position.x = rectTransform.position.x - rect.width;
					position.y = rectTransform.position.y - rect.height;
					break;
				case MoveDirection.BottomCentre:
					position.y = rectTransform.position.y - rect.height;
					break;
				case MoveDirection.BottomRight:
					position.x = rectTransform.position.x + rect.width;
					position.y = rectTransform.position.y - rect.height;
					break;
				case MoveDirection.CustomPosition:
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(moveDirection), moveDirection, null);
			}
			
			return position;
		}
	}
}