using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using UnityEngine.Video;

public class MainMenuTransition : MonoBehaviour
{
    private bool started = false;
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject initialScreen;
    [SerializeField] private GameObject secondScreen;
    [SerializeField] private GameObject staticScren;
    [SerializeField] private GameObject MainScreen;
    [SerializeField] private VideoPlayer player;
    private AnimatorStateInfo animatorState;

    private void Update()
    {
        animatorState = cam.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);

        if (!started && Input.GetKeyDown(KeyCode.Space))
        {
            started = true;
            cam.GetComponent<Animator>().SetTrigger("StartTransition");
        }
        if (animatorState.IsName("CameraZoom") && animatorState.normalizedTime > 0.1f && animatorState.normalizedTime < 1.0f)
        {
            cam.GetComponent<PostProcessVolume>().profile.GetSetting<MotionBlur>().enabled.value = true;
        }


        if (animatorState.IsName("CameraZoom") && animatorState.normalizedTime > 0.8f && animatorState.normalizedTime < 1.0f)
        {
            player.Play();
            staticScren.SetActive(true);
            cam.GetComponent<PostProcessVolume>().profile.GetSetting<MotionBlur>().enabled.value = false;
        }

        if (started && animatorState.IsName("CameraZoom") && animatorState.normalizedTime >= 1.0f && !player.isPlaying)
        {
            started = false;
            secondScreen.SetActive(true);
            MainScreen.SetActive(true);
            staticScren.SetActive(false);
            cam.GetComponent<Animator>().SetTrigger("EndTransition");
        }

    }
}