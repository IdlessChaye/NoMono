using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NingyoRi
{
	public class UIManager : BaseLocalManager {
		public override ManagerType managerType
		{
			get
			{
				return ManagerType.UI;
			}
		}

		public void ShowPage(BasePageContext context)
		{
			string prefabPath = context.PrefabPath;
			GameObject prefab = GameManager.Instance.GetManager<ResourceManager>(ManagerType.Resource).Get<GameObject>(prefabPath);
			if (prefab == null)
				throw new System.Exception("UIManager ShowPage.");
			BaseMonoPageContext mono = prefab.GetComponent<BaseMonoPageContext>();
			if (mono == null)
				throw new System.Exception("UIManager ShowPage No BaseMonoPageContext.");
			context.Init();
			context.Setup();
			mono.ShowPage();
		}

		public void ShowWidget()
		{

		}


	}
}
