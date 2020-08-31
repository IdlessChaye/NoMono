using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NingyoRi
{
	public static class Utils
	{
		public static void TrySetActive(GameObject go, bool isActive)
		{
			if (go == null)
				return;
			if (go.activeSelf != isActive)
				go.SetActive(isActive);
		}
	}
}