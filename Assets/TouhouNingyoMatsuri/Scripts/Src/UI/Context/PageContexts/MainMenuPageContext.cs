using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NingyoRi
{
	public class MainMenuPageContext : BasePageContext
	{
		protected override string _prefabPath { get { return @"UI/Contexts/PageContexts/MainMenuPage"; } }

		private MonoMainMenuPage _mono;
		protected override void Init()
		{
			_mono = _monoContext as MonoMainMenuPage;
		}
		protected override void SetupCallbacks()
		{
			base.SetupCallbacks();
			BindCallback(_mono.startButton, () => UnityEngine.SceneManagement.SceneManager.LoadScene("MainLevel"));
		}
	}
}