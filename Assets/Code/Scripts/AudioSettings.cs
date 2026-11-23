using UnityEngine;

public class AudioSettings : MonoBehaviour
{
    private static float masterVolume = 1f;
    private static bool isMuted = false;

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