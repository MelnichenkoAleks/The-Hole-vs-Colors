using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
	#region Singleton class: UIManager

	public static UIManager Instance;

	void Awake ()
	{
		if (Instance == null) {
			Instance = this;
		}
	}

	#endregion

	[Header ("Level Progress UI")]
	
	[SerializeField] int sceneOffset;
	[SerializeField] TMP_Text nextLevelText;
	[SerializeField] TMP_Text currentLevelText;
	[SerializeField] Image progressFillImage;

	[Space]
	[SerializeField] TMP_Text levelCompletedText;

	[Space]
    // Белая затухающая панель в начале
    [SerializeField] Image fadePanel;

	void Start ()
	{
		FadeAtStart ();
		
		progressFillImage.fillAmount = 0f;

		SetLevelProgressText ();
	}

	void SetLevelProgressText ()
	{
		int level = SceneManager.GetActiveScene ().buildIndex + sceneOffset;
		currentLevelText.text = level.ToString ();
		nextLevelText.text = (level + 1).ToString ();
	}

	public void UpdateLevelProgress ()
	{
		float val = 1f - ((float)Level.Instance.objectsInScene / Level.Instance.totalObjects);		
		progressFillImage.DOFillAmount (val, .4f);
	}

	//--------------------------------------
	public void ShowLevelCompletedUI ()
	{		
		levelCompletedText.DOFade (1f, .6f).From (0f);
	}

	public void FadeAtStart ()
	{		
		fadePanel.DOFade (0f, 1.3f).From (1f);
	}
}
