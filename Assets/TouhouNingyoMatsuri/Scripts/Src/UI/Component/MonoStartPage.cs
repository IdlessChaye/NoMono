using UnityEngine;
using UnityEngine.SceneManagement;

namespace NingyoRi
{
	public class MonoStartPage : MonoBehaviour
	{
		public void OnLoadMenuScene()
		{
			SceneManager.LoadScene(1);
		}
	}
}