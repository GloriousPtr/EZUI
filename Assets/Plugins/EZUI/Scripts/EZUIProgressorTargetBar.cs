using UnityEngine;
using UnityEngine.UI;
using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo(EZUI.StaticData.EZUI_EditorAssemblyName)]
namespace EZUI
{
	public class EZUIProgressorTargetBar : EZUIProgressorTarget
	{
		[SerializeField] private Image mask;
		[SerializeField] private Image fill;
		[SerializeField] private Color color = new Color(0.223f, 0.53f, 0.7f);

#if UNITY_EDITOR
		private void OnValidate()
		{
			if (fill)
				fill.color = color;
		}
#endif

		internal override void SetValue(EZUIProgressor progressor)
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