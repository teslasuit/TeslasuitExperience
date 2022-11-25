using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiRotation : MonoBehaviour {

    [SerializeField] private Transform target;
	void Update () {
		transform.localRotation=Quaternion.Inverse(target.localRotation);
	}
}
