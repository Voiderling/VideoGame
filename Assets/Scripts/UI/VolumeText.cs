using UnityEngine;
using UnityEngine.UI;

public class VolumeText : MonoBehaviour
{
    [SerializeField] private string volumeName;
    [SerializeField] private string textIntro; // Sound or music
    private Text txt;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        txt = GetComponent<Text>();
    }
    private void Update()
    {
        UpdateVolume();
    }
    private void UpdateVolume()
    {
        float volume = PlayerPrefs.GetFloat(volumeName) * 100;
        txt.text = textIntro + volume.ToString();
    }
}
