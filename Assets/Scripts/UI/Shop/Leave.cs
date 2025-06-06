using UnityEngine;
using UnityEngine.SceneManagement;

public class Leave : MonoBehaviour
{
	public void StopStore()
	{
		SpawnerManager.Instance.StartNextDay();
		SceneManager.UnloadSceneAsync("Store");
	}
}
