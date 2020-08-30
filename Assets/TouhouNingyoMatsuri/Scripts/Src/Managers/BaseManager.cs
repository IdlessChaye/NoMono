
using UnityEngine.SceneManagement;

namespace NingyoRi
{
	public abstract class BaseManager
	{
		public abstract ManagerType managerType { get; }

		public bool needTick { get;	private set; }

		public virtual void Init() { }


		public virtual void Tick0() { }

		public virtual void Tick1() { }

		public virtual void Tick2() { }

		public virtual void Tick3() { }

		public virtual void Tick4() { }


		public virtual void Destroy() { }

		public virtual void OnLevelLoaded(Scene scene, LoadSceneMode loadSceneMode) { }

		public virtual void OnLevelUnLoaded(Scene scene) { }

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