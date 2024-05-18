using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class BlurController : MonoBehaviour
{

    [SerializeField] private PostProcessProfile profile;
    bool decrease = true;
    public float blur = 10;
    public bool lensActive = false;
    private void Awake()
    {
        profile.GetSetting<MotionBlur>().enabled.value = lensActive;


    }
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    decrease = true;
        //}

        //if(decrease && profile.GetSetting<DepthOfField>().focusDistance.value > 0.1)
        //{
        //    print("BLUR: " + blur);
        //    blur = blur - 1;
        //    print("after BLUR: " + blur);

        //    print("value: " + profile.GetSetting<DepthOfField>().focusDistance.value);
        //    //profile.GetSetting<DepthOfField>().focusDistance = new FloatParameter { value = blur };
        //    profile.GetSetting<DepthOfField>().focusDistance.value = blur;
        //}

        //if(profile.GetSetting<DepthOfField>().focusDistance.value <= 0.1)
        //{
        //    decrease = false;
        //}

        //if (decrease)
        //{
        //    profile.GetSetting<DepthOfField>().focusDistance.value = blur;

        //}
        //else
        //{
        //    profile.GetSetting<DepthOfField>().focusDistance.value = 10f;

        //}

        profile.GetSetting<MotionBlur>().enabled.value = lensActive;
        print("active: " + profile.GetSetting<MotionBlur>().enabled.value);



    }
}
