
using UnityEngine.SceneManagement;

namespace NingyoRi
{
	public abstract class BaseManager
	{
		public abstract ManagerType managerType { get; }

		public bool needTick { get;	private set; }

		public virtual void Init() { }

		public virtual void Setup() { }

		public virtual void Tick0(float deltaTime) { }

		public virtual void Tick1(float deltaTime) { }

		public virtual void Tick2(float deltaTime) { }

		public virtual void Tick3(float deltaTime) { }

		public virtual void Tick4(float deltaTime) { }

		public virtual void TickAddTo(float deltaTime) { }

		public virtual void OnLevelLoaded(Scene scene, LoadSceneMode loadSceneMode) { }

		public virtual void OnLevelUnLoaded(Scene scene) { } // 相当于Clear，在逻辑上的Destroy

		public virtual void Destroy() { }

		public void SetNeedTick(bool needTick)
		{
			this.needTick = needTick;
		}

	}

	// LocalManager 在Scene切换时会被Destroy
	public abstract class BaseLocalManager : BaseManager
	{
		
	}

	public abstract class BaseGlobalManager : BaseManager
	{

	}
}