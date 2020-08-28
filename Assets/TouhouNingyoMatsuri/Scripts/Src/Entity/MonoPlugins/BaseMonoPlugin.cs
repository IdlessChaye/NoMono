using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NingyoRi
{
	public class BaseMonoPlugin : MonoBehaviour
	{
		[HideInInspector]
		public GameObject pluginGO;

		private void Awake()
		{
			pluginGO = this.gameObject;
		}
	}
}