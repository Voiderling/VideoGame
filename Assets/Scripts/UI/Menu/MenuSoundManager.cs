using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Unity.VisualScripting.Member;

public class MenuSoundManager : MonoBehaviour
{
    public static MenuSoundManager instance { get; private set; }
    private AudioSource source;
    private AudioSource musicSource;
    private bool hasBounds = true;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        source = GetComponent<AudioSource>();
        if(hasBounds )
            musicSource = transform.GetChild(0).GetComponent<AudioSource>();
        else
            musicSource = transform.GetComponent<AudioSource>();
        ChangeMusicVolume(0);
        ChangeSoundVolume(0);
    }
    public void PlaySound(AudioClip _sound)
    {
        source.PlayOneShot(_sound);
    }
    public void DisableSound()
    {
        source.Stop();
    }
    public void ChangeSoundVolume(float _change)
    {
        ChangeSourceVolume(1, "soundVolume", _change, source);
    }
    public void ChangeMusicVolume(float _change)
    {
        ChangeSourceVolume(1, "musicVolume", _change, musicSource);
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void ChangeSourceVolume(float baseVolume, string volumeName, float change, AudioSource source)
    {
        float currentVolume = PlayerPrefs.GetFloat(volumeName, 1);
        currentVolume += change;

        if (currentVolume > 1)
            currentVolume = 0;
        else if (currentVolume < 0)
        {
            currentVolume = 1;
        }
        float finalVolume = currentVolume * baseVolume;
        source.volume = finalVolume;
        PlayerPrefs.SetFloat(volumeName, currentVolume);
    }
}