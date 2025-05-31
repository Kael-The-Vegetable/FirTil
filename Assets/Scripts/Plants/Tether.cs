using UnityEngine;

public class Tether : MonoBehaviour
{
    LineRenderer lineRenderer;
    Transform stem;

	private void Awake()
	{
		lineRenderer = GetComponent<LineRenderer>();
	}
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, stem.position);
    }

    public void SetStem(Transform parent)
    {
        stem = parent;
    }
}
