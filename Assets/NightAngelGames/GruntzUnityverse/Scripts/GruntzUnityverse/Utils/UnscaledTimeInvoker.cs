using System.Collections;
using UnityEngine;

public class UnscaledTimeInvoker : MonoBehaviour
{
    public void InvokeRepeatingUnscaled(string methodName, float initialDelay, float repeatRate)
    {
        StartCoroutine(InvokeRepeatingCoroutine(methodName, initialDelay, repeatRate));
    }

    private IEnumerator InvokeRepeatingCoroutine(string methodName, float initialDelay, float repeatRate)
    {
        // Initial delay
        yield return new WaitForSecondsRealtime(initialDelay);

        // Repeat indefinitely
        while (true)
        {
            SendMessage(methodName); // Call the method by name
            yield return new WaitForSecondsRealtime(repeatRate);
        }
    }

    public void CancelInvokeUnscaled(string methodName)
    {
        StopCoroutine(nameof(InvokeRepeatingCoroutine));
    }
}
