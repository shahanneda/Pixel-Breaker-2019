using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class HowToPlay : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public Text tutorialText;

    public void PlayTutorialClip(VideoClip videoClip)
    {
        videoPlayer.gameObject.SetActive(true);
        videoPlayer.clip = videoClip;
        videoPlayer.aspectRatio = VideoAspectRatio.FitOutside;
        videoPlayer.Play();
    }

    public void ShowText(string text)
    {
        tutorialText.text = text;
    }

    public void StopTutorialClip()
    {
        videoPlayer.Stop();
        videoPlayer.clip = null;
        videoPlayer.gameObject.SetActive(false);
    }

    public void HideText()
    {
        tutorialText.text = "";
    }
}
