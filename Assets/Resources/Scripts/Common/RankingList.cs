using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;
using System.IO;

public class RankingList : MonoBehaviour
{

    public Text rankText;   							//名次文本
    public Text nameText;   							//姓名文本
    public Text scoreText;  							//分数文本

    private int NumOfRank = 10;							//排行榜显示数量
	private List<Data> rankList = new List<Data>();		//用来存储排行榜数据

    void Start()
    {
        rankText.text = "Rank";
        nameText.text = "Name";
        scoreText.text = "Score";

		//查询排行榜本地数据，如果存在就加载（游戏未发布文本保存在Resources文件夹，发布后保存在游戏放置文件夹下Resources文件夹中）
		string path = Application.dataPath + "/Resources/RankingList.xml";
		if (File.Exists(path))
			LoadXML(path);
		
		ShowRankingList ();								//显示排行榜
    }

    void LoadXML(string path)   //XML加载
    {
        XmlDocument xml = new XmlDocument();
        xml.Load(path);
        XmlNodeList items = xml.SelectNodes("//RankingList/Item");
        foreach (XmlNode item in items)
        {
            XmlNode name = item.FirstChild;
            XmlNode score = name.NextSibling;
			rankList.Add (new Data(name.InnerText, int.Parse (score.InnerText)));
        }
    }

    public void ShowRankingList()
	{
		rankText.text = "Rank";
		nameText.text = "Name";
		scoreText.text = "Score";
        
		//有本地数据
		if (rankList.Count != 0)
        {
            int i = 1;
			foreach (Data item in rankList)
            {
                rankText.text += "\n" + i.ToString();
				nameText.text += "\n" + item.name;
                scoreText.text += "\n" + item.score.ToString();
                i++;
            }
		}

		//无本地数据
        else
        {
            rankText.text += "\nnull";
            nameText.text += "\nnull";
            scoreText.text += "\nnull";
        }
    }

    public void SetSimpleItem(string newName, int newScore)						//设置单个数据，一般游戏结束后保存单局成绩
    {
		if(newName == "") newName = "player";									//如果玩家不输入，默认输入名

        //如果存在xml，则更改xml中的数据，不存在就新建xml，保存数据
        string path = Application.dataPath + "/Resources/RankingList.xml";
        if (File.Exists(path))
            UpdateXML(path, newName, newScore);
        else
            CreateXML(path, newName, newScore);
    }

    void CreateXML(string path, string newName, int newScore)
    {
        XmlDocument xml = new XmlDocument();
        XmlElement root = xml.CreateElement("RankingList");
        XmlElement item = xml.CreateElement("Item");
        XmlElement name = xml.CreateElement("Name");
        XmlElement score = xml.CreateElement("Score");

        name.InnerText = newName;												//保存姓名和分数
        score.InnerText = newScore.ToString();

        item.AppendChild(name);
        item.AppendChild(score);
        root.AppendChild(item);
        xml.AppendChild(root);
        xml.Save(path);
    }

    void UpdateXML(string path, string newName, int newScore)
    {
        XmlDocument xml = new XmlDocument();
        xml.Load(path);
        XmlNode root = xml.SelectSingleNode("RankingList");
        XmlNodeList items = root.SelectNodes("Item");
        XmlNode insertNode = null;

        //根据分数降序排行
        foreach (XmlNode item in items)
        {
            XmlNode mScore = item.FirstChild.NextSibling;
            if (newScore > int.Parse(mScore.InnerText))
            {
                insertNode = item;
                break;
            }
        }

        //分数在前10名则保存
        if (insertNode != null || items.Count < NumOfRank)
        {
            XmlElement item = xml.CreateElement("Item");
            XmlElement name = xml.CreateElement("Name");
            XmlElement score = xml.CreateElement("Score");
            name.InnerText = newName;
            score.InnerText = newScore.ToString();
            item.AppendChild(name);
            item.AppendChild(score);
            if (insertNode != null)
                root.InsertBefore(item, insertNode);
            else
                root.AppendChild(item);
            xml.AppendChild(root);
            if (items.Count > NumOfRank)
                root.RemoveChild(items[NumOfRank]);
            xml.Save(path);
        }
    }
}