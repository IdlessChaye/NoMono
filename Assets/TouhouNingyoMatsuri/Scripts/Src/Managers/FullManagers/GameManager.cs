using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;

namespace NingyoRi
{
	public class GameManager : FullSingleton<GameManager>
	{
		private Dictionary<uint, BaseManager> _managerDict = new Dictionary<uint, BaseManager>(16);

		private LinkedList<BaseGlobalManager> _globalManagerLinList = new LinkedList<BaseGlobalManager>();
		private LinkedList<BaseLocalManager> _localManagerLinList = new LinkedList<BaseLocalManager>();

		private LinkedList<BaseGlobalManager> _tickGlobalManagerLinList = new LinkedList<BaseGlobalManager>();
		private LinkedList<BaseLocalManager> _tickLocalManagerLinList = new LinkedList<BaseLocalManager>();

		private LinkedList<BaseGlobalManager> _toBeAddedTickGlobalManagers = new LinkedList<BaseGlobalManager>(); // 在Tick中或开始时添加的Manager，需要在帧末尾添加到Tick队列
		private LinkedList<BaseLocalManager> _toBeAddedTickLocalManagers = new LinkedList<BaseLocalManager>(); // 初始化是在Add的那个帧时候完成，但Tick是在下个帧
		private void AddFullManagers()
		{
			FullCoroutineManager.Instance.Init();
			FullMusicManager.Instance.Init();
		}

		private void AddGlobalManagers()
		{
			AddGlobalManager(new ResourceManager());
			AddGlobalManager(new TextMapManager());
			AddGlobalManager(new InputManager());
		}

		private void AddLocalManagers(Scene scene)
		{
			var sceneName = scene.name;
			if (sceneName.Equals("Menu"))
			{
				AddLocalManager(new EntityManager());
				AddLocalManager(new UIManager());
			}
			else if (sceneName.Equals("MainLevel"))
			{
				AddLocalManager(new EntityManager());
				AddLocalManager(new UIManager());
			}
		}

		private void RemoveFullManagers()
		{
			FullMusicManager.Instance.Destroy();
			FullCoroutineManager.Instance.Destroy();
		}

		public override void Awake()
		{
			base.Awake();

			SceneManager.sceneLoaded += OnSceneLoaded;
			SceneManager.sceneUnloaded += OnSceneUnloaded;

			GameManager.Instance.Init();
			GameManager.Instance.Setup();
		}

		public override void Init()
		{
			AddGlobalManagers(); // 下一级的(GlobalManager\LocalManager)放在Init中增删
		}

		public override void Setup()
		{
			AddFullManagers(); // 同级的(FullManager)放在Setup中增删

			Messenger.Broadcast((uint)EventType.GameStart);
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
			var node = _globalManagerLinList.First;
			BaseGlobalManager manager = null;
			while (node != null)
			{
				manager = node.Value;
				if (manager != null)
				{
					manager.OnLevelLoaded(scene, loadSceneMode);
				}
				node = node.Next;
			}
		}

		private void OnLocalManagerLoaded(Scene scene, LoadSceneMode loadSceneMode)
		{
			AddLocalManagers(scene);

			var node = _localManagerLinList.First;
			BaseLocalManager manager = null;
			while(node != null)
			{
				manager = node.Value;
				if (manager != null)
				{
					manager.OnLevelLoaded(scene, loadSceneMode);
				}
				node = node.Next;
			}
		}

		private void OnGlobalManagerUnloaded(Scene scene)
		{
			var node = _globalManagerLinList.First;
			BaseGlobalManager manager = null;
			while (node != null)
			{
				manager = node.Value;
				if (manager != null)
				{
					manager.OnLevelUnLoaded(scene);
				}
				node = node.Next;
			}
		}

		private void OnLocalManagerUnloaded(Scene scene)
		{
			var node = _localManagerLinList.Last;
			BaseLocalManager manager = null;
			while (node != null)
			{
				manager = node.Value;
				if (manager != null)
				{
					manager.OnLevelUnLoaded(scene);
				}
				node = node.Previous;
			}

			node = _localManagerLinList.Last;
			manager = null;
			while (node != null)
			{
				manager = node.Value;
				if (manager != null)
				{
					manager.Destroy();
					_managerDict.Remove((uint)manager.managerType);
				}
				node = node.Previous;
			}

			_localManagerLinList.Clear();
			_tickLocalManagerLinList.Clear();
			_toBeAddedTickLocalManagers.Clear();
		}

		#endregion

		public void AddGlobalManager(BaseGlobalManager manager)
		{
			if (manager == null)
				return;
			if (_managerDict.ContainsKey((uint)manager.managerType))
				return;
			manager.Init();
			_globalManagerLinList.AddLast(manager);
			if (manager.needTick)
				_toBeAddedTickGlobalManagers.AddLast(manager);
			_managerDict.Add((uint)manager.managerType, manager);
			manager.Setup();
		}

		public void AddLocalManager(BaseLocalManager manager)
		{
			if (manager == null)
				return;
			if (_managerDict.ContainsKey((uint)manager.managerType))
				return;
			manager.Init();
			_localManagerLinList.AddLast(manager);
			if (manager.needTick)
				_toBeAddedTickLocalManagers.AddLast(manager);
			_managerDict.Add((uint)manager.managerType, manager);
			manager.Setup();
		}

		public T GetManager<T>(ManagerType type) where T: class
		{
			BaseManager manager;
			if (_managerDict.TryGetValue((uint)type, out manager) == false)
				return null;
			return manager as T;
		}

		public void QuitGame()
		{
			RemoveFullManagers();

			this.Destroy();
		}

		public override void Destroy()
		{
			var node = _localManagerLinList.Last;
			BaseLocalManager manager = null;
			while (node != null)
			{
				manager = node.Value;
				if (manager != null)
				{
					manager.Destroy();
					_managerDict.Remove((uint)manager.managerType);
				}
				node = node.Previous;
			}

			_localManagerLinList.Clear();
			_tickLocalManagerLinList.Clear();
			_toBeAddedTickLocalManagers.Clear();

			var nodee = _globalManagerLinList.Last;
			BaseGlobalManager managerr = null;
			while (nodee != null)
			{
				managerr = nodee.Value;
				if (managerr != null)
				{
					managerr.Destroy();
					_managerDict.Remove((uint)managerr.managerType);
				}
				nodee = nodee.Previous;
			}

			_globalManagerLinList.Clear();
			_tickGlobalManagerLinList.Clear();
			_toBeAddedTickGlobalManagers.Clear();

			_managerDict.Clear();

			UnityEngine.Application.Quit();
		}


		private void Update()
		{
			#region Tick Global Manager

			var nodeGlobal = _tickGlobalManagerLinList.First;
			BaseGlobalManager managerGlobal = null;
			while (nodeGlobal != null)
			{
				managerGlobal = nodeGlobal.Value;
				if (managerGlobal != null && managerGlobal.needTick) // manager 的 isActive 一直都是 true，如果不是，则不是Manager，而是 Entity
				{
					managerGlobal.Tick0();
				}
				nodeGlobal = nodeGlobal.Next;
			}

			nodeGlobal = _tickGlobalManagerLinList.First;
			managerGlobal = null;
			while(nodeGlobal != null)
			{
				managerGlobal = nodeGlobal.Value;
				if (managerGlobal != null && managerGlobal.needTick)
				{
					managerGlobal.Tick1();
				}
				nodeGlobal = nodeGlobal.Next;
			}

			nodeGlobal = _tickGlobalManagerLinList.First;
			managerGlobal = null;
			while (nodeGlobal != null)
			{
				managerGlobal = nodeGlobal.Value;
				if (managerGlobal != null && managerGlobal.needTick)
				{
					managerGlobal.Tick2();
				}
				nodeGlobal = nodeGlobal.Next;
			}

			nodeGlobal = _tickGlobalManagerLinList.First;
			managerGlobal = null;
			while (nodeGlobal != null)
			{
				managerGlobal = nodeGlobal.Value;
				if (managerGlobal != null && managerGlobal.needTick)
				{
					managerGlobal.Tick3();
				}
				nodeGlobal = nodeGlobal.Next;
			}

			nodeGlobal = _tickGlobalManagerLinList.First;
			managerGlobal = null;
			while (nodeGlobal != null)
			{
				managerGlobal = nodeGlobal.Value;
				if (managerGlobal != null && managerGlobal.needTick)
				{
					managerGlobal.Tick4();
				}
				nodeGlobal = nodeGlobal.Next;
			}

			#endregion

			#region Tick Local Manager

			var nodeLocal = _tickLocalManagerLinList.First;
			BaseLocalManager managerLocal = null;
			while (nodeLocal != null)
			{
				managerLocal = nodeLocal.Value;
				if (managerLocal != null && managerLocal.needTick) // manager 的 isActive 一直都是 true，如果不是，则不是Manager，而是 Entity
				{
					managerLocal.Tick0();
				}
				nodeLocal = nodeLocal.Next;
			}

			nodeLocal = _tickLocalManagerLinList.First;
			managerLocal = null;
			while(nodeLocal != null)
			{
				managerLocal = nodeLocal.Value;
				if (managerLocal != null && managerLocal.needTick)
				{
					managerLocal.Tick1();
				}
				nodeLocal = nodeLocal.Next;
			}

			nodeLocal = _tickLocalManagerLinList.First;
			managerLocal = null;
			while (nodeLocal != null)
			{
				managerLocal = nodeLocal.Value;
				if (managerLocal != null && managerLocal.needTick)
				{
					managerLocal.Tick2();
				}
				nodeLocal = nodeLocal.Next;
			}

			nodeLocal = _tickLocalManagerLinList.First;
			managerLocal = null;
			while (nodeLocal != null)
			{
				managerLocal = nodeLocal.Value;
				if (managerLocal != null && managerLocal.needTick)
				{
					managerLocal.Tick3();
				}
				nodeLocal = nodeLocal.Next;
			}

			nodeLocal = _tickLocalManagerLinList.First;
			managerLocal = null;
			while (nodeLocal != null)
			{
				managerLocal = nodeLocal.Value;
				if (managerLocal != null && managerLocal.needTick)
				{
					managerLocal.Tick4();
				}
				nodeLocal = nodeLocal.Next;
			}

			#endregion
		}

		private void LateUpdate()
		{
			AddToTick();
		}

		#region Add To Tick LinkedList
		private void AddToTick()
		{
			AddToTickGlobal();
			AddToTickLocal();
		}
		private void AddToTickGlobal()
		{
			var node = _toBeAddedTickGlobalManagers.First;
			BaseGlobalManager manager = null;
			while(node != null)
			{
				manager = node.Value;
				if (manager != null && manager.needTick)
				{
					_tickGlobalManagerLinList.AddLast(manager);
				}
				node = node.Next;
			}

			_toBeAddedTickGlobalManagers.Clear();
		}

		private void AddToTickLocal()
		{
			var node = _toBeAddedTickLocalManagers.First;
			BaseLocalManager manager = null;
			while (node != null)
			{
				manager = node.Value;
				if (manager != null && manager.needTick)
				{
					_tickLocalManagerLinList.AddLast(manager);
				}
				node = node.Next;
			}

			_toBeAddedTickLocalManagers.Clear();
		}

		#endregion

	}
}