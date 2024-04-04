using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Web : MonoBehaviour
{
    public bool UseExternalDB = false;
    public string internalDomain = "http://localhost/Unity/";
    public string externalDomain = "https://mysim421.000webhostapp.com/";
    void Start()
    {
        //StartCoroutine(GetUsers());
        //StartCoroutine(Login("testuser", "123456"));
        //StartCoroutine(RegisterUser("testuser3", "123456"));
    }

   /* public void ShowUserItems()
    {
        StartCoroutine(GetItemsIDs(Main.Instance.userInfo.userID));
    } */

    IEnumerator GetDate()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("https://mysim421.000webhostapp.com/GetDate.php"))
        {
            yield return www.SendWebRequest();

            if(www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);

                byte[] results = www.downloadHandler.data;
            }
        }
    }


    IEnumerator GetUsers()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("https://mysim421.000webhostapp.com/GetUsers.php"))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);

                byte[] results = www.downloadHandler.data;
            }
        }
    }


    public IEnumerator Login(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("loginUser", username);
        form.AddField("loginPass", password);

        using UnityWebRequest www = UnityWebRequest.Post("https://mysim421.000webhostapp.com/Login.php", form);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            Main.Instance.userInfo.SetCredentials(username, password);
            Main.Instance.userInfo.SetID(www.downloadHandler.text);

            if(www.downloadHandler.text.Contains("Username does not exist" ) || www.downloadHandler.text.Contains("-1"))
            {
                Debug.Log("Try Again");
            } else
            {
                Main.Instance.userProfile.SetActive(true);
                Main.Instance.login.gameObject.SetActive(false);
            }
        }
    }

    public IEnumerator RegisterUser(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("loginUser", username);
        form.AddField("loginPass", password);

        using UnityWebRequest www = UnityWebRequest.Post("https://mysim421.000webhostapp.com/RegisterUser.php", form);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
        }
    }

    public IEnumerator GetItemsIDs(string userID, System.Action<string> callback) {
        WWWForm form = new WWWForm();
        form.AddField("userID", userID);

        using (UnityWebRequest www = UnityWebRequest.Post("https://mysim421.000webhostapp.com/GetItemsIDs.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                string jsonArray = www.downloadHandler.text;

                callback(jsonArray);
            }
        }
    }


    public IEnumerator GetItem(string itemID, System.Action<string> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("itemID", itemID);

        using (UnityWebRequest www = UnityWebRequest.Post("https://mysim421.000webhostapp.com/GetItem.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                string jsonArray = www.downloadHandler.text;

                callback(jsonArray);
            }
        }
    }

    public IEnumerator GetItemIcon(string itemID, System.Action<Sprite> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("itemID", itemID);

        using (UnityWebRequest www = UnityWebRequest.Post("https://mysim421.000webhostapp.com/GetItemIcon.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error);
            }
            else
            {
                byte[] bytes = www.downloadHandler.data;

                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(bytes);

                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                callback(sprite);
            }
        }
    }


    public IEnumerator SellItem(string ID, string itemID, string userID)
    {
        WWWForm form = new WWWForm();
        form.AddField("itemID", itemID);
        form.AddField("userID", userID);
        form.AddField("ID", ID);

        using (UnityWebRequest www = UnityWebRequest.Post("https://mysim421.000webhostapp.com/SellItem.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
            }
        }
    }
}
