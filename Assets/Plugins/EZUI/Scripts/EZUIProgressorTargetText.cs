using TMPro;
using UnityEngine;

namespace EZUI
{
	public class EZUIProgressorTargetText : EZUIProgressorTarget
	{
		[SerializeField] private TextMeshProUGUI textLabel;
		[SerializeField] private bool useProgress = true;
		[SerializeField] private string prefix;
		[SerializeField] private string suffix = "%";
		
		public override void SetValue(EZUIProgressor progressor)
		{
			float value = useProgress
				? progressor.CurrentProgress
				: progressor.CurrentValue;
			
			textLabel.text = $"{prefix} {value} {suffix}";
		}

		public override void SetPercent(float percent)
		{
			textLabel.text = $"{prefix} {Mathf.RoundToInt(percent * 100)} {suffix}";
		}
	}
}