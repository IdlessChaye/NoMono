using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NingyoRi
{
	public class PlayerPlugin : BaseMonoPlugin
	{
		[SerializeField]
		private GameObject _alice;
		[SerializeField]
		private GameObject _marisa;
		[SerializeField]
		private GameObject _reimu;


		public GameObject alice { get { return _alice; } }
		public GameObject marisa { get { return _marisa; } }
		public GameObject reimu { get { return _reimu; } }

	}
}