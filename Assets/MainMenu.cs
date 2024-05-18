using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class MainMenu : MonoBehaviour
{
    public bool started = false;
    [SerializeField] private Camera cam;
    [SerializeField] private float zoomRate;
    [SerializeField] private float moveRate;
    [SerializeField] private GameObject initialScreen;
    [SerializeField] private GameObject secondScreen;
    [SerializeField] private GameObject staticScren;
    [SerializeField] private float transitionDuration;
    [SerializeField] private GameObject MainScreen;
    [SerializeField] private VideoPlayer player;
    [SerializeField] private BlurController blurController;
    private AnimatorStateInfo animatorState;

    private void Update()
    {
        animatorState = cam.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);

        if (!started && Input.GetKeyDown(KeyCode.Space))
        {
            started = true;
            cam.GetComponent<Animator>().SetTrigger("StartTransition");
            //player.Play();
            //staticScren.SetActive(true);
            //secondScreen.SetActive(true);
            //MainScreen.SetActive(true);
        }
        if (animatorState.IsName("CameraZoom") && animatorState.normalizedTime > 0.1f && animatorState.normalizedTime < 1.0f)
        {
            //blurController.blur = 10;
            blurController.lensActive = true;


        }


        if (animatorState.IsName("CameraZoom") && animatorState.normalizedTime > 0.8f && animatorState.normalizedTime < 1.0f)
        {
            //blurController.blur = 10;
            player.Play();
            staticScren.SetActive(true);
            blurController.lensActive = false;

            
        }

        //if (animatorState.IsName("CameraZoom") )
        //{
        //    blurController.lensActive = false;
        //    player.Play();
        //    staticScren.SetActive(true);
        //    secondScreen.SetActive(true);
        //    MainScreen.SetActive(true);
        //}


        if (started && animatorState.IsName("CameraZoom") && animatorState.normalizedTime >= 1.0f && !player.isPlaying)
        {
            started = false;
            secondScreen.SetActive(true);
            MainScreen.SetActive(true);
            staticScren.SetActive(false);
            cam.GetComponent<Animator>().SetTrigger("EndTransition");
            //StartCoroutine(EndTransition());
        }

    }

    private IEnumerator EndTransition()
    {
        yield return new WaitForSeconds(1);
        staticScren.SetActive(false);
        cam.GetComponent<Animator>().SetTrigger("EndTransition");
    }

    private void FixedUpdate()
    {

        

        //if(started && cam.orthographicSize > 1.7)
        //{
        //    cam.orthographicSize -= zoomRate;
        //}

        //if(started && cam.transform.position.x <= 1.6)
        //{
        //    cam.transform.position += new Vector3(moveRate, 0, 0);
        //}

        //if (started && cam.transform.position.x >= 1.6 && cam.orthographicSize <= 1.7)
        //{
        //    initialScreen.SetActive(false);
        //    secondScreen.SetActive(true);
        //}
    }
}
