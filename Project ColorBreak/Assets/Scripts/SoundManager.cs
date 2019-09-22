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

    private AudioSource bgmPlayer;

    public string startBgm;


    //------------------------------멤버 변수-------------------------------------

    private void Awake()
    {
        if (m_instance == null)
            m_instance = this;
        else if (m_instance != this)
            DestroyImmediate( this.gameObject );

        bgmPlayer = GetComponent<AudioSource>();

    }

    void Start()
    {
        LoadFiles();

        PlayBGM( startBgm );

    }

    // Update is called once per frame
    void Update()
    {

    }

    void LoadFiles()
    {
        AudioClip[] bgms = Resources.LoadAll<AudioClip>( "Sound/BGM" );
        AudioClip[] SFXs = Resources.LoadAll<AudioClip>( "FX" );

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

    public AudioClip FindAudioClip( string name, SoundType type = SoundType.BGM )
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
        bgmPlayer.clip = FindAudioClip( name );
        bgmPlayer.Play();
    }

    public void StopBGM()
    {
        bgmPlayer.Stop();
    }




}
