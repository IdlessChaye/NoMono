using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NingyoRi
{
	public enum LanguageType
	{
		Chinese,
		Japanese,
		English
	}

	public partial class TextMapManager : BaseGlobalManager
	{
		public static TextMapManager instance;
		public TextMapManager() : base()
		{
			instance = this;
		}


		private const string PATH = @"TextMap/TextMap";

		private List<string> _keyList;
		private List<string> _chineseList;
		private List<string> _japaneseList;
		private List<string> _englishList;

		public override void Init()
		{
			LoadTextMapData(PATH);
		}

		private void LoadTextMapData(string path)
		{
			string[] textmapItems = GetTextMapItems(path);
			if (textmapItems == null)
				throw new System.Exception("TextMap Load failed.");
			int length = textmapItems.Length;
			if (length == 0)
				return;
			if (_keyList == null)
				_keyList = new List<string>(length);
			if (_chineseList == null)
				_chineseList = new List<string>(length);
			if (_japaneseList == null)
				_japaneseList = new List<string>(length);
			if (_englishList == null)
				_englishList = new List<string>(length);
			_keyList.Clear();
			_chineseList.Clear();
			_japaneseList.Clear();
			_englishList.Clear();
			for (int i = 0; i < length; i++)
			{
				string item = textmapItems[i];
				string[] texts = item.Split('\t');
				if (texts.Length != 4)
					throw new System.Exception("Parse TextMap Failed! key: " + texts != null && texts.Length != 0 ? texts[0] : "");
				_keyList.Add(texts[0]);
				_chineseList.Add(texts[1]);
				_japaneseList.Add(texts[2]);
				_englishList.Add(texts[3]);
			}
		}


		private string[] GetTextMapItems(string textmapPath)
		{
			string textmap = Resources.Load<TextAsset>(textmapPath).text as string;
			if (string.IsNullOrEmpty(textmap))
				return null;
			textmap = textmap.Replace("\r\n", "\n");
			return textmap.Split('\n');
		}

		public string Get(string key)
		{
			if (_keyList == null || _keyList.Contains(key) == false)
				return null;

			string text = null;
			int index = _keyList.IndexOf(key);
			switch (PlayerDataItem.Instance.languageType)
			{
				case LanguageType.Chinese:
					text = _chineseList != null ? _chineseList[index] : null;
					break;
				case LanguageType.Japanese:
					text = _japaneseList != null ? _japaneseList[index] : null;
					break;
				case LanguageType.English:
					text = _englishList != null ? _englishList[index] : null;
					break;
			}

			if (text == null)
				throw new System.Exception("Textmap Get Error!");

			return text;
		}
	}
}