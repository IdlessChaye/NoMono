using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace NingyoRi
{
	public class MonoLocalizedText : MonoBehaviour
	{
		public Text text;
		public string key;

		// Use this for initialization
		void Start()
		{
			text.text = TextMapManager.instance.Get(key);
		}

	}
}