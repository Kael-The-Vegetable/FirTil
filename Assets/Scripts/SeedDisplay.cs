using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SeedDisplay : MonoBehaviour
{
    public static SeedDisplay Instance;

    [SerializeField] Image seedPacket;
    [SerializeField] TMP_Text seedName;
    [SerializeField] TMP_Text seedCount;

	private void Awake()
	{
		Instance = this;
	}

    public void UpdateDisplay(Sprite sprite, string seedString, int seedNumber)
    {
        seedPacket.enabled = true;
        seedPacket.sprite = sprite;
        seedName.text = seedString;
        seedCount.text = seedNumber.ToString();
    }
	

    public void ResetDisplay()
    {
        seedPacket.enabled = false;
        seedName.text = "";
        seedCount.text = "";
    }
}
