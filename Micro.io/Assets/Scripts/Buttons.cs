using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Buttons : MonoBehaviour
{
    private Button btn;
    Transform help;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(GameObject.Find("Main Camera").GetComponent<Camera>().pixelHeight);
        btn = GetComponent<Button>();
        if (btn != null) btn.onClick.AddListener(btnClicked);
        if (this.name == "ToHelp")
        {
            help = transform.GetChild(1);
            help.localScale = Vector3.zero;
        }
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
        if (name == "Help")
        {
            if (help.localScale == Vector3.zero)
            {
                help.localScale = Vector3.one;
            }
            else
            {
                help.localScale = Vector3.zero;
            }
        }
        else if(name == "Exit")
        {
            Application.Quit();
        }
        else if (SceneManager.GetSceneByName(name) != null)
        {
            SceneManager.LoadScene(name);
        }
    }
}
