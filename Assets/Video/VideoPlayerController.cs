using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoPlayerController : MonoBehaviour
{
    public static VideoPlayerController Instance;

    // ACT = 0, SART_PRACTICE = 2, SART_TEST = 3, BASKET = 4, CORE = 5, QUESTIONAIRE = 1 };
    [SerializeField] private List<VideoTherapy> listOfVideos;
    [SerializeField] private RawImage rawImage;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private CanvasGroup videoPlayerPanel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        videoPlayer.loopPointReached += VideoPlayer_loopPointReached;
        videoPlayer.prepareCompleted += VideoPlayer_prepareCompleted;
    }

    private void VideoPlayer_loopPointReached(VideoPlayer source)
    {
        StopVideo();
    }

    private void VideoPlayer_prepareCompleted(VideoPlayer source)
    {
        videoPlayerPanel.alpha = 1;
        videoPlayerPanel.interactable = true;
        videoPlayerPanel.blocksRaycasts = true;
        rawImage.texture = source.texture;
        source.Play();
    }

    public void PlayVideo()
    {
        videoPlayer.Prepare();
    }

    public void StopVideo()
    {
        videoPlayer.Stop();
        videoPlayerPanel.alpha = 0;
        videoPlayerPanel.interactable = false;
        videoPlayerPanel.blocksRaycasts = false;
    }

    public void SetVideo(TherapyLadderStep therapyStep)
    {
        videoPlayer.clip = listOfVideos.Where(therapy => therapy.TherapyStep == therapyStep).SingleOrDefault().VideoClip;
    }
}

[Serializable]
public struct VideoTherapy
{
    public TherapyLadderStep TherapyStep;
    public VideoClip VideoClip;
}