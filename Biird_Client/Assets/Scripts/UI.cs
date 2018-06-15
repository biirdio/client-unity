using Biird;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
	private BiirdClient _biird;
	[SerializeField] private Text _testText;
	[SerializeField] private BiirdIds _ids;
	private void Awake()
	{
		BiirdClient.Init();
		_biird = BiirdClient.GetBiird();
		_biird.OnBiirdInitialized+= OnBiirdInitialized;
		_biird.LoadData();
	}

	private void OnBiirdInitialized()
	{
		_testText.text = _biird.Items["Hello World"];
	}

}
