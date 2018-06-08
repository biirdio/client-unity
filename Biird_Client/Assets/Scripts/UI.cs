using Biird;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
	private BiirdClient _biird;
	[SerializeField] private Text _testText;
	
	private void Awake()
	{
		_biird = BiirdClient.GetBiird();
		_biird.OnTextReceived+=OnTextReceived;
		_biird.SelectLanguage(SupportedLanguages.English);
		_biird.ReceiveTheText("b9fb0f44-31d5-45df-9ec3-776568802c31"); //<p>Hello World!</p>
	}

	private void OnTextReceived(string text)
	{
		_testText.text = text;
	}
}
