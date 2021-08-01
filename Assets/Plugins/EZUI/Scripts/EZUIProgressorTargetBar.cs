using UnityEngine;
using UnityEngine.UI;

namespace EZUI
{
	public class EZUIProgressorTargetBar : EZUIProgressorTarget
	{
		public Image mask;
		public Image fill;
		public Color color = new Color(0.223f, 0.53f, 0.7f);

#if UNITY_EDITOR
		private void OnValidate()
		{
			if (fill)
				fill.color = color;
		}
#endif

		public override void SetValue(EZUIProgressor progressor)
		{
			if (mask)
				mask.fillAmount = progressor.CurrentPercent;
		}

		public override void SetPercent(float percent)
		{
			if (mask)
				mask.fillAmount = percent;
		}
	}
}