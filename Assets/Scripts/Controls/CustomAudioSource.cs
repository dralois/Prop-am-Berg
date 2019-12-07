using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomAudioSource : MonoBehaviour
{
    [SerializeField] private AudioClip[] myClip;
    [SerializeField] private bool loop = true;
    [SerializeField] private bool playOnAwake = true;
    [SerializeField] private bool randomize = true;
    [SerializeField] private bool noDuplicate = true;
    [SerializeField] private bool playRandomly = false;
    [SerializeField] private float minRandomWaitTime = 2f;
    [SerializeField] private float maxRandomWaitTime = 20f;

    void Awake()
    {       
        float waitTime = Random.Range(minRandomWaitTime, maxRandomWaitTime);
        Debug.Log(waitTime);
        if (playOnAwake)
        {
            waitTime = 0;
        }
        StartCoroutine(playSound(waitTime));
    }

    public IEnumerator playSound(float startWait)
    {
        yield return new WaitForSeconds(startWait);
        Debug.Log(this.name + " tried to play a sound");

        if (myClip == null) Debug.LogError("Please assign the audioclip of " + this.name);
        else
        {
            int index = Random.Range(0, myClip.Length);
            AudioManager.Instance.Play(AudioManager.AudioType.Sound, myClip[index], loop, randomize, noDuplicate);
            Debug.Log(this.name + " played " + myClip[index].name);

            if (playRandomly)
            {               
                yield return new WaitForSeconds(Random.Range(minRandomWaitTime, maxRandomWaitTime));
                StartCoroutine(playSound(0));
            }
            
        }
        yield return null;
    }

    
}
