using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NingyoRi
{
	public partial class AudioListenerEntity : BaseEntity
	{
		protected override string _prefabPath { get { return @"Entity/AudioListener"; } }

		private Transform _targetTF;
		public AudioListenerEntity(Transform transform)
		{
			_targetTF = transform;
		}

		protected override void Init()
		{
			if (_targetTF != null)
				GetGameObject().transform.SetParent(_targetTF, false);
		}

		public override void Setup()
		{
			
		}
	}
}