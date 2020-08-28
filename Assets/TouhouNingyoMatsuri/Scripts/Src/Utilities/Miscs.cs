﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NingyoRi
{
	public static class Miscs
	{
		public static string TextMapGet(string key)
		{
			return GameManager.Instance.GetManager<TextMapManager>(ManagerType.TextMap).Get(key);
		}

		public static UIManager GetUIManager()
		{
			return GameManager.Instance.GetManager<UIManager>(ManagerType.UI);
		}

		public static ResourceManager GetResourceManager()
		{
			return GameManager.Instance.GetManager<ResourceManager>(ManagerType.Resource);
		}
	}
}