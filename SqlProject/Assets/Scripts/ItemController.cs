using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using SimpleJSON;
using TMPro;

public class ItemController : MonoBehaviour
{

    Action<string> _createItemsCallback;


    // Start is called before the first frame update
    void Start()
    {
        _createItemsCallback = (jsonArray) => {
            StartCoroutine(CreateItemsRoutine(jsonArray));
        };

        CreateItems();
    }

    public void CreateItems()
    {
        string userid = Main.Instance.userInfo.userID;
        StartCoroutine(Main.Instance.web.GetItemsIDs(userid, _createItemsCallback));
    }

    IEnumerator CreateItemsRoutine(string jsonArraystring)
    {
        JSONArray jsonArray = JSON.Parse(jsonArraystring) as JSONArray;

        for(int i = 0; i < jsonArray.Count; i++)
        {
            bool isDone = false;
            string itemId = jsonArray[i].AsObject["itemID"];
            string id = jsonArray[i].AsObject["ID"]; ;

            JSONObject itemInfoJson = new JSONObject();

            // Create a callback to get info from web.cs
            Action<string> getItemInfoCallback = (itemInfo) =>
            {
                isDone = true;
                JSONArray tempArray = JSON.Parse(itemInfo) as JSONArray;
                itemInfoJson = tempArray[0].AsObject;
            };

            StartCoroutine(Main.Instance.web.GetItem(itemId, getItemInfoCallback));

            //Wait until the callback is called from WEB
            yield return new WaitUntil(() => isDone == true);

            //Instantiate Gameobject
            GameObject itemGo = Instantiate(Resources.Load("Prefabs/Item") as GameObject);
            Item item = itemGo.AddComponent<Item>();

            item.ID = id;
            item.ItemID = itemId;

            itemGo.transform.SetParent(this.transform);
            itemGo.transform.localScale = Vector3.one;
            itemGo.transform.localPosition = Vector3.zero;

            //Fill Information
            itemGo.transform.Find("Name").GetComponent<TMP_Text>().text = itemInfoJson["name"];
            itemGo.transform.Find("Price").GetComponent<TMP_Text>().text = itemInfoJson["price"];
            itemGo.transform.Find("Description").GetComponent<TMP_Text>().text = itemInfoJson["description"];

            // Create a callback to get SPrite from web.cs
            Action<Sprite> getItemIconCallback = (downloadedSprite) => {
                itemGo.transform.Find("Image").GetComponent<Image>().sprite = downloadedSprite;
            };
            StartCoroutine(Main.Instance.web.GetItemIcon(itemId, getItemIconCallback));

            //Set Sell Button
            itemGo.transform.Find("SellButton").GetComponent<Button>().onClick.AddListener(() =>
            {
                string idInInventory = id;
                string iId = itemId;
                string userId = Main.Instance.userInfo.userID;

                StartCoroutine(Main.Instance.web.SellItem(idInInventory, iId, userId));
            });

        }

        yield return null;
    }
}
