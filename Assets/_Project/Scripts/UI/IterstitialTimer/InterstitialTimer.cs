using System.Collections;
using UnityEngine;
using YG;

public class InterstitialTimer : MonoBehaviour
{
    [Tooltip("Объект таймера перед показом рекламы. Он будет активироваться и деактивироваться в нужное время.")]
    [SerializeField] private GameObject secondsPanelObject;
    [Tooltip("Массив объектов, которые будут показываться по очереди через секунду." +
        "Сколько объектов вы поместите в массив, столько секунд будет отчитываться перед показом рекламы." +
        "\n\nНапример, поместите в массив три объекта:" +
        " певый с текстом '3', второй с текстом '2', третий с текстом '1'." +
        "\nВ таком случае произойдёт отчет трёх секунд с показом объектов с цифрами перед рекламой.")]
    [SerializeField] private GameObject[] secondObjects;

    private IInterstitialAdvService _interstitialService;
    private Coroutine timerAdShowCoroutine;
    private Coroutine backupTimerClosureCoroutine;
    private int objSecCounter;
    private bool _isShowing;

    private void OnDestroy()
    {
        _interstitialService.ShowInterstitial -= ShowTimerAndAdv;
    }

    public void Construct(IInterstitialAdvService interstitialAdvService)
    {
        _interstitialService = interstitialAdvService;
        _interstitialService.ShowInterstitial += ShowTimerAndAdv;
    }

    private void ShowTimerAndAdv()
    {
        if (_isShowing)
            return;

        _isShowing = true;

        objSecCounter = 0;

        if (secondsPanelObject)
            secondsPanelObject.SetActive(true);

        timerAdShowCoroutine = StartCoroutine(TimerAdShow());

        YG2.PauseGame(true);
    }

    private IEnumerator TimerAdShow()
    {
        while (true)
        {
            if (objSecCounter < secondObjects.Length)
            {
                for (int i = 0; i < secondObjects.Length; i++)
                    secondObjects[i].SetActive(false);

                secondObjects[objSecCounter].SetActive(true);
                objSecCounter++;

                yield return new WaitForSecondsRealtime(1.0f);
            }

            if (objSecCounter == secondObjects.Length)
            {
                YG2.InterstitialAdvShow();
                backupTimerClosureCoroutine = StartCoroutine(BackupTimerClosure());

                while (!YG2.nowInterAdv)
                    yield return null;

                RestartTimer();
                yield break;
            }
        }
    }

    IEnumerator BackupTimerClosure()
    {
        yield return new WaitForSecondsRealtime(2f);

        if (objSecCounter != 0)
        {
            RestartTimer();
            YG2.PauseGame(false);
        }

        backupTimerClosureCoroutine = null;
    }

    private void RestartTimer()
    {
        secondsPanelObject.SetActive(false);
        foreach (var obj in secondObjects)
            obj.SetActive(false);

        objSecCounter = 0;

        if (timerAdShowCoroutine != null)
        {
            StopCoroutine(timerAdShowCoroutine);
            timerAdShowCoroutine = null;
        }

        if (backupTimerClosureCoroutine != null)
        {
            StopCoroutine(backupTimerClosureCoroutine);
            backupTimerClosureCoroutine = null;
        }

        _interstitialService.OnHideTimer();
        _isShowing = false;
    }
}