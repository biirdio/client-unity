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
		_biird.SelectLanguage(SupportedLanguages.Ukrainian);
		_biird.ReceiveTheText("f7fe003a-9ddf-4b1e-8920-79d4a57796ee"); //<p>Hello World!</p>
	}

	private void OnTextReceived(string text)
	{
		_testText.text = text;
	}
}
