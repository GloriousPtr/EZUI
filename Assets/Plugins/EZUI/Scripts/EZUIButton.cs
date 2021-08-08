using UnityEngine;
using UnityEngine.UI;
using EZUI.Animation;
using TMPro;
using UnityEngine.EventSystems;

namespace EZUI
{
	[RequireComponent(typeof(Button))]
	public class EZUIButton : EZUIBase, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
	{
		[SerializeField] private AnimationData onPointerEnter = new AnimationData(State.None, Mode.Punch);
		[SerializeField] private AnimationData onPointerExit = new AnimationData(State.None, Mode.Punch);
		[SerializeField] private AnimationData onPointerDown = new AnimationData(State.None, Mode.Punch);
		[SerializeField] private AnimationData onPointerUp = new AnimationData(State.None, Mode.Punch);
		[SerializeField] private AnimationData onClick = new AnimationData(State.None, Mode.Punch);
		
		[SerializeField] private string displayText;
		[SerializeField] private Button button;
		
		public TextMeshProUGUI textLabel;
		
		private void Start()
		{
			button.onClick.AddListener(OnClick);
		}

		protected override bool UseCustomStartPosition(out Vector3 customPosition)
		{
			customPosition = rectTransform.anchoredPosition;
			return false;
		}

		protected override void SetInitialState()
		{
		}

		protected override void Init()
		{
			base.Init();
			
			button = GetComponent<Button>();
			textLabel = GetComponentInChildren<TextMeshProUGUI>();
			displayText = textLabel.text;
		}
		
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (!onPointerEnter.isAnimating)
				StartTransition(onPointerEnter);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			if (!onPointerExit.isAnimating)
				StartTransition(onPointerExit);
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			if (!onPointerDown.isAnimating)
				StartTransition(onPointerDown);
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			if (!onPointerUp.isAnimating)
				StartTransition(onPointerUp);
		}

		public void OnClick()
		{
			if (!onClick.isAnimating)
				StartTransition(onClick);
		}
	}
}