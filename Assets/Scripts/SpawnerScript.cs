using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public GameObject tictac, toe;
    public AudioClip tictacAudio, toeAudio;
    public int player;
    // Start is called before the first frame update
    void Start()
    {
        if(player == 0)
        {
            StartCoroutine(SpawnTicTacs());
        }
        else
        {
            SpawnToe();
        }
    }

    WaitForSeconds waitTime = new WaitForSeconds(0.05f);
    IEnumerator SpawnTicTacs()
    {
        this.GetComponent<AudioSource>().clip = tictacAudio;
        this.GetComponent<AudioSource>().Play();
        for (int i = 0; i < 10; i++)
        {
            Instantiate(tictac, this.transform.position + Random.insideUnitSphere, Random.rotation);
            yield return waitTime;
        }
        Destroy(this.gameObject);
    }

    protected void SpawnToe()
    {
        this.GetComponent<AudioSource>().clip = toeAudio;
        this.GetComponent<AudioSource>().Play();
        Instantiate(toe, this.transform.position, Random.rotation);
        StartCoroutine("DelayDestroy");//Method was not accepting non-string parameter. I have no idea why
    }

    WaitForSeconds dTimer = new WaitForSeconds(0.4f);
    IEnumerable DelayDestroy()
    {
        yield return dTimer;
        Destroy(this.gameObject);
    }
}
