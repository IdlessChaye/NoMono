using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NingyoRi
{
	public class TestForList : MonoBehaviour
	{
		void Start()
		{
			List<int> a = new List<int>();
			a.Add(1);
			for (int i= 0; i < a.Count; i++)
			{
				if (i < 10)
					a.Add(i);
				Debug.Log(i + "asd " + a[i]);
			}
		}
	}
}