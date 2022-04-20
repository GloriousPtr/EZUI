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

		internal override void ShouldChange(EZUIProgressor progressor)
		{
			if (mask)
				mask.fillAmount = progressor.GetCurrentPercent();
		}

		/// <summary>
		/// Sets the Bar Fill
		/// </summary>
		/// <param name="percent">Percent [0.0f, 1.0f]</param>
		public void SetPercent(float percent)
		{
			if (mask)
				mask.fillAmount = percent;
		}

		/// <summary>
		/// Sets the color of Progress Bar Fill
		/// </summary>
		/// <param name="fillColor">Color value</param>
		public void SetColor(Color fillColor)
		{
			color = fillColor;
			if (fill)
				fill.color = fillColor;
		}
	}
}
