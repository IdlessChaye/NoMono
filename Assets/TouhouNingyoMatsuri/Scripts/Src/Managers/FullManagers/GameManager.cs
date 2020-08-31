using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace NingyoRi
{
	public class GameManager : FullSingleton<GameManager>
	{
		private Dictionary<uint, BaseManager> _managerDict = new Dictionary<uint, BaseManager>(16);

		private LinkedList<BaseGlobalManager> _globalManagerLinList = new LinkedList<BaseGlobalManager>();
		private LinkedList<BaseLocalManager> _localManagerLinList = new LinkedList<BaseLocalManager>();

		private LinkedList<BaseGlobalManager> _tickGlobalManagerLinList = new LinkedList<BaseGlobalManager>();
		private LinkedList<BaseLocalManager> _tickLocalManagerLinList = new LinkedList<BaseLocalManager>();

		private LinkedList<BaseGlobalManager> _toBeAddedGlobalManagers = new LinkedList<BaseGlobalManager>(); // 在Tick中或开始时添加的Manager，需要在帧末尾添加到Tick队列
		private LinkedList<BaseLocalManager> _toBeAddedLocalManagers = new LinkedList<BaseLocalManager>(); // 初始化是在Add的那个帧时候完成，但Tick是在下个帧
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

			AddToGlobal(); // 因为只有一处会 AddGlobalManager 所以AddTo放到这里执行一次即可
		}

		public override void Setup()
		{
			AddFullManagers(); // 同级的(FullManager)放在Setup中增删

			//Messenger.Broadcast((uint)EventType.GameStart); // Test
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

			AddToLocal();

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
			var node = _globalManagerLinList.Last;
			BaseGlobalManager manager = null;
			while (node != null)
			{
				manager = node.Value;
				if (manager != null)
				{
					manager.OnLevelUnLoaded(scene);
				}
				node = node.Previous;
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
			_toBeAddedLocalManagers.Clear();
		}

		#endregion

		public void AddGlobalManager(BaseGlobalManager manager)
		{
			if (manager == null)
				return;
			if (_managerDict.ContainsKey((uint)manager.managerType))
				return;
			manager.Init();
			_toBeAddedGlobalManagers.AddLast(manager);
		}

		public void AddLocalManager(BaseLocalManager manager)
		{
			if (manager == null)
				return;
			if (_managerDict.ContainsKey((uint)manager.managerType))
				return;
			manager.Init();
			_toBeAddedLocalManagers.AddLast(manager);
		}

		#region Add To LinkedList
		private void AddToGlobal()
		{
			if (_toBeAddedGlobalManagers.Count == 0)
				return;

			var node = _toBeAddedGlobalManagers.First;
			BaseGlobalManager manager = null;
			while (node != null)
			{
				manager = node.Value;
				if (manager != null)
				{
					_globalManagerLinList.AddLast(manager);
					_managerDict.Add((uint)manager.managerType, manager);
					if (manager.needTick)
					{
						_tickGlobalManagerLinList.AddLast(manager);
					}
					manager.Setup();
				}
				node = node.Next;
			}

			_toBeAddedGlobalManagers.Clear();
		}

		private void AddToLocal()
		{
			if (_toBeAddedLocalManagers.Count == 0)
				return;

			var node = _toBeAddedLocalManagers.First;
			BaseLocalManager manager = null;
			while (node != null)
			{
				manager = node.Value;
				if (manager != null)
				{
					_localManagerLinList.AddLast(manager);
					_managerDict.Add((uint)manager.managerType, manager);
					if (manager.needTick)
					{
						_tickLocalManagerLinList.AddLast(manager);
					}
					manager.Setup();
				}
				node = node.Next;
			}

			_toBeAddedLocalManagers.Clear();
		}

		#endregion

		public override void Destroy() // Destroy 模拟 OnSceneUnloaded
		{
			var scene = SceneManager.GetActiveScene();

			OnSceneUnloaded(scene);

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
			_toBeAddedGlobalManagers.Clear();

			_managerDict.Clear();

			UnityEngine.Application.Quit();
		}


		private void Update()
		{
			float deltaTime = Time.deltaTime;

			#region Tick Global Manager

			var nodeGlobal = _tickGlobalManagerLinList.First;
			BaseGlobalManager managerGlobal = null;
			while (nodeGlobal != null)
			{
				managerGlobal = nodeGlobal.Value;
				if (managerGlobal != null && managerGlobal.needTick) // manager 的 isActive 一直都是 true，如果不是，则不是Manager，而是 Entity
				{
					managerGlobal.Tick0(deltaTime);
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
					managerGlobal.Tick1(deltaTime);
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
					managerGlobal.Tick2(deltaTime);
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
					managerGlobal.Tick3(deltaTime);
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
					managerGlobal.Tick4(deltaTime);
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
					managerLocal.Tick0(deltaTime);
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
					managerLocal.Tick1(deltaTime);
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
					managerLocal.Tick2(deltaTime);
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
					managerLocal.Tick3(deltaTime);
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
					managerLocal.Tick4(deltaTime);
				}
				nodeLocal = nodeLocal.Next;
			}

			#endregion
		}

		private void LateUpdate()
		{
			float deltaTime = Time.deltaTime;

			var nodeGlobal = _globalManagerLinList.First;
			BaseGlobalManager managerGlobal = null;
			while (nodeGlobal != null)
			{
				managerGlobal = nodeGlobal.Value;
				if (managerGlobal != null)
				{
					managerGlobal.TickAddTo(deltaTime);
				}
				nodeGlobal = nodeGlobal.Next;
			}

			var nodeLocal = _localManagerLinList.First;
			BaseLocalManager managerLocal = null;
			while (nodeLocal != null)
			{
				managerLocal = nodeLocal.Value;
				if (managerLocal != null)
				{
					managerLocal.TickAddTo(deltaTime);
				}
				nodeLocal = nodeLocal.Next;
			}

		}

		public T GetManager<T>(ManagerType type) where T : class
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


	}
}