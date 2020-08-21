
namespace NingyoRi
{
	public class Singleton<T> where T : new()
	{
		private static T _instance = new T();

		public static T Instance { get { return _instance; } }
	}
}