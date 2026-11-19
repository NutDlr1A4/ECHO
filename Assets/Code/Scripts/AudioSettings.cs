using UnityEngine;

public class AudioSettings : MonoBehaviour
{
    private float masterVolume = 1f;
    private bool isMuted = false;

    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;
        UpdateVolume();
    }

    public void SetMute(bool muted)
    {
        isMuted = muted;
        UpdateVolume();
    }

    private void UpdateVolume()
    {
        AudioListener.volume = isMuted ? 0f : masterVolume;
    }
}