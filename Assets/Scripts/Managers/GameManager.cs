using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : PersistentSingleton<GameManager>
{
	protected override void Initialize()
	{
		SceneManager.activeSceneChanged += ActiveSceneChange;
	}

	public void LoadScene(string sceneName, bool onlyScene = false, bool setActive = false)
	{
		SceneManager.LoadScene(sceneName, onlyScene ? LoadSceneMode.Single : LoadSceneMode.Additive);
		if (!onlyScene && setActive) SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
	}
	public async void LoadSceneAsync(string sceneName, bool onlyScene = false, bool setActive = false)
	{
		await SceneManager.LoadSceneAsync(sceneName, onlyScene ? LoadSceneMode.Single : LoadSceneMode.Additive);
		if (!onlyScene && setActive) SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
	}
	public static void Quit() => Application.Quit();
	private void ActiveSceneChange(Scene old, Scene nu)
	{
		switch (nu.name)
		{
			case "MainMenu":
				Time.timeScale = 1;
				break;
			case "GameScene":
				break;
			default:
				Debug.LogError(nu.name + " | Unrecognized Scene Name to be Active Scene");
				break;
		}
	}
}
