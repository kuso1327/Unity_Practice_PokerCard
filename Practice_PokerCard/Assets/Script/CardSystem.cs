using System.Collections;           //協程
using System.Collections.Generic;   //List
using System.Linq;                  //查詢
using UnityEngine;

[ExecuteAlways]     //在編輯模式執行
public class CardSystem : MonoBehaviour
{
    /// <summary>
    /// 撲克牌：所有撲克牌，52張
    /// </summary>
    public List<GameObject> cards = new List<GameObject>();

    /// <summary>
    /// 花色：黑桃、愛心、方塊、梅花
    /// </summary>
    public string[] type = { "Spades", "Heart", "Diamond", "Club" };

    private void Start()
    {
        GetAllCard();
    }

    /// <summary>
    /// 取得所有撲克牌
    /// </summary>
    void GetAllCard()
    {
        if (cards.Count == 52) return;
        //跑四個花色
        for (int i = 0; i < type.Length; i++)
        {
            //跑1-13張
            for (int j = 1; j < 14; j++)
            {
                string number = j.ToString();       //數字 = j.轉字串

                switch (j)
                {

                    case 1:
                        number = "A";               //數字1改為A
                        break;
                    case 11:
                        number = "J";               //數字11改為J
                        break;
                    case 12:
                        number = "Q";               //數字12改為Q
                        break;
                    case 13:
                        number = "K";               //數字13改為K
                        break;
                }

                //卡牌=素材.載入<遊戲物件>("素材名稱")
                GameObject card = Resources.Load<GameObject>("PlayingCards_" + number + type[i]);
                //撲克牌.添加(卡牌)
                cards.Add(card);
            }

        }
    }

    /// <summary>
    /// 透過花色選取卡牌
    /// </summary>
    /// <param name="type">花色</param>
    public void ChooseCardByType(string type)
    {
        DeleltAllChild();                                       //刪除所有子物件
        
        //暫存牌組 = 撲克牌.哪裡((物件)=>物件.名稱.包含(花色))
        var temp = cards.Where((x) => x.name.Contains(type));

        //迴圈 遍尋 暫存牌組 生成(卡牌,父物件)
        foreach (var item in temp)
        {
            Instantiate(item, transform);

            StartCoroutine(SetChildPosition());                 //設定協程
        }

    }

    /// <summary>
    /// 洗牌
    /// </summary>
    public void Shuffle()
    {
        DeleltAllChild();
        //參考型別 - Toarray()轉為陣列實質型別 ToList()轉回清單
        List<GameObject> shuffle = cards.ToArray().ToList();    //另存一份洗牌用原始牌組
        List<GameObject> newCards = new List<GameObject>();     //儲存洗牌用的新牌組

        for (int i = 0; i < 52; i++)
        {
            int r = Random.Range(0, shuffle.Count);                 //從洗牌用牌組挑選一張
            GameObject temp = shuffle[r];                           //挑出來的隨機卡牌
            newCards.Add(temp);                                     //添加到新牌組
            shuffle.RemoveAt(r);                                    //刪除挑出來的牌


        }

        foreach (var item in newCards)
        {
            Instantiate(item, transform);
            StartCoroutine(SetChildPosition());
        }
    }

    /// <summary>
    /// 設定子物件座標：座標、排序、大小、角度
    /// </summary>
    /// <returns></returns>
    private IEnumerator SetChildPosition()
    {
        yield return new WaitForSeconds(0.1F);                  //避免刪除這次卡牌

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);            //取得子物件
            child.eulerAngles = new Vector3(180, 0, 0);         //設定角度
            child.localScale = Vector3.one * 20;                //設定尺寸

            float x = i % 13;                                   //x = i % 13每13張都從1開始
            int y = i / 13;                                     //y = i / 13取得每一排的高度
            child.position = new Vector3((x - 6) * 1.3f, 5 - y * 2F, 0);   //x = (i-6)*間距,y = 5 - y * 間距

            yield return null;
        }
    }

    /// <summary>
    /// 刪除子物件
    /// </summary>
    void DeleltAllChild()
    {
        for (int i = 0; i <transform.childCount ; i++)
        {
            Destroy(transform.GetChild(i).gameObject);          
        }
    }

    /// <summary>
    /// 排序：花色、數字由大到小
    /// </summary>
    public void Sort()
    {
        DeleltAllChild();
        
        //排序後的卡牌 = 從 cards 找資源放到card
        //where 條件 - 黑桃||愛心||方塊||梅花
        //select 選取 card
        var sort = from card in cards
                   where card.name.Contains(type[0]) || card.name.Contains(type[1])|| card.name.Contains(type[2])|| card.name.Contains(type[3])
                   select card;

        foreach (var item in sort)
        {
            Instantiate(item, transform);
            StartCoroutine(SetChildPosition());
        }
    }


}
