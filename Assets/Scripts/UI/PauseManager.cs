using UnityEngine;

public class PauseManager : Singleton<PauseManager>
{
	[SerializeField] private Canvas _pauseCanvas;
	private bool _paused = false;
	private float _originalTime = 0;

	public bool IsPaused => _paused;
	protected override void Initialize()
	{
		InputManager.Escape.AddListener(OnPause);
		_pauseCanvas.gameObject.SetActive(_paused);
	}

	public void OnPause()
	{
		_paused ^= true;
		_pauseCanvas.gameObject.SetActive(_paused);
		(Time.timeScale, _originalTime) = (_originalTime, Time.timeScale);
	}

	public void OnExit()
	{
		GameManager.Instance.LoadSceneAsync("MainMenu", true, true);
	}

	private void OnDestroy()
	{
		InputManager.Escape.RemoveListener(OnPause);
	}
}
