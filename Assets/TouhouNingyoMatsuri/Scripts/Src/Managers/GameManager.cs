using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;

namespace NingyoRi
{
	public class GameManager : FullSingleton<GameManager>
	{
		private List<BaseGlobalManager> _globalManagerList = new List<BaseGlobalManager>(8);
		private List<BaseLocalManager> _localManagerList = new List<BaseLocalManager>(8);
		private List<BaseManager> _managerList = new List<BaseManager>(16);
		private Dictionary<ManagerType, BaseManager> _managerDict = new Dictionary<ManagerType, BaseManager>(16);

		public override void Awake()
		{
			base.Awake();
			GameManager.Instance.Init();
		}

		public override void Init()
		{
			AddGlobalManager(new TextMapManager());
			AddGlobalManager(new InputManager());
			AddGlobalManager(new ResourceManager());

			AddLocalManager(new UIManager());
			AddLocalManager(new EntityManager());

			SceneManager.sceneLoaded += OnSceneLoaded;
			SceneManager.sceneUnloaded += OnSceneUnloaded;
		}

		private void AddGlobalManager(BaseGlobalManager manager)
		{
			if (manager == null)
				return;
			if (_globalManagerList.Contains(manager))
				return;
			if (_managerDict.ContainsKey(manager.managerType))
				return;
			manager.Init();
			_globalManagerList.Add(manager);
			_managerList.Add(manager);
			_managerDict.Add(manager.managerType, manager);
		}

		private void AddLocalManager(BaseLocalManager manager)
		{
			if (manager == null)
				return;
			if (_localManagerList.Contains(manager))
				return;
			if (_managerDict.ContainsKey(manager.managerType))
				return;
			manager.Init();
			_localManagerList.Add(manager);
			_managerList.Add(manager);
			_managerDict.Add(manager.managerType, manager);
		}

		public T GetManager<T>(ManagerType type) where T: class
		{
			if (_managerDict.ContainsKey(type) == false)
				return null;
			return _managerDict[type] as T;
		}

		private void OnSceneUnloaded(Scene scene)
		{
			for(int i = _localManagerList.Count - 1; i >= 0; i--)
			{
				BaseLocalManager manager = _localManagerList[i];
				if (manager != null)
					manager.OnLevelUnLoaded(scene);
			}
		}

		private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
		{
			for (int i = _localManagerList.Count - 1; i >= 0; i--)
			{
				BaseLocalManager manager = _localManagerList[i];
				if (manager != null)
					manager.OnLevelLoaded(scene, loadSceneMode);
			}
		}

		private void Update()
		{
			for(int i = _managerList.Count - 1; i >= 0; i--)
			{
				BaseManager manager = _managerList[i];
				if (manager != null)
					manager.Tick0();
			}

			for (int i = _managerList.Count - 1; i >= 0; i--)
			{
				BaseManager manager = _managerList[i];
				if (manager != null)
					manager.Tick1();
			}

			for (int i = _managerList.Count - 1; i >= 0; i--)
			{
				BaseManager manager = _managerList[i];
				if (manager != null)
					manager.Tick2();
			}

			for (int i = _managerList.Count - 1; i >= 0; i--)
			{
				BaseManager manager = _managerList[i];
				if (manager != null)
					manager.Tick3();
			}

			for (int i = _managerList.Count - 1; i >= 0; i--)
			{
				BaseManager manager = _managerList[i];
				if (manager != null)
					manager.Tick4();
			}
		}
	}
}