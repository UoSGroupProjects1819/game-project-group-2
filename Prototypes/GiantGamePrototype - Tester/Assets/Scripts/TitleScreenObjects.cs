using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenObjects : MonoBehaviour {

    public GameObject island1;
    public GameObject island2;
    public GameObject island3;

    public float speedI1;
    public float speedI2;
    public float speedI3;

    public int dir = -1;

    // Use this for initialization
    void Start () {

        speedI1 = Random.Range(.05f, .15f);
        speedI2 = Random.Range(.05f, .15f);
        speedI3 = Random.Range(.05f, .15f);

        StartCoroutine("FloatDir");

    }
	
	// Update is called once per frame
	void FixedUpdate () {

        

        var move = new Vector3(0, dir, 0);
        island1.transform.position += move * speedI1 * Time.deltaTime;

        island2.transform.position += move * speedI2 * Time.deltaTime;

        island3.transform.position += move * speedI3 * Time.deltaTime;

    }

    public void OnClickOptions()
    {

        SceneManager.LoadScene("Options");

    }

    public void OnClickCredits()
    {

        SceneManager.LoadScene("Credits");

    }

    public void OnClickIsland1()
    {

        PlayerPrefs.GetInt("IslandSelect", 0);
        PlayerPrefs.SetInt("IslandSelect", 0);

        SceneManager.LoadScene("Prototype");

    }

    public void OnClickIsland2()
    {

        PlayerPrefs.GetInt("IslandSelect", 0);
        PlayerPrefs.SetInt("IslandSelect", 1);

        SceneManager.LoadScene("Prototype");

    }

    public void OnClickIsland3()
    {

        PlayerPrefs.GetInt("IslandSelect", 0);
        PlayerPrefs.SetInt("IslandSelect", 2);

        SceneManager.LoadScene("Prototype");

    }

    IEnumerator FloatDir()
    {

        yield return new WaitForSeconds(3);
        dir = dir * -1;
        StartCoroutine("FloatDir");

    }
}
