using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Buttons : MonoBehaviour
{
    private Button btn;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(GameObject.Find("Main Camera").GetComponent<Camera>().pixelHeight);
        btn = GetComponent<Button>();
        if (btn != null) btn.onClick.AddListener(btnClicked);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void btnClicked()
    {
        GoToScene(this.name.Substring(2));
    }

    public void GoToScene(string name)
    {
        /*if (name == "Help")
        {
            transform.GetChild(1).gameObject.SetActive(true);
        }
        else */if(name == "Exit")
        {
            Application.Quit();
        }
        else if (SceneManager.GetSceneByName(name) != null)
        {
            SceneManager.LoadScene(name);
        }
    }
}
