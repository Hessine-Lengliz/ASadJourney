using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChanger : MonoBehaviour
{
    public float changeTime;
    public string sceneName;
    private void Update()
    {
        changeTime -= Time.deltaTime;
        if (changeTime < 0 || Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(sceneName);

        }
       
    }
}
