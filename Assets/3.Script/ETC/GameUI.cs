using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameObject HeartPrefabs;
    [SerializeField] private Camera FinalCamera;
    [SerializeField] private Text Score;
    [SerializeField] private Text Fisrt;
    [SerializeField] private Text Second;
    [SerializeField] private Text Third;
    [SerializeField] private Text FScore;
    [SerializeField] private Text SScore;
    [SerializeField] private Text TScore;
    [SerializeField] private Text CurrentScore;
    [SerializeField] private Text CurrentName;

    

    private GameObject Heart;
    private List<GameObject> HeartList = new List<GameObject>();
    private Player_Health currentHealth;
    private Vector3 HP_Pos = new Vector3(-80f, -45f, 0f);
    private void Start()
    {
        currentHealth = FindObjectOfType<Player_Health>();
        for (int i = 0; i < currentHealth.CurrentHealth; i++)
        {
            Heart = Instantiate(HeartPrefabs, transform.position, Quaternion.LookRotation(FinalCamera.gameObject.transform.forward));
            Heart.transform.SetParent(transform);
            Heart.transform.position = transform.position + HP_Pos + Vector3.right * 15f * i;
            Heart.transform.localScale = new Vector3(50, 50, 50);
            HeartList.Add(Heart);
        }
        Debug.Log(currentHealth.gameObject.name);
        Debug.Log(currentHealth.CurrentHealth);
    }
    private void Update()
    {
        for(int i = 0; i < HeartList.Count;i++)
        {
            HeartList[i].transform.Rotate(Vector3.up,Time.deltaTime*10f);
        }
        SetHp();
        Score.text = "Score : " + GameManager.instance.Score;
    }

    private void SetHp()
    {
        if (!HeartList.Count.Equals(currentHealth.CurrentHealth)&&HeartList.Count>0)
        {
            Destroy(HeartList[HeartList.Count - 1]);
            HeartList.RemoveAt(HeartList.Count - 1);
        }
    }

    public void LoadBoard()
    {
        RankingManager.instance.SetRanking_Data();
        CurrentScore.text = RankingManager.instance.rank.score.ToString();
        CurrentName.text = RankingManager.instance.rank.name;
        
        List<RankData> ranks= RankingManager.instance.LoadRank();
        switch(ranks.Count)
        {
            case 1:
                Fisrt.text = ranks[0].name;
                FScore.text = ranks[0].score.ToString();
                break;
            case 2:
                Fisrt.text = ranks[0].name;
                FScore.text = ranks[0].score.ToString();
                Second.text = ranks[1].name;
                SScore.text = ranks[1].score.ToString();
                break;
            case 3:
                Fisrt.text = ranks[0].name;
                FScore.text = ranks[0].score.ToString();
                Second.text = ranks[1].name;
                SScore.text = ranks[1].score.ToString();
                Third.text = ranks[2].name;
                TScore.text = ranks[2].score.ToString();
                break;
        }

        

    }
}
