using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace NingyoRi
{
	public abstract class BaseContext
	{
		public bool needTick { get; private set; }

		protected string _prefabPath { get; set; }
		protected GameObject _gameObject { get; private set; }
		protected BaseMonoContext _monoContext { get; private set; }


		private List<UnityEvent> _unityEventList;

		public void Load(Transform pageContextRoot)
		{
			Init();

			if (string.IsNullOrEmpty(_prefabPath))
				throw new System.Exception("UIManager ShowPage No PrefabPath.");
			GameObject prefab = Miscs.GetResourceManager().Get<GameObject>(_prefabPath);
			if (prefab == null)
				throw new System.Exception("UIManager ShowPage.");
			_gameObject = GameObject.Instantiate(prefab);
			_gameObject.transform.SetParent(pageContextRoot, false);
			_monoContext = _gameObject.GetComponent<BaseMonoContext>();
			if (_monoContext == null)
				throw new System.Exception("UIManager ShowPage No BaseMonoPageContext.");
			_monoContext.Init(this);
		}

		public virtual void Setup()
		{
			if (_unityEventList == null)
				_unityEventList = new List<UnityEvent>(4);

			_monoContext.Show();
			FullCoroutineManager.Instance.AddCoroutine(0.5f, () => { // 注意：这0.5s之内Context接收不到Callback，先这样，以后也许会有Bug
				SetupCallbacks();
				SetupEvents();
			});
		}

		protected abstract void Init(); // 设置 _prefabPath

		protected virtual void SetupCallbacks()
		{

		}

		protected virtual void SetupEvents()
		{

		}

		public virtual void Tick2(float deltaTime) { }

		public void Close()
		{
			Clear();
			_monoContext.Hide();
			FullCoroutineManager.Instance.AddCoroutine(0.5f, () => {
				Destroy();
			});
		}

		protected virtual void Clear()
		{
			ClearAllUnityEvents();
			_unityEventList = null;
			needTick = false;
		}

		protected virtual void Destroy()
		{
			if (_monoContext != null)
			{
				GameObject.Destroy(_monoContext.gameObjectContext);
			}
		}

		private void ClearAllUnityEvents()
		{
			if (_unityEventList == null)
				return;
			foreach (var unityEvent in _unityEventList)
			{
				unityEvent.RemoveAllListeners();
			}
			_unityEventList.Clear();
		}

		protected void BindCallback(Button button, UnityAction action)
		{
			if (action == null)
				return;
			_unityEventList.Add(button.onClick);
			button.onClick.AddListener(action);
		}

	}
}