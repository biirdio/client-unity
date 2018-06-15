using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Biird
{
	public class BiirdClient : MonoBehaviour
	{
		private const string Url = "https://api.biird.io/resourceValue/";
		private BiirdIds _ids;
		private Dictionary<string, string> _items;
		private BiirdDatabase _biirdDatabase;
		private int _contentCounter;

		private static BiirdClient _biird;
		private static string _selectedLanguage;

#if NET_4_6
		public Dictionary<string, string> Items => _items;
#else
		public Dictionary<string, string> Items
		{
			get { return _items; }
		}
#endif
		public delegate void BiirdInitialized();

		public event BiirdInitialized OnBiirdInitialized;

		#region Setup Biird

		/// <summary>
		/// Initialize Biird Client
		/// </summary>
		/// <returns>Biird object</returns>
		public static void Init()
		{
			_biird = FindObjectOfType<BiirdClient>();
			if (_biird == null)
			{
				var gameObject = new GameObject("Biird");
				_biird = gameObject.AddComponent<BiirdClient>();
				SelectLanguage();
				DontDestroyOnLoad(gameObject);
			}
		}

		/// <summary>
		/// Returns instance of biird client. Client should be initialized before use this function. 
		/// </summary>
		/// <returns></returns>
		/// <exception cref="Exception">Client cannot be null. Use Init() to initialize Biird Client.</exception>
		public static BiirdClient GetBiird()
		{
			if (_biird != null)
			{
				return _biird;
			}

			throw new Exception("Biird Client is not initialized");
		}

		#if NET_4_6
		public static string SelectedLanguage => _selectedLanguage;
		#else
		public static string SelectedLanguage
		{
			get { return _selectedLanguage; }
		}
		#endif

		/// <summary>
		/// Start to load all needed data.
		/// </summary>
		/// <exception cref="Exception">OnBiirdInitialized isn't subscribed</exception>
		public void LoadData()
		{
			//todo add version checking
			if (!PlayerPrefs.HasKey("BiirdLocalizationData"))
			{
				_contentCounter = 0;
				_biirdDatabase = new BiirdDatabase();
				_items = new Dictionary<string, string>();
				_ids = BiirdIds.Instance;
				_ids.Init();
				_biirdDatabase.BiirdItems = new List<BiirdItem>();
				StartCoroutine(DownloadData());
			}
			else
			{
				_items = new Dictionary<string, string>();
				string json = PlayerPrefs.GetString("BiirdLocalizationData");
				_biirdDatabase = JsonUtility.FromJson<BiirdDatabase>(json);
				SetDictionary();
				if (OnBiirdInitialized != null)
				{
					OnBiirdInitialized.Invoke();
				}
				else
				{
					throw new Exception(
						"OnBiirdInitialized should have any subscription before call.\nUse BiirdClient.GetBiird().OnBiirdInitialized+=YOUR_FUNCTION();");
				}
			}

		}

		private IEnumerator DownloadData()
		{
			yield return null;
			foreach (var keypair in _ids.AllIds)
			{
				ReceiveTheText(keypair, RegisterDownloadedData);
			}

			yield return new WaitUntil(() => _contentCounter == _ids.AllIds.Count);

			SetDictionary();

			string jsonData = JsonUtility.ToJson(_biirdDatabase);
			Debug.Log(jsonData);
			PlayerPrefs.SetString("BiirdLocalizationData", jsonData);
			PlayerPrefs.Save();
			if (OnBiirdInitialized != null)
			{
				OnBiirdInitialized.Invoke();
			}
			else
			{
				throw new Exception(
					"OnBiirdInitialized should have any subscription before call.\nUse BiirdClient.GetBiird().OnBiirdInitialized+=YOUR_FUNCTION();");
			}
		}

		private void SetDictionary()
		{
			foreach (var biirdItem in _biirdDatabase.BiirdItems)
			{
				_items.Add(biirdItem.Key, biirdItem.Value);
			}
		}

		private void RegisterDownloadedData(string key, string value)
		{
			_biirdDatabase.BiirdItems.Add(new BiirdItem(key, value));
			_contentCounter++;
		}

		/// <summary>
		/// Set current language
		/// </summary>
		private static void SelectLanguage()
		{
			switch (Application.systemLanguage)
			{
				case SystemLanguage.Afrikaans:
					_selectedLanguage = "af";
					break;
				case SystemLanguage.Arabic:
					_selectedLanguage = "ar";
					break;
				case SystemLanguage.Basque:
					_selectedLanguage = "eu";
					break;
				case SystemLanguage.Belarusian:
					_selectedLanguage = "be";
					break;
				case SystemLanguage.Bulgarian:
					_selectedLanguage = "bg";
					break;
				case SystemLanguage.Catalan:
					_selectedLanguage = "ca";
					break;
				case SystemLanguage.Chinese:
					_selectedLanguage = "zh";
					break;
				case SystemLanguage.Czech:
					_selectedLanguage = "cs";
					break;
				case SystemLanguage.Danish:
					_selectedLanguage = "da";
					break;
				case SystemLanguage.Dutch:
					_selectedLanguage = "nl";
					break;
				case SystemLanguage.English:
					_selectedLanguage = "en";
					break;
				case SystemLanguage.Estonian:
					_selectedLanguage = "et";
					break;
				case SystemLanguage.Faroese:
					_selectedLanguage = "fo";
					break;
				case SystemLanguage.Finnish:
					_selectedLanguage = "fi";
					break;
				case SystemLanguage.French:
					_selectedLanguage = "fr";
					break;
				case SystemLanguage.German:
					_selectedLanguage = "de";
					break;
				case SystemLanguage.Greek:
					_selectedLanguage = "el";
					break;
				case SystemLanguage.Hebrew:
					_selectedLanguage = "he";
					break;
				case SystemLanguage.Hungarian:
					_selectedLanguage = "hu";
					break;
				case SystemLanguage.Icelandic:
					_selectedLanguage = "is";
					break;
				case SystemLanguage.Indonesian:
					_selectedLanguage = "id";
					break;
				case SystemLanguage.Italian:
					_selectedLanguage = "it";
					break;
				case SystemLanguage.Japanese:
					_selectedLanguage = "ja";
					break;
				case SystemLanguage.Korean:
					_selectedLanguage = "ko";
					break;
				case SystemLanguage.Latvian:
					_selectedLanguage = "lv";
					break;
				case SystemLanguage.Lithuanian:
					_selectedLanguage = "lt";
					break;
				case SystemLanguage.Norwegian:
					_selectedLanguage = "no";
					break;
				case SystemLanguage.Polish:
					_selectedLanguage = "pl";
					break;
				case SystemLanguage.Portuguese:
					_selectedLanguage = "pt";
					break;
				case SystemLanguage.Romanian:
					_selectedLanguage = "ro";
					break;
				case SystemLanguage.Russian:
					_selectedLanguage = "ru";
					break;
				case SystemLanguage.SerboCroatian:
					_selectedLanguage = "hr";
					break;
				case SystemLanguage.Slovak:
					_selectedLanguage = "sk";
					break;
				case SystemLanguage.Slovenian:
					_selectedLanguage = "sl";
					break;
				case SystemLanguage.Spanish:
					_selectedLanguage = "es";
					break;
				case SystemLanguage.Swedish:
					_selectedLanguage = "sv";
					break;
				case SystemLanguage.Thai:
					_selectedLanguage = "th";
					break;
				case SystemLanguage.Turkish:
					_selectedLanguage = "tr";
					break;
				case SystemLanguage.Ukrainian:
					_selectedLanguage = "uk";
					break;
				case SystemLanguage.Vietnamese:
					_selectedLanguage = "vi";
					break;
				case SystemLanguage.ChineseSimplified:
					_selectedLanguage = "zh";
					break;
				case SystemLanguage.ChineseTraditional:
					_selectedLanguage = "zh";
					break;
				case SystemLanguage.Unknown:
					_selectedLanguage = "en";
					break;
				default:
					_selectedLanguage = "en";
					break;
			}
		}

		#endregion


		#region Download Data

		/// <summary>
		/// Start process to receive text with id
		/// </summary>
		/// <exception cref="ArgumentNullException"> Throws when id is empty or null</exception>
		/// <param name="pair">Keypair from ids dictionary. Contains your key as Key and biird key as Value</param>
		/// <param name="onComplete">function that call after receiving text</param>
		private void ReceiveTheText(KeyValuePair<string, string> pair, Action<string, string> onComplete)
		{
			if (string.IsNullOrEmpty(pair.Value))
			{
				throw new ArgumentNullException();
			}

			StartCoroutine(GetText(pair.Key, pair.Value, onComplete));
		}

		/// <summary>
		/// Download text
		/// </summary>
		/// <param name="idInGame">your key</param>
		/// <param name="biirdId">biird key id</param>
		/// <param name="onComplete">function, that call after receiving text</param>
		/// <returns></returns>
		private static IEnumerator GetText(string idInGame, string biirdId, Action<string, string> onComplete)
		{
			string result;
			using (var request = UnityWebRequest.Get(Url + biirdId + "?language=" + _selectedLanguage))
			{
				yield return request.SendWebRequest();

				if (request.isNetworkError || request.isHttpError)
				{
					Debug.LogError(request.error);
					yield break;
				}

				result = request.downloadHandler.text;
				Debug.Log("Receive: " + result);
			}
#if NET_4_6
			onComplete?.Invoke(idInGame, result);
#else
			if (onComplete != null)
			{
				onComplete(idInGame, result);
			}
#endif
		}

		#endregion

	}

	[Serializable]
	internal class BiirdDatabase
	{
		[SerializeField] public List<BiirdItem> BiirdItems;
	}

	[Serializable]
	internal struct BiirdItem
	{
		public string Key;
		public string Value;

		public BiirdItem(string key, string value)
		{
			Key = key;
			Value = value;
		}
	}
}