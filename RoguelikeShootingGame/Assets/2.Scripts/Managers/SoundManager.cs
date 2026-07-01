using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineEnum;

public class SoundManager : MonoBehaviour
{
    static SoundManager _uniqueInstance;

    public static SoundManager Instance { get { return _uniqueInstance; } }

    Dictionary<SOUNDENUM, string> SoundName;
    AudioSource _audioSource;

    private void Awake()
    {
        _uniqueInstance = this;

        SoundName = new Dictionary<SOUNDENUM, string>();
        SoundName.Add(SOUNDENUM.UIButtonClick, "SFX_UI_Button_Organic_Plastic_Thin_Generic_1");
        SoundName.Add(SOUNDENUM.SubMapClick, "SFX_UI_Click_Organic_Wooden_Plastic_Negative_Back_2");
        SoundName.Add(SOUNDENUM.SubEnhanceClick, "SFX_UI_Click_Designed_Liquid_Generic_1");
        SoundName.Add(SOUNDENUM.ArrowDraw, "Bow Draw 3");
        SoundName.Add(SOUNDENUM.ArrowRelease, "Bow Release 3");
        SoundName.Add(SOUNDENUM.MonsterArrowHit, "Wooden Arrow Flesh Impact 3");
        SoundName.Add(SOUNDENUM.PlayerHit, "SFX_UI_Click_Designed_Liquid_Generic_Open_1");

        _audioSource = GetComponent<AudioSource>();

        DontDestroyOnLoad(this);
    }

    public void sfxPlay(SOUNDENUM sound, float volume = 1)
    {
        AudioClip audioClip = Resources.Load<AudioClip>("Sound/" + SoundName[sound]);

        _audioSource.PlayOneShot(audioClip, volume);
    }

    public void bgmPlay(SOUNDENUM sound)
    {
        if (_audioSource.isPlaying)
            _audioSource.Stop();

        AudioClip audioClip = Resources.Load<AudioClip>("Sound/" + SoundName[sound]);

        _audioSource.clip = audioClip;
        _audioSource.Play();
    }
}
