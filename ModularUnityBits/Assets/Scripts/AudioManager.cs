using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    //our audio handles
    [SerializeField] private AudioSource soundClipSource;
    [SerializeField] private AudioSource BkgSoundClipSource;
    [SerializeField] private AudioClip[] Clips;
   
    [SerializeField] private Toggle muteAllToggle, muteMusicToggle;
    [SerializeField] private Slider masterVol,effectsVol,musicVol;

    [HideInInspector] public bool SoundMuted; //mute status



    public void Start()
    {
        SoundMuted = false;
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

    public void OnMasterVolAdjust()
    {
        //adjust volumes
        BkgSoundClipSource.volume = masterVol.value;
        soundClipSource.volume = masterVol.value;
        
        //change the other sliders to match
        effectsVol.value = masterVol.value;
        musicVol.value = masterVol.value;
    }

    public void OnEffectsVolAdjust()
    {
        soundClipSource.volume = effectsVol.value;
    }

    public void OnMusicVolAdjust()
    {
        BkgSoundClipSource.volume = musicVol.value;
    }

    public void OnPlayClip(int index)
    {
        if (!SoundMuted) soundClipSource.PlayOneShot(Clips[index]);
    }
}
