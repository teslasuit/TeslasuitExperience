using UnityEngine;
using System.Collections;

public class TriggerRot : MonoBehaviour {
    [SerializeField] private Hand hand;
    [SerializeField] private float minRot = -11.6734f;
    [SerializeField] private float maxRot = 20f;
    [SerializeField] private float minValueInput = 0;
    [SerializeField] private float maxValueInput = 1;
    public Hand HandDevice { set { hand=value;
	} }

	void Update () {
        if(hand==null)
            return;
	    if(hand.GetTrigger()>minValueInput)
	        transform.localEulerAngles = new Vector3(minRot + (maxRot - minRot)*(hand.GetTrigger() - minValueInput) / (maxValueInput - minValueInput), 0);
        else
            transform.localEulerAngles = new Vector3(minRot , 0);
        
	}
}
