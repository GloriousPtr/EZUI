using UnityEditor.AnimatedValues;
using UnityEngine.Events;

namespace EZUI_Editor
{
	public class AnimBoolGroup
	{
		public AnimBool expanded = new AnimBool(false);
		public AnimBool move = new AnimBool(false);
		public AnimBool rotate = new AnimBool(false);
		public AnimBool scale = new AnimBool(false);
		public AnimBool fade = new AnimBool(false);
		public AnimBool events = new AnimBool(false);

		public AnimBoolGroup(UnityAction repaint)
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