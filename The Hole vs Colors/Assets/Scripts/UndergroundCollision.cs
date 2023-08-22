using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class UndergroundCollision : MonoBehaviour
{
    public int levelToUnlock;
    int numberOfUnlockedLevels;

    public AudioSource audioSource;
	public AudioClip audioClipVictory;
    public AudioClip audioClipGameOver;
	public AudioClip audioClipGameCube;


    void OnTriggerEnter (Collider other)
	{
        // Объект или препятствие находится внизу ямы
        if (!Game.isGameover) 
		{
			string tag = other.tag;
			if (tag.Equals ("Object"))
			{
                audioSource.PlayOneShot(audioClipGameCube);
                Level.Instance.objectsInScene--;
				UIManager.Instance.UpdateLevelProgress ();

				Magnet.Instance.RemoveFromMagnetField (other.attachedRigidbody);

				Destroy (other.gameObject);
			
				if (Level.Instance.objectsInScene == 0) 
				{
                    // больше нет объектов для сбора (ПОБЕДА)
                    audioSource.PlayOneShot(audioClipVictory);
                    UIManager.Instance.ShowLevelCompletedUI ();
					Level.Instance.PlayWinFx ();

					numberOfUnlockedLevels = PlayerPrefs.GetInt("levelsUnlocked");
					if (numberOfUnlockedLevels <= levelToUnlock)
					{
						PlayerPrefs.SetInt("levelsUnlocked", numberOfUnlockedLevels + 1);
					}

                    // Загрузка следующего уровня через 2 секунды
                    Invoke("NextLevel", 2f);
				}
			}
			

			if (tag.Equals ("Obstacle")) 
			{
                audioSource.PlayOneShot(audioClipGameOver);
                Game.isGameover = true;
				Destroy (other.gameObject);

                // Встряхнуть камеру в течение 1 секунды
                Camera.main.transform.DOShakePosition (1f, .2f, 20, 90f).OnComplete (() => 
					{                        
                        Level.Instance.RestartLevel ();
					});
			}
		}
	}

	void NextLevel ()
	{
		Level.Instance.LoadNextLevel ();
	}
		
}
