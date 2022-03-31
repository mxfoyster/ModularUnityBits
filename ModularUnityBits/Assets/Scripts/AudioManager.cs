using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    //our audio handles
    [SerializeField] private AudioSource soundClipSource;
    [SerializeField] private AudioSource BkgSoundClipSource;
    [SerializeField] private AudioClip[] Clips;
    [SerializeField] private AudioClip[] audioTracks;
    [SerializeField] private string[] trackDetails;
    [SerializeField] private Text trackDetailsPlace;
    [SerializeField] private Text playButtonText;
    [SerializeField] private Text trackNumberText;

    [SerializeField] private Toggle muteAllToggle, muteMusicToggle;
    [SerializeField] private Slider masterVol,effectsVol,musicVol;

    [HideInInspector] public bool SoundMuted; //mute status

    private int currentTrack;

    public void Start()
    {
        SoundMuted = false;
        currentTrack = 0;
        SwapTracks();
    }

    private void Update()
    {
       
    }

    /// <summary>
    /// Control the state of our background sound.
    /// Checks unmuteSound status.
    /// </summary>
    /// <param name="soundState">"play", "pause", "resume" and "stop".</param>
    public void BkgSound(string soundState)
    {
        if (!SoundMuted)
            switch (soundState)
            {
                case "play":
                    BkgSoundClipSource.Play();
                    break;
                case "pause":
                    BkgSoundClipSource.Pause();
                    break;
                case "resume":
                    BkgSoundClipSource.UnPause();
                    break;
                case "stop":
                    BkgSoundClipSource.Stop();
                    break;
            }
    }


    /// <summary>
    /// Toggles between muted and unmuted
    /// </summary>
    public void OnToggleAudio()
    {
        if (muteAllToggle.isOn)
        {
            BkgSoundClipSource.Pause();
            SoundMuted = true;
            muteMusicToggle.enabled = false;
            masterVol.enabled = false;

        }
        else
        {
            BkgSoundClipSource.UnPause();
            SoundMuted=false;
            muteMusicToggle.enabled = true;
            masterVol.enabled = true;
            OnToggleMusic(); //to make sure when we switch back on music is how we left it
        }
    }

    /// <summary>
    /// Toggles just the music
    /// </summary>
    public void OnToggleMusic()
    {
        if (muteMusicToggle.isOn)
            BkgSoundClipSource.Pause();
        
        else
            BkgSoundClipSource.UnPause();
    }

    /// <summary>
    /// Master fader method.
    /// This controls the other two faders too when adjusted 
    /// </summary>
    public void OnMasterVolAdjust()
    {
        //adjust volumes
        BkgSoundClipSource.volume = masterVol.value;
        soundClipSource.volume = masterVol.value;
        
        //change the other sliders to match
        effectsVol.value = masterVol.value;
        musicVol.value = masterVol.value;
    }

    /// <summary>
    /// Effects fader method
    /// </summary>
    public void OnEffectsVolAdjust()
    {
        soundClipSource.volume = effectsVol.value;
    }

    /// <summary>
    /// Music fader method
    /// </summary>
    public void OnMusicVolAdjust()
    {
        BkgSoundClipSource.volume = musicVol.value;
    }

    /// <summary>
    /// method for all effect butoons
    /// </summary>
    /// <param name="index">The index of sound clip array to play</param>
    public void OnPlayClip(int index)
    {
        if (!SoundMuted) soundClipSource.PlayOneShot(Clips[index]);
    }

    public void OnPlayPauseButton()
    {
        if (playButtonText.text == ">")
        {
            playButtonText.text = "||";
            BkgSoundClipSource.Pause();
        }
        else
        {
            playButtonText.text = ">";
            BkgSoundClipSource.UnPause();
        }
    }

    public void OnBackButton()
    {
        if (currentTrack > 0)
        {
            currentTrack--;
        }
        else 
        {
            currentTrack = trackDetails.Length - 1;
        }
        SwapTracks();
    }

    public void OnForwardButton()
    {
        if (currentTrack < trackDetails.Length-1)
        {
            currentTrack++;
        }
        else
        {
            currentTrack = 0;
        }
       SwapTracks();
    }
    public void SwapTracks()
    {
        playButtonText.text = ">";
        trackNumberText.text = "Now Playing Track " + (currentTrack + 1).ToString() + ":";
        trackDetailsPlace.text = trackDetails[currentTrack];
        BkgSoundClipSource.clip = audioTracks[currentTrack];
        BkgSoundClipSource.Play();
    }

}
