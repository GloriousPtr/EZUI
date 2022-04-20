using UnityEditor.AnimatedValues;
using UnityEngine.Events;

namespace EZUI_Editor
{
	internal class AnimBoolGroup
	{
		internal AnimBool expanded = new AnimBool(false);
		internal AnimBool move = new AnimBool(false);
		internal AnimBool rotate = new AnimBool(false);
		internal AnimBool scale = new AnimBool(false);
		internal AnimBool fade = new AnimBool(false);
		internal AnimBool events = new AnimBool(false);

		internal AnimBoolGroup(UnityAction repaint)
		{
			expanded.valueChanged.AddListener(repaint);
			move.valueChanged.AddListener(repaint);
			rotate.valueChanged.AddListener(repaint);
			scale.valueChanged.AddListener(repaint);
			fade.valueChanged.AddListener(repaint);
			events.valueChanged.AddListener(repaint);
		}
	}
}
