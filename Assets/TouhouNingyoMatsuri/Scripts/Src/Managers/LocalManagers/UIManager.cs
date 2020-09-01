using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NingyoRi
{
	public partial class UIManager : BaseLocalManager
	{
		private Transform _uiCanvasRoot;
		private Transform _pageContextRoot;
		private Transform _widgetContextRoot;

		//private Dictionary<uint, BaseContext> _contextDict = new Dictionary<uint, BaseContext>(4);

		private LinkedList<BaseContext> _contextLinList = new LinkedList<BaseContext>();
		private LinkedList<BaseContext> _tickContextLinList = new LinkedList<BaseContext>();
		private List<BaseContext> _toBeAddedContextList = new List<BaseContext>();

		private void ShowPages(Scene scene)
		{
			var sceneName = scene.name;
			if (sceneName.Equals("Menu"))
			{
				ShowPage(new MainMenuPageContext());
			}
		}
		public override void Init()
		{
			var _uiCanvasPrefab = Miscs.GetResourceManager().Get<GameObject>(GlobalVars.uiCanvasPath);
			if (_uiCanvasPrefab == null)
				throw new System.Exception("UIManager Init");

			var _uiCanvasGO = GameObject.Find("UICanvas");
			_uiCanvasRoot = _uiCanvasGO != null ? _uiCanvasGO.transform : null;
			if (_uiCanvasRoot == null)
				_uiCanvasRoot = GameObject.Instantiate(_uiCanvasPrefab).transform;
			_pageContextRoot = _uiCanvasRoot.Find(@"Contexts/Pages");
			_widgetContextRoot = _uiCanvasRoot.Find(@"Contexts/Widgets");

			SetNeedTick(true);
		}

		public void ShowPage(BasePageContext context)
		{
			if (context == null)
				return;
			isDirtyAdd = true;
			context.Load(_pageContextRoot);
			_toBeAddedContextList.Add(context);
		}

		public void ShowWidget(BaseWidgetContext context)
		{
			if (context == null)
				return;
			isDirtyAdd = true;
			context.Load(_widgetContextRoot);
			_toBeAddedContextList.Add(context);
		}

		public void ClosePage(BasePageContext context)
		{
			if (context == null)
				return;
			_tickContextLinList.Remove(context);
			_contextLinList.Remove(context);
			//_contextDict;
			context.Close();
		}

		public void CloseWidget(BaseWidgetContext context)
		{
			if (context == null)
				return;
			_tickContextLinList.Remove(context);
			_contextLinList.Remove(context);
			//_contextDict;
			context.Close();
		}

		public override void Tick2(float deltaTime)
		{
			var node = _tickContextLinList.First;
			BaseContext context = null;
			while (node != null)
			{
				context = node.Value;
				if (context != null && context.needTick)
				{
					context.Tick2(deltaTime);
				}
				node = node.Next;
			}
		}

		public override void TickAddTo()
		{
			if (isDirtyAdd == true)
			{
				isDirtyAdd = false;
				if (_toBeAddedContextList.Count != 0)
				{
					BaseContext context = null;
					for (int i = 0; i < _toBeAddedContextList.Count; i++)
					{
						context = _toBeAddedContextList[i];
						if (context != null)
						{
							_contextLinList.AddLast(context);
							//_contextDict.Add(context.);
							if (context.needTick) 
								_tickContextLinList.AddLast(context);
						}
					}

					for (int i = 0; i < _toBeAddedContextList.Count; i++)
					{
						context = _toBeAddedContextList[i];
						if (context != null)
							context.Setup();
					}

					_toBeAddedContextList.Clear();
				}
			}
		}

		public override void OnLevelLoaded(Scene scene, LoadSceneMode loadSceneMode)
		{
			ShowPages(scene);
		}

		public override void OnLevelUnLoaded(Scene scene)
		{
			var node = _contextLinList.Last;
			BaseContext context = null;
			while (node != null)
			{
				context = node.Value;
				if (context != null && context.needTick)
				{
					context.Close();
				}
				node = node.Previous;
			}

			_tickContextLinList.Clear();
			_contextLinList.Clear();
			// Dict
		}

		public override void Destroy()
		{
			_uiCanvasRoot = null;
			_pageContextRoot = null;
			_widgetContextRoot = null;
		}

	}
}
