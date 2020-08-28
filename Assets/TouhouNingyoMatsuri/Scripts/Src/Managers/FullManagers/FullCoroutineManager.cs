using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NingyoRi
{
	public class FullCoroutineManager : FullSingleton<FullCoroutineManager>
	{
		private List<IEnumerator> _coroutineList = new List<IEnumerator>();

		public override void Init()
		{
			gameObject.name = "FullCoroutineManager";
		}

		public void AddCoroutine(float delay, System.Action action)
		{
			var delayCorou = DelayCoroutine(delay, action);
			_coroutineList.Add(delayCorou);
			StartCoroutine(delayCorou);
		}

		private IEnumerator DelayCoroutine(float delay, System.Action action)
		{
			yield return new WaitForSeconds(delay);
			if (action != null)
				action();
		}

		public override void Destroy()
		{
			StopAllCoroutines();
			_coroutineList.Clear();
		}
	}
}