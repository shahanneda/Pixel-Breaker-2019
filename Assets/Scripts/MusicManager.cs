using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource introSource;

    public AudioSource sectionACalm;
    public AudioSource sectionBCalm;

    public AudioSource sectionA;
    public AudioSource sectionB;

    public AudioSource outroA;
    public AudioSource outroB;

    public bool tense = false;

    private AudioSource currentSource;
    private AudioSource queuedClip;

    private bool outroQueued = false;

    private void Start()
    {
        PlayClip(introSource);
        QueueClip(sectionACalm);
    }

    private void Update()
    {
        if (!currentSource.isPlaying)
        {
            CheckNextClip();
            PlayClip(queuedClip);
        }
    }

    private void CheckNextClip()
    {
        if (!outroQueued)
        {
            if (tense)
            {
                if (currentSource.Equals(sectionACalm) || currentSource.Equals(sectionA))
                {
                    QueueClip(sectionB);
                }
                else if (currentSource.Equals(sectionBCalm) || currentSource.Equals(sectionB))
                {
                    QueueClip(sectionA);
                }
            }
            else
            {
                if (currentSource.Equals(sectionACalm) || currentSource.Equals(sectionA))
                {
                    QueueClip(sectionBCalm);
                }
                else if (currentSource.Equals(sectionBCalm) || currentSource.Equals(sectionB))
                {
                    QueueClip(sectionACalm);
                }
            }
        }
    }

    private void PlayClip(AudioSource clip)
    {
        currentSource = clip;
        clip.Play();
    }

    public void QueueClip(AudioSource clipToQueue)
    {
        queuedClip = clipToQueue;
    }

    public void QueueOutro()
    {
        outroQueued = true;

        if (introSource.clip.Equals(sectionACalm) || introSource.clip.Equals(sectionA))
        {
            QueueClip(outroA);
        }
        else if (introSource.clip.Equals(sectionBCalm) || introSource.clip.Equals(sectionB))
        {
            QueueClip(outroB);
        }
    }
}
