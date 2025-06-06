using UnityEngine;

public class MainMenu : MonoBehaviour
{
	public void StartGame()
	{
		GameManager.Instance.LoadSceneAsync("GameScene", true, true);
	}
	public void ExitGame()
	{
		GameManager.Quit();
	}
}
