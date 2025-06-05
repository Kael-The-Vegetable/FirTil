using UnityEngine;

public class CanvasFlip : MonoBehaviour
{
	[SerializeField] private Canvas _canvasOne;
	[SerializeField] private Canvas _canvasTwo;

	public void Swap()
	{
		_canvasOne.gameObject.SetActive(_canvasTwo.gameObject.activeInHierarchy);
		_canvasTwo.gameObject.SetActive(!_canvasOne.gameObject.activeInHierarchy);
	}
}
