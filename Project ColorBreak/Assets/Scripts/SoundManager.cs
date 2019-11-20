using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    BGM,
    SFX
}

public class SoundManager : MonoBehaviour
{

    public static SoundManager m_instance;
    public static SoundManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = Transform.FindObjectOfType<SoundManager>();
            }
            return m_instance;
        }

    }

    public Dictionary<SoundType, List<AudioClip>> audioClips = new Dictionary<SoundType, List<AudioClip>>();

    public AudioSource bgmPlayer;
    public AudioSource sfxPlayer;

    //------------------------------멤버 변수-------------------------------------

    private void Awake()
    {
        if (m_instance == null)
            m_instance = this;
        else if (m_instance != this)
            DestroyImmediate( this.gameObject );

    }

    void Start()
    {
        LoadFiles();
    }

    void LoadFiles()
    {
        AudioClip[] bgms = Resources.LoadAll<AudioClip>( "Sound/BGM" );
        AudioClip[] SFXs = Resources.LoadAll<AudioClip>( "Sound/Effect" );

        List<AudioClip> bgmList = new List<AudioClip>();
        List<AudioClip> sfxList = new List<AudioClip>();

        foreach (var bgm in bgms)
        {
            bgmList.Add( bgm );
        }
        audioClips.Add( SoundType.BGM, bgmList );

        foreach (var sfx in SFXs)
        {
            sfxList.Add( sfx );
        }
        audioClips.Add( SoundType.SFX, sfxList );

    }

    public AudioClip FindAudioClip( string name, SoundType type)
    {
        AudioClip result = null;

        List<AudioClip> list;
        audioClips.TryGetValue( type, out list );

        foreach (var sound in list)
        {
            if (sound.name == name)
            {
                result = sound;
            }
        }

        return result;
    }

    public void PlayBGM()
    {
        bgmPlayer.Play();
    }

    public void PlayBGM( string name )
    {
        bgmPlayer.clip = FindAudioClip( name, SoundType.BGM );
        bgmPlayer.Play();
    }

    public void StopBGM()
    {
        bgmPlayer.Stop();
    }

    public void PlaySFX( string name )
    {
        sfxPlayer.clip = FindAudioClip( name, SoundType.SFX );
        sfxPlayer.Play();
    }
}
