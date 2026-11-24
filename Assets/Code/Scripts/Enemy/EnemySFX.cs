using UnityEngine;

public class EnemySFX : MonoBehaviour
{
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip[] walkSoundArr;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip alertSound;
    private AudioSource audioSource;
    private EnemyController enemyController;
    private bool beginingFlag = true;
    private int walkSoundArrIndexTracker = 0;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        enemyController =
            transform.root.GetComponentInChildren<EnemyController>(true);
    }

    private void Update()
    {
        if (enemyController.IsMoving())
        {
            if (beginingFlag)
            {
                PlaySoundWalk();
                beginingFlag = false;
            }
            else
            {
                if (!IsInvoking(nameof(PlaySoundWalk)))
                {
                    Invoke(nameof(PlaySoundWalk),
                        walkSoundArr[walkSoundArrIndexTracker].length);
                }
            }
        }
        else
        {
            CancelInvoke(nameof(PlaySoundWalk));
            beginingFlag = true;
        }
    }

    public void PlaySoundShoot()
    {
        audioSource.PlayOneShot(shootSound,2f);
    }

    public void PlaySoundWalk()
    {
        audioSource.PlayOneShot(
            walkSoundArr[walkSoundArrIndexTracker],2);
        walkSoundArrIndexTracker =
            (walkSoundArrIndexTracker + 1)
            % walkSoundArr.Length;
    }

    public void PlaySoundDeath()
    {
        audioSource.PlayOneShot(deathSound);
    }
    public void PlaySoundAlert()
    {
        audioSource.PlayOneShot(alertSound,4);
        Debug.Log("AlertSound");
    }
}
