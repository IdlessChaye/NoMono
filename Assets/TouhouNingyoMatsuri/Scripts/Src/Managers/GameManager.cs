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
		private Dictionary<ManagerType, BaseManager> _managerDict = new Dictionary<ManagerType, BaseManager>(16);

		private List<BaseGlobalManager> _tickGlobalManagerList = new List<BaseGlobalManager>(4);
		private List<BaseLocalManager> _tickLocalManagerList = new List<BaseLocalManager>(4);

		public override void Awake()
		{
			base.Awake();
			GameManager.Instance.Init();

			CoroutineManager.Instance.Init();
		}

		public override void Init()
		{
			AddGlobalManager(new ResourceManager());
			AddGlobalManager(new TextMapManager());
			AddGlobalManager(new InputManager());

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
			if (manager.needTick)
				_tickGlobalManagerList.Add(manager);
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
			if (manager.needTick)
				_tickLocalManagerList.Add(manager);
			_managerDict.Add(manager.managerType, manager);
		}

		public T GetManager<T>(ManagerType type) where T: class
		{
			if (_managerDict.ContainsKey(type) == false)
				return null;
			return _managerDict[type] as T;
		}

		public void QuitGame()
		{
			CoroutineManager.Instance.Destroy();
			this.Destroy();
		}

		public override void Destroy()
		{
			for (int i = _localManagerList.Count - 1; i >= 0; i--)
			{
				var manager = _localManagerList[i];
				if (manager != null)
				{
					manager.Destroy();
					if (_managerDict.ContainsKey(manager.managerType))
						_managerDict.Remove(manager.managerType);
				}
			}

			_localManagerList.Clear();
			_tickLocalManagerList.Clear();

			for (int i = _globalManagerList.Count - 1; i >= 0; i--)
			{
				var manager = _globalManagerList[i];
				if (manager != null)
				{
					manager.Destroy();
					if (_managerDict.ContainsKey(manager.managerType))
						_managerDict.Remove(manager.managerType);
				}
			}

			_globalManagerList.Clear();
			_tickGlobalManagerList.Clear();

			_managerDict.Clear();

			UnityEngine.Application.Quit();
		}

		#region On Scene Change

		private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
		{
			OnGlobalManagerLoaded(scene, loadSceneMode);
			OnLocalManagerLoaded(scene, loadSceneMode);
		}

		private void OnSceneUnloaded(Scene scene)
		{
			OnGlobalManagerUnloaded(scene);
			OnLocalManagerUnloaded(scene);
		}

		private void OnGlobalManagerLoaded(Scene scene, LoadSceneMode loadSceneMode)
		{

		}

		private void OnLocalManagerLoaded(Scene scene, LoadSceneMode loadSceneMode)
		{
			var sceneName = scene.name;
			if (sceneName.Equals("Menu"))
			{
				AddLocalManager(new UIManager());
			}
			else if (sceneName.Equals("MainLevel"))
			{
				AddLocalManager(new UIManager());
				AddLocalManager(new EntityManager());
			}

			for (int i = _localManagerList.Count - 1; i >= 0; i--)
			{
				BaseLocalManager manager = _localManagerList[i];
				if (manager != null)
					manager.OnLevelLoaded(scene, loadSceneMode);
			}
		}

		private void OnGlobalManagerUnloaded(Scene scene)
		{

		}

		private void OnLocalManagerUnloaded(Scene scene)
		{
			for (int i = _localManagerList.Count - 1; i >= 0; i--)
			{
				var manager = _localManagerList[i];
				if (manager != null)
					manager.OnLevelUnLoaded(scene);
			}

			for (int i = _localManagerList.Count - 1; i >= 0; i--)
			{
				var manager = _localManagerList[i];
				if (manager != null)
				{
					manager.Destroy();
					if (_managerDict.ContainsKey(manager.managerType))
						_managerDict.Remove(manager.managerType);
				}
			}

			_localManagerList.Clear();
			_tickLocalManagerList.Clear();
		}

		#endregion

		private void Update()
		{
			#region Tick Global Manager

			for (int i = _tickGlobalManagerList.Count - 1; i >= 0; i--)
			{
				BaseManager manager = _tickGlobalManagerList[i];
				if (manager != null)
					manager.Tick0();
			}

			for (int i = _tickGlobalManagerList.Count - 1; i >= 0; i--)
			{
				BaseManager manager = _tickGlobalManagerList[i];
				if (manager != null)
					manager.Tick1();
			}

			for (int i = _tickGlobalManagerList.Count - 1; i >= 0; i--)
			{
				BaseManager manager = _tickGlobalManagerList[i];
				if (manager != null)
					manager.Tick2();
			}

			for (int i = _tickGlobalManagerList.Count - 1; i >= 0; i--)
			{
				BaseManager manager = _tickGlobalManagerList[i];
				if (manager != null)
					manager.Tick3();
			}

			for (int i = _tickGlobalManagerList.Count - 1; i >= 0; i--)
			{
				BaseManager manager = _tickGlobalManagerList[i];
				if (manager != null)
					manager.Tick4();
			}

			#endregion

			#region Tick Local Manager

			for (int i = _tickLocalManagerList.Count - 1; i >= 0; i--)
			{
				BaseManager manager = _tickLocalManagerList[i];
				if (manager != null)
					manager.Tick0();
			}

			for (int i = _tickLocalManagerList.Count - 1; i >= 0; i--)
			{
				BaseManager manager = _tickLocalManagerList[i];
				if (manager != null)
					manager.Tick1();
			}

			for (int i = _tickLocalManagerList.Count - 1; i >= 0; i--)
			{
				BaseManager manager = _tickLocalManagerList[i];
				if (manager != null)
					manager.Tick2();
			}

			for (int i = _tickLocalManagerList.Count - 1; i >= 0; i--)
			{
				BaseManager manager = _tickLocalManagerList[i];
				if (manager != null)
					manager.Tick3();
			}

			for (int i = _tickLocalManagerList.Count - 1; i >= 0; i--)
			{
				BaseManager manager = _tickLocalManagerList[i];
				if (manager != null)
					manager.Tick4();
			}

			#endregion
		}
	}
}