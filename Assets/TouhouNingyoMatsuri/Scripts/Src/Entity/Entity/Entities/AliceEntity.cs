using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NingyoRi
{
	public partial class PlayerEntity : CharacterEntity
	{
		protected override void Init()
		{
			_prefabPath = @"Entity/Players/Alice";
			SetNeedTick(true);
		}

		protected override void Setup()
		{
			var rawPlayerCtrl = new LCRawPlayerController();
			rawPlayerCtrl.Init(this);
			AddComponent(rawPlayerCtrl);
		}
	}
}