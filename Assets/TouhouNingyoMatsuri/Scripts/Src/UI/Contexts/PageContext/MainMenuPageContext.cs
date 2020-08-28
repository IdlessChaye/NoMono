using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NingyoRi
{
	public class MainMenuPageContext : BasePageContext
	{
		private MonoMainMenuPage _mono;
		protected override void Init()
		{
			_prefabPath = @"UI/Contexts/PageContexts/MainMenuPage";
		}

		protected override void Setup()
		{
			base.Setup();
			_mono = _monoContext as MonoMainMenuPage;
			//needTick = false; // 需要在BaseContext里加一个needTick开关方法
		}

		protected override void SetupCallbacks()
		{
			base.SetupCallbacks();
			BindCallback(_mono.startButton, () => Debug.Log("Hello"));
		}
	}
}