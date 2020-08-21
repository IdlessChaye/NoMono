using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NingyoRi
{

	public class ResourceManager : BaseGlobalManager
	{
		public override ManagerType managerType
		{
			get
			{
				return ManagerType.Resource;
			}
		}

		private Dictionary<string, Object> _prefabDict = new Dictionary<string, Object>();

		public T Get<T>(string path) where T : Object
		{
			if (string.IsNullOrEmpty(path))
				return null;
			Object go = null;
			if (_prefabDict.ContainsKey(path))
			{
				go = _prefabDict[path];
				if (go == null)
				{ 
					go = Resources.Load<T>(path) as T;
					_prefabDict[path] = go;
 				}
			}
			else
			{ 
				go = Resources.Load<T>(path) as T;
				_prefabDict[path] = go;
			}
			return go as T;
		}
	}
}