using UnityEngine;
using UnityEngine.EventSystems;

public class MenuSFX : MonoBehaviour,
    ISelectHandler,
    IPointerEnterHandler,
    IPointerDownHandler,
    ISubmitHandler
{
    public AudioSource audioSource;
    public AudioClip navigateSound;
    public AudioClip clickSound;

    public void OnSelect(BaseEventData eventData)
    {
        PlayNavigateSound();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PlayNavigateSound();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            PlayClickSound();
    }

    public void OnSubmit(BaseEventData eventData)
    {
        PlayClickSound();
    }

    private void PlayNavigateSound()
    {
        if (!audioSource.isPlaying)
            audioSource.PlayOneShot(navigateSound);
    }

    private void PlayClickSound()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(clickSound);
    }
}