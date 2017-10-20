using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class UserHandler : MonoBehaviour {

    public string email;
    public string password;
    public string savedPasswordHash;

    public Text displayText;
    public Button loginButton;
    public InputField emailField;
    public InputField passField;
	// Use this for initialization
	void Start () {
        Button btn = loginButton.GetComponent<Button>();
        btn.onClick.AddListener(LoginClick);
    }

    void LoginClick()
    {
        email = emailField.GetComponent<InputField>().text;
        password = passField.GetComponent<InputField>().text;

        savedPasswordHash = CreateMD5(password);

        WWWForm formData = new WWWForm();
        formData.AddField("postEmail", email);
        formData.AddField("postPassword", savedPasswordHash);

        WWW www = new WWW("http://ec2-18-221-237-133.us-east-2.compute.amazonaws.com/scripts/createUser.php", formData);

        StartCoroutine(request(www));
    }

    void Register()
    {
        WWWForm formData = new WWWForm();
        formData.AddField("postEmail", email);
        formData.AddField("postPassword", savedPasswordHash);
        WWW www = new WWW("http://ec2-18-221-237-133.us-east-2.compute.amazonaws.com/scripts/createUser.php", formData);

        StartCoroutine(request(www));
    }
	
	// Update is called once per frame
	IEnumerator request (WWW www) {
        yield return www;

        if(www.text == "0 results")
        {
            Register();
        }
        else
        {
            displayText = displayText.GetComponent<Text>();

            displayText.text = "Logged in";
        }
	}
    public static string CreateMD5(string input)
    {
        // Use input string to calculate MD5 hash
        using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
        {
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
