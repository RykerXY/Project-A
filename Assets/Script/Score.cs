using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    public float maxHeight = 0f;
    private float highest;
    public Transform player;
    public TextMeshProUGUI highestText;
    public TextMeshProUGUI highText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        highest = PlayerPrefs.GetFloat("Highest", 0f);
    }

    // Update is called once per frame
    void Update()
    {
        // เปรียบเทียบตำแหน่ง y ของผู้เล่นกับ maxHeight
        if (player.position.y > maxHeight)
        {
            maxHeight = player.position.y; // อัพเดต maxHeight ถ้าผู้เล่นสูงขึ้น
        }
        highestText.text = "Highest : " + Mathf.Round(highest * 100f) / 100f;
        highText.text = "High : " + Mathf.Round(maxHeight * 100f) / 100f;
        
        if (maxHeight > highest)
        {
            highest = maxHeight;
            PlayerPrefs.SetFloat("Highest", Mathf.Round(highest * 100f) / 100f);
        }
    }
}
