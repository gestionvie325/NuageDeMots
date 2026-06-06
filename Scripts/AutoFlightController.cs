using UnityEngine;
using System.Collections;

public class AutoFlightController : MonoBehaviour
{
    public static AutoFlightController instance;

    public bool isAutoFlying = false;

    public float waitOnWord = 3f;

    private Coroutine flightRoutine;

    void Awake()
    {
        instance = this;
    }

    public void ToggleAutoFlight()
    {
        isAutoFlying = !isAutoFlying;

        if (isAutoFlying)
        {
            flightRoutine = StartCoroutine(AutoFlightRoutine());
            Debug.Log("Vol automatique activé");
        }
        else
        {
            if (flightRoutine != null)
            {
                StopCoroutine(flightRoutine);
            }

            Debug.Log("Vol automatique désactivé");
        }
    }

  IEnumerator AutoFlightRoutine()
{
    while (isAutoFlying)
    {
        WordBehaviour randomWord = GetRandomWordInFront();

        if (randomWord != null)
        {
            BottomPanelUI.instance.ShowDefinition(randomWord.definition);
            CameraFocusController.instance.FocusOnWord(randomWord.transform);
        }

        yield return new WaitForSeconds(waitOnWord);
    }
}

WordBehaviour GetRandomWordInFront()
{
    var candidates = WordBehaviour.allWords.FindAll(w =>
        w.transform.position.z >
        Camera.main.transform.position.z + 15f
    );

    if (candidates.Count == 0)
        return null;

    return candidates[
        Random.Range(0, candidates.Count)
    ];
}
}
