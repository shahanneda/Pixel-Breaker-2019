using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource musicSource;

    public AudioClip sectionACalm;
    public AudioClip sectionBCalm;

    public AudioClip sectionA;
    public AudioClip sectionB;

    public AudioClip outroA;
    public AudioClip outroB;

    public bool tense = false;

    private AudioClip queuedClip;

    private void Start()
    {
        QueueClip(sectionACalm);
    }

    private void Update()
    {
        if (!musicSource.isPlaying)
        {
            CheckNextClip();

            musicSource.clip = queuedClip;
            musicSource.Play();
        }
    }

    private void CheckNextClip()
    {
        if (tense)
        {
            if (musicSource.clip.Equals(sectionACalm) || musicSource.clip.Equals(sectionA))
            {
                QueueClip(sectionB);
            }
            else if (musicSource.clip.Equals(sectionBCalm) || musicSource.clip.Equals(sectionB))
            {
                QueueClip(sectionA);
            }
        }
        else
        {
            if (musicSource.clip.Equals(sectionACalm) || musicSource.clip.Equals(sectionA))
            {
                QueueClip(sectionBCalm);
            }
            else if (musicSource.clip.Equals(sectionBCalm) || musicSource.clip.Equals(sectionB))
            {
                QueueClip(sectionACalm);
            }
        }
    }

    public void QueueClip(AudioClip clipToQueue)
    {
        queuedClip = clipToQueue;
    }
}
