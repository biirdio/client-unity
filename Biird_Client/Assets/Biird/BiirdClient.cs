using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Biird
{
	public class BiirdClient : MonoBehaviour
	{
		public delegate void TextReceived(string text);
		public event TextReceived OnTextReceived;

		private const string Url = "https://api.biird.io/resourceValue/";

		private string _selectedLanguage = "uk";
	

		#region Setup Biird

		/// <summary>
		/// Get instance of Biird class
		/// </summary>
		/// <returns>Biird object</returns>
		public static BiirdClient GetBiird()
		{
			var biird = FindObjectOfType<BiirdClient>();
			if (biird == null)
			{
				var gameObject = new GameObject("Biird");
				biird = gameObject.AddComponent<BiirdClient>();
				DontDestroyOnLoad(gameObject);
			}
			return biird;
		}
		
		/// <summary>
		/// Set current language
		/// </summary>
		/// <exception cref="ArgumentNullException"> Throws when use unsupported langauage</exception>
		/// <param name="language"></param>
		public void SelectLanguage(SupportedLanguages language)
		{
			switch (language)
			{
				case SupportedLanguages.English:
					_selectedLanguage = "en";
					break;
				case SupportedLanguages.Ukrainian:
					_selectedLanguage = "uk";
					break;
				case SupportedLanguages.Danish:
					_selectedLanguage = "da";
					break;
				default:
					throw new ArgumentNullException();
			}
		}

		#endregion
		

		#region Download Data

		/// <summary>
		/// Start process to receive text with id
		/// </summary>
		/// <exception cref="ArgumentNullException"> Throws when id is empty or null</exception>
		/// <param name="id">item id</param>
		public void ReceiveTheText(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				throw new ArgumentNullException();
			}
			StartCoroutine(GetText(id));
		}
		
		private IEnumerator GetText(string id)
		{
			string result;
			using (var request = UnityWebRequest.Get(Url + id + "?language=" + _selectedLanguage))
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

			if (OnTextReceived != null)
			{
				OnTextReceived(result);
			}
		}

		#endregion
		
	}

	public enum SupportedLanguages : byte
	{
		English, Ukrainian, Danish
	}

	
}