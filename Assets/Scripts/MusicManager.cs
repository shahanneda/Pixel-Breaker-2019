using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource intro;

    [Space]

    public AudioSource sectionACalm;
    public AudioSource sectionBCalm;

    [Space]

    public AudioSource sectionA;
    public AudioSource sectionB;

    [Space]

    public AudioSource outroA;
    public AudioSource outroB;

    [Space]

    public bool tense = false;
    private bool previousTense = false;

    [SerializeField] private AudioSource currentSong;
    private AudioSource queuedSong;

    private bool isOutroQueued = false;
    private bool ending;

    private TileManager tileManager;

    private void Start()
    {
        queuedSong = intro;

        PlayQueuedSong();
        QueueSong(sectionACalm);

        tileManager = FindObjectOfType<TileManager>();
    }

    private void Update()
    {
        if (isOutroQueued)
        {
            if (!ending)
            {
                currentSong.volume -= Time.deltaTime / 1.5f;
                tileManager.isPlaying = false;

                if (currentSong.volume <= 0)
                {
                    FindObjectOfType<GameManager>().GameOver();

                    PlayQueuedSong();

                    ending = true;
                    enabled = false;
                }
            }
        }
    }

    public void CheckNextSong()
    {
        if (!isOutroQueued)
        {
            tense = tileManager.CheckIfTense();

            if (tense)
            {
                if (tense != previousTense)
                {
                    if (currentSong.Equals(intro) || currentSong.Equals(sectionACalm) || currentSong.Equals(sectionB))
                    {
                        QueueSong(sectionA);
                    }
                    else if ((currentSong.Equals(sectionBCalm) || currentSong.Equals(sectionA)) && !currentSong.isPlaying)
                    {
                        QueueSong(sectionB);
                    }
                }
            }
            else
            {
                if (currentSong.Equals(sectionACalm) || currentSong.Equals(sectionA))
                {
                    QueueSong(sectionBCalm);
                }
                else if (currentSong.Equals(sectionBCalm) || currentSong.Equals(sectionB))
                {
                    QueueSong(sectionACalm);
                }
            }

            previousTense = tense;
            PlayQueuedSong();
        }
    }

    private void PlayQueuedSong()
    {
        if (queuedSong != currentSong)
        {
            float percentDone = 0;

            if (currentSong != null && !isOutroQueued)
            {
                percentDone = currentSong.time / currentSong.clip.length;
                currentSong.Stop();
            }

            float newSongTime = queuedSong.clip.length * percentDone;

            currentSong = queuedSong;
            queuedSong.time = newSongTime;
            queuedSong.Play();
        }
    }

    public void QueueSong(AudioSource songToQueue)
    {
        queuedSong = songToQueue;
    }

    public void QueueOutro()
    {
        isOutroQueued = true;

        if (currentSong.Equals(intro) || currentSong.Equals(sectionACalm) || currentSong.Equals(sectionA))
        {
            QueueSong(outroA);
        }
        else if (currentSong.Equals(sectionBCalm) || currentSong.Equals(sectionB))
        {
            QueueSong(outroB);
        }
    }
}
