using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    
    public RectTransform targetObject;    

    void Start () {          
    }	
	
	void LateUpdate () {
        Vector3 curTargetPos = transform.position;
        curTargetPos.y = targetObject.position.y;
        curTargetPos.x = targetObject.position.x;

        //transform.position = Vector3.Lerp(transform.position, curTargetPos, Time.deltaTime * 1f);        
        transform.position = curTargetPos;
    }

}
