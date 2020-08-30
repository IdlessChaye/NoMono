using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NingyoRi
{
	[RequireComponent(typeof(Animator))]
	public class BaseMonoContext : MonoBehaviour
	{
		[SerializeField]
		protected Animator animator;

		[HideInInspector]
		public GameObject gameObjectContext { get; private set; }

		[HideInInspector]
		public BaseContext context { get; private set; }

		private void Awake()
		{
			gameObjectContext = this.gameObject;
			if (animator == null)
				animator = GetComponent<Animator>();
		}

		public virtual void Init(BaseContext context)
		{
			this.context = context;
		}

		public virtual void Show()
		{
			animator.SetTrigger("Show");
		}

		public virtual void Hide()
		{
			animator.SetTrigger("Hide");
		}
	}
}