using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoicePlayTrigger : MonoBehaviour
{
    public AudioSource trigSource;
    public AudioClip[] voiceList;
    public GameObject[] subtitles;

    public void PlayCatVoice(int index)
    {
        trigSource.PlayOneShot(voiceList[index]);
        subtitles[index].SetActive(true);

        StartCoroutine(RemoveSub(index, voiceList[index].length));
    }

    private IEnumerator RemoveSub(int index, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        subtitles[index].SetActive(false);
    }
}
