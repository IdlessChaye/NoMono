using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NingyoRi
{
	public class MainMenuPageContext : BasePageContext
	{
		private MonoMainMenuPage _mono;
		public override void Init()
		{
			_prefabPath = @"UI/Contexts/PageContexts/MainMenuPage";
		}

		public override void Setup()
		{
			_mono = _monoContext as MonoMainMenuPage;

		}

		public override void SetupCallbacks()
		{
			BindCallback(_mono.startButton, () => Debug.Log("Hello"));
		}
	}
}