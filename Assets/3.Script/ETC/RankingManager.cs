using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class RankingManager : MonoBehaviour
{
    public static RankingManager instance = null;

    private void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            RankList rank = RankingManager.instance.LoadRank();
            RankingManager.instance.Rank_List = rank.ranks;
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    

    public List<RankData> Rank_List = new List<RankData>();
   
    
    public RankData rank = new RankData();
    
    public void SaveRank()
    {
       
        
        if(Rank_List.Count.Equals(0))
        {
            
            Debug.Log("������?");
            Rank_List.Add(rank);
            

            string jsonData = JsonUtility.ToJson(new RankList(){ ranks = Rank_List });
            string path = Path.Combine(Application.dataPath, "rankData.json");
            
            File.WriteAllText(path, jsonData);
            return;
        }

        for(int i = Rank_List.Count-1; i>=0;i--)
        {
           
            if(Rank_List[i].score>rank.score)
            {
                if(i.Equals(2))
                    return;
                else
                {
                    Rank_List.Insert(i+1, rank);
                    if (Rank_List.Count == 4)
                        Rank_List.RemoveAt(Rank_List.Count - 1);
                    string jsonData = JsonUtility.ToJson(new RankList() { ranks = Rank_List });
                    string path = Path.Combine(Application.dataPath, "rankData.json");
                    
                    File.WriteAllText(path, jsonData);
                    return;
                }
            }
            else
            {
                if(i==0)
                {
                    Rank_List.Insert(i, rank);
                    if (Rank_List.Count == 4)
                        Rank_List.RemoveAt(Rank_List.Count - 1);
                    string jsonData = JsonUtility.ToJson(new RankList() { ranks = Rank_List });
                    string path = Path.Combine(Application.dataPath, "rankData.json");
                    Debug.Log("path : " + path);
                    Debug.Log("jsonData : " + jsonData);
                    File.WriteAllText(path, jsonData);
                    return;
                }
                
            }
            
            
        }
        
        
    }

    public void SetRanking_Data()
    {
        rank.name = GameManager.instance.SelectPlayer.ToString();
        rank.score = GameManager.instance.Score;
    }

    public RankList LoadRank()
    {
        string path = Path.Combine(Application.dataPath, "rankData.json");
        string jsonData = File.ReadAllText(path);
        //RankData loadRank = JsonUtility.FromJson<RankData>(jsonData);
        

        return JsonUtility.FromJson<RankList> (jsonData);
    }

    

    
}


