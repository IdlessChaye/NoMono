using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace NingyoRi
{
	public abstract class BaseContext
	{
		[HideInInspector]
		public GameObject contextGO;

		public bool needTick { get; set; }

		protected BaseMonoContext _monoContext;

		protected string _prefabPath { get; set; }

		public void Show()
		{
			Init();

			if (string.IsNullOrEmpty(_prefabPath))
				throw new System.Exception("UIManager ShowPage No PrefabPath.");
			GameObject prefab = Miscs.GetResourceManager().Get<GameObject>(_prefabPath);
			if (prefab == null)
				throw new System.Exception("UIManager ShowPage.");
			contextGO = GameObject.Instantiate(prefab);
			_monoContext = contextGO.GetComponent<BaseMonoContext>();
			if (_monoContext == null)
				throw new System.Exception("UIManager ShowPage No BaseMonoPageContext.");
			_monoContext.Show();
			CoroutineManager.Instance.AddCoroutine(0.5f, () => {
				Setup();
				SetupCallbacks();
				SetupEvents();
				});
		}

		public abstract void Init();

		public virtual void Setup() { }

		public virtual void SetupCallbacks() { }

		public virtual void SetupEvents() { }

		public virtual void Tick() { }

		public virtual void Clear() { }

		public virtual void Destroy() { }


		protected void BindCallback(Button button, UnityAction action)
		{
			if (action == null)
				return;
			button.onClick.AddListener(action	);
		}
	}
}