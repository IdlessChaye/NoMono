using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NingyoRi
{
	public class BaseMonoPlugin : MonoBehaviour
	{
		[HideInInspector]
		public GameObject gameObjectPlugin { get; private set; }
		[HideInInspector]
		public BaseEntity entity { get; private set; }

		private void Awake()
		{
			gameObjectPlugin = this.gameObject;
		}

		public virtual void Init(BaseEntity entity)
		{
			this.entity = entity;
		}
	}
}