using System.Collections;
using UnityEditor;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class EndGameScreenManager : MonoBehaviour
{
    [SerializeField] private PostProcessVolume postProcessVolume;
    [SerializeField] private GameObject p1WinScreen;
    [SerializeField] private GameObject p2WinScreen;
    [SerializeField] private Image gameOverText;
    [SerializeField] private GameObject gameplayUI;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;
    public void ShowTheWinner(int playerIdx) {
        Time.timeScale = 0.5f;

        gameplayUI.SetActive(false);
        gameOverText.color = new Color(1, 1, 1, 0);
        gameOverText.gameObject.SetActive(true);
        CameraManager.Instance.SetPreGameCamActive();

        StartCoroutine(DofCour());

        Sequence sequence = DOTween.Sequence();
        Vector2 endPos = gameOverText.transform.localPosition;
        Vector2 startPos = endPos + new Vector2(0, -100);
        gameOverText.transform.localPosition = startPos;
        sequence.Append(gameOverText.DOFade(1, 1f));
        sequence.Join(gameOverText.transform.DOLocalMove(endPos, 1f));
        sequence.AppendInterval(1);

        GameObject winnerScreen = playerIdx == 0 ? p1WinScreen : p2WinScreen;
        winnerScreen.SetActive(true);

        // position winner screen off screen to up
        winnerScreen.transform.localPosition = new Vector3(0, winnerScreen.GetComponent<RectTransform>().rect.height, 0);
        sequence.Append(winnerScreen.transform.DOLocalMove(new Vector3(0, 0, 0), 0.5f).SetEase(Ease.OutCubic));
        sequence.AppendInterval(0.5f);
        sequence.AppendCallback(() => {
            restartButton.GetComponent<CanvasGroup>().alpha = 0;
            restartButton.gameObject.SetActive(true);
            mainMenuButton.GetComponent<CanvasGroup>().alpha = 0;
            mainMenuButton.gameObject.SetActive(true);
        });

        sequence.Append(restartButton.GetComponent<CanvasGroup>().DOFade(1, 0.5f));
        sequence.Join(mainMenuButton.GetComponent<CanvasGroup>().DOFade(1, 0.5f));
    }

    private IEnumerator DofCour() {
        var dof = postProcessVolume.profile.GetSetting<DepthOfField>();
        dof.active = true;
        float endValue = dof.focusDistance.value;
        float duration = 1f;
        
        for (float t = 0; t < duration; t += Time.deltaTime) {
            dof.focusDistance.value = Mathf.Lerp(0, endValue, t / duration);
            yield return null;
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(EndGameScreenManager))]
public class EndGameScreenManagerEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        EndGameScreenManager myScript = (EndGameScreenManager)target;
        if(GUILayout.Button("Show The Winner")) {
            myScript.ShowTheWinner(0);
        }
    }
}

#endif