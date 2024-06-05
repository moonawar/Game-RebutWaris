using System.Collections;
using UnityEngine;

public class LoaderCallback : MonoBehaviour
{
    private bool _isFirstUpdate = true;
    private void Update()
    {
        if (_isFirstUpdate)
        {
            _isFirstUpdate = false;
            StartCoroutine(Callback());
        }
    }

    private IEnumerator Callback() {
        yield return new WaitForSeconds(5f);
        FlowManager.LoaderCallback();
    }
}
