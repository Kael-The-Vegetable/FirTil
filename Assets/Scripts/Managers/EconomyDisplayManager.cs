using TMPro;
using UnityEngine;

public class EconomyDisplayManager : MonoBehaviour
{
    [SerializeField] TMP_Text scoreDisplay;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scoreDisplay.text = "0";
    }

    // Update is called once per frame
    void Update()
    {
        scoreDisplay.text = EconomyManager.Instance.Balance.ToString();
    }
}
