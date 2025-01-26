using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource firstAudioSource;  // Audio source for the first sound
    public AudioSource secondAudioSource; // Audio source for the looping sound

    private void Awake()
    {
        // Play the first sound on Awake
        if (firstAudioSource != null)
        {
            firstAudioSource.Play();
            // Start coroutine to check when the first sound is finished
            StartCoroutine(PlayLoopingSoundAfterFirst());
        }
        else
        {
            Debug.LogWarning("First Audio Source is not assigned.");
        }
    }

    private System.Collections.IEnumerator PlayLoopingSoundAfterFirst()
    {
        // Wait for the first sound to finish playing
        while (firstAudioSource.isPlaying)
        {
            yield return null; // Wait for the next frame
        }

        // Once the first sound is done, start playing the looping sound
        if (secondAudioSource != null)
        {
            secondAudioSource.loop = true;
            secondAudioSource.Play();
        }
        else
        {
            Debug.LogWarning("Second Audio Source is not assigned.");
        }
    }
}
