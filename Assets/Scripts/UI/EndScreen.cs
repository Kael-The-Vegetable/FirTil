using TMPro;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
	[SerializeField] private TMP_Text scoreText;

	private void OnEnable()
	{
		scoreText.text = EconomyManager.Instance.Balance.ToString();
	}

	public void RetryGame()
	{
		Time.timeScale = 1.0f;
		GameManager.Instance.LoadSceneAsync("GameScene", true, true);
	}
	public void ExitGame()
	{
		GameManager.Quit();
	}
}
