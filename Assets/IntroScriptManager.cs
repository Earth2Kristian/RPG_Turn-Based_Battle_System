using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScriptManager : MonoBehaviour
{
    public float textValue;

    // Text Message Objects
    public GameObject textMessage1;
    public GameObject textMessage2;
    public GameObject textMessage3;
    void Start()
    {
        textValue = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            textValue++;
        }

        if (textValue == 0)
        {
            textMessage1.SetActive(true);
        }
        if (textValue == 1)
        {
            textMessage1.SetActive(false);
            textMessage2.SetActive(true);
        }
        if (textValue == 2)
        {
            textMessage2.SetActive(false);
            textMessage3.SetActive(true);
        }
        if (textValue == 3) 
        {
            SceneManager.LoadScene(2);
        }
    }
}
