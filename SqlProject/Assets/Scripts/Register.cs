using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Register : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public TMP_InputField confirmpasswordInput;
    public Login log;

    public void Reg()
    {
        if(passwordInput.text == confirmpasswordInput.text && passwordInput.text != null && usernameInput.text != null)
        {
            StartCoroutine(Main.Instance.web.RegisterUser(usernameInput.text, passwordInput.text));
            log.login.SetActive(true);
            log.reg.SetActive(false);
            Debug.Log("Registered!");
        }
        else
        {
            Debug.Log("wrong!");
        }
    }
}
