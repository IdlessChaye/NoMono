using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NingyoRi
{
	public class MonoMainMenuPage : BaseMonoPageContext
	{
		[SerializeField]
		private Button _startButton;

		public Button startButton { get { return _startButton; } }
	}
}