using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class HudController : MonoBehaviour {
    public Text score;
    public Button restart;
    public Text birdCount;
    public Text pigCount;

    private void Awake()
    {
 
        restart.onClick.AddListener(ReloadScene);
    }

    private void Update()
    {
        birdCount.text = "X " + FindObjectOfType<GameMode>().birdCount;
        pigCount.text = "X " + FindObjectOfType<GameMode>().pigCount;
    }

    void ReloadScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("game");
    }

    public void UpdateScore(int addedScore)
    {
        //score.text.TrimStart('0');
        //Debug.Log(score.text.TrimStart('0'));
        var nextScore = int.Parse(score.text) + addedScore;
        if (nextScore.ToString().Length < 8)
        {
            var textS = new StringBuilder();
            for (int i = 0; i < 8 - nextScore.ToString().Length; i++)
            {
                textS.Append("0");
            }

            textS.Append(nextScore.ToString());
            score.text = textS.ToString();
        }
        else
        {
            score.text = "99999999";
        }
    }

    public void EndGame(bool isWin)
    {
        if (!isWin)
        {
            restart.gameObject.GetComponentInChildren<Text>().text = "FAIL!";
        }
        restart.gameObject.SetActive(true);
    }

}
