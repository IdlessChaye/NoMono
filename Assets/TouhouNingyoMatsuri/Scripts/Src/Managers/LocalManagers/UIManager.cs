using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NingyoRi
{
	public partial class UIManager : BaseLocalManager
	{
		private GameObject _uiCanvasPrefab;
		private Transform _uiCanvasRoot;
		private Transform _pageContextRoot;
		private Transform _widgetContextRoot;

		private List<BaseContext> _contextList = new List<BaseContext>();
		private List<BaseContext> _tickContextList = new List<BaseContext>();

		public override void Init()
		{
			_uiCanvasPrefab = Miscs.GetResourceManager().Get<GameObject>(GlobalVars.uiCanvasPath);
			if (_uiCanvasPrefab == null)
				throw new System.Exception("UIManager Init");

			var _uiCanvasGO = GameObject.Find("UICanvas");
			_uiCanvasRoot = _uiCanvasGO != null ? _uiCanvasGO.transform : null;
			if (_uiCanvasRoot == null)
				_uiCanvasRoot = GameObject.Instantiate(_uiCanvasPrefab).transform;
			_pageContextRoot = _uiCanvasRoot.Find(@"Contexts/Pages");
			_widgetContextRoot = _uiCanvasRoot.Find(@"Contexts/Widgets");
		}

		public void ShowPage(BasePageContext context)
		{
			if (context == null)
				return;
			context.Show(_pageContextRoot);
			_contextList.Add(context);
			if (context.needTick)
				_tickContextList.Add(context);
		}

		public void ShowWidget(BaseWidgetContext context)
		{
			if (context == null)
				return;
			context.Show(_widgetContextRoot);
			_contextList.Add(context);
			if (context.needTick)
				_tickContextList.Add(context);
		}

		public void ClosePage(BasePageContext context)
		{
			if (context == null)
				return;
			context.Close();
			if (_tickContextList.Contains(context))
				_tickContextList.Remove(context);
			_contextList.Remove(context);
		}

		public void CloseWidget(BaseWidgetContext context)
		{
			if (context == null)
				return;
			context.Close();
			if (_tickContextList.Contains(context))
				_tickContextList.Remove(context);
			_contextList.Remove(context);
		}

		public override void Tick2()
		{
			for (int i = 0; i < _tickContextList.Count; i++)
			{
				var context = _tickContextList[i];
				if (context != null && context.needTick)
					context.Tick();
			}
		}


		public override void OnLevelLoaded(Scene scene, LoadSceneMode loadSceneMode)
		{
			if (scene.name.Equals("Menu"))
			{
				Miscs.GetUIManager().ShowPage(new MainMenuPageContext());
			}
		}

		public override void OnLevelUnLoaded(Scene scene)
		{
			if (_contextList != null)
			{
				foreach (var context in _contextList)
					context.Clear();
			}
		}

		public override void Destroy()
		{
			_uiCanvasRoot = null;
			_pageContextRoot = null;
			_widgetContextRoot = null;
			_tickContextList.Clear();
			if (_contextList != null)
			{ 
				foreach (var context in _contextList)
					context.Destroy();
			}
			_contextList.Clear();
		}

	}
}
