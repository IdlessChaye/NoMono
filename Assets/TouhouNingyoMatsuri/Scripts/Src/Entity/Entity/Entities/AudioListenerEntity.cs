using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NingyoRi
{
	public partial class AudioListenerEntity : BaseEntity
	{
		private Transform _targetTF;
		public AudioListenerEntity(Transform transform)
		{
			_targetTF = transform;
		}

		protected override void Init()
		{
			_prefabPath = @"Entity/AudioListener";
		}

		public override void Setup()
		{
			if (_targetTF != null)
				GetGameObject().transform.SetParent(_targetTF, false);
		}
	}
}