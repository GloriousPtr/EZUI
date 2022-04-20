using TMPro;
using UnityEngine;
using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo(EZUI.StaticData.EZUI_EditorAssemblyName)]
namespace EZUI
{
	public class EZUIProgressorTargetText : EZUIProgressorTarget
	{
		public TextMeshProUGUI textLabel;
		public bool useProgress = true;
		public string prefix;
		public string suffix = "%";
		
		internal override void ShouldChange(EZUIProgressor progressor)
		{
			float value = useProgress
				? progressor.GetCurrentProgress()
				: progressor.GetCurrentValue();
			
			textLabel.text = $"{prefix} {value} {suffix}";
		}

		public void SetPercent(float percent)
		{
			textLabel.text = $"{prefix} {Mathf.RoundToInt(percent * 100)} {suffix}";
		}
	}
}
