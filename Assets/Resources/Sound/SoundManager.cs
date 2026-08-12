using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioSource bgm;
    [SerializeField] private AudioSource stir;
    [SerializeField] private AudioSource shake;
    [SerializeField] private AudioSource pour;
    [SerializeField] private AudioSource breakenGlass;
    [SerializeField] private AudioSource startBell;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    public void BGM()
    {
        bgm.Play();
    }

    public void PlayStir() 
    { 
        stir.Play(); 
    }

    public void StopStir() 
    { 
        stir.Stop(); 
    }

    public void PlayShake() 
    {
        shake.Play(); 
    }

    public void StopShake()
    {
        shake.Stop(); 
    }

    public void PlayPour() 
    {
        pour.Play(); 
    }

    public void StopPour() 
    {
        pour.Stop();
    }

    public void StartGameSound()
    {
        startBell.Play();
    }

    public void BreakGlassSound()
    {
        breakenGlass.Play();
    }
}
