using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public Button button;
    public GameObject reg;
    public GameObject login;

    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(() =>
        {
           StartCoroutine(Main.Instance.web.Login(usernameInput.text, passwordInput.text));
        });
    }

    public void Swap()
    {
        reg.SetActive(true);
        login.SetActive(false);
    }

}
