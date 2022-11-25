using EasyButtons;
using System.Linq;
using RootMotion.FinalIK;
using UnityEngine;

public class SuitMocapMirror : MonoBehaviour {
 
    [Header("fake IK mocap settings")]
    public VRIK vrik;

    public VRIK.References references;
    

    public static Transform FindInChildren(Transform go, string name)
    {
        return (from x in go.GetComponentsInChildren<Transform>()
                where x.gameObject.name == name
                select x).First();
    }

    [Button]
    private void CopyNodes() {
        

        if (vrik) {
            references.root = vrik.references.root;
            references.pelvis = vrik.references.pelvis;
            references.spine = vrik.references.spine;
            references.chest = vrik.references.chest;
            references.neck = vrik.references.neck;
            references.head = vrik.references.head;
            references.leftShoulder = vrik.references.leftShoulder;
            references.leftUpperArm = vrik.references.leftUpperArm;
            references.leftForearm = vrik.references.leftForearm;
            references.leftHand = vrik.references.leftHand;
            references.rightShoulder = vrik.references.rightShoulder;
            references.rightUpperArm = vrik.references.rightUpperArm;
            references.rightForearm = vrik.references.rightForearm;
            references.rightHand = vrik.references.rightHand;
            references.leftThigh = vrik.references.leftThigh;
            references.leftCalf = vrik.references.leftCalf;
            references.leftFoot = vrik.references.leftFoot;
            references.leftToes = vrik.references.leftToes;
            references.rightThigh = vrik.references.rightThigh;
            references.rightCalf = vrik.references.rightCalf;
            references.rightFoot = vrik.references.rightFoot;
            references.rightToes = vrik.references.rightToes;
            
        }
       
    }


    private void Update() {

        
        if (vrik == null) {
            //if (GameManager.Instance && GameManager.Instance.localPlayerSceleton)
                vrik = GameManager.Instance.LocalIk;
        }
        if(vrik&&vrik.isActiveAndEnabled)
            syncIk();
    }

    

    private void syncIk() {
        if(vrik.references.root&& references.root) { 
            references.root.localRotation=vrik.references.root.localRotation;
            references.root.localPosition=vrik.references.root.localPosition;
        }
        if (vrik.references.pelvis && references.pelvis) {
            references.pelvis.localRotation = vrik.references.pelvis.localRotation;
            references.pelvis.localPosition = vrik.references.pelvis.localPosition;
        }
        if (vrik.references.spine && references.spine) {
            references.spine.localRotation = vrik.references.spine.localRotation;
            references.spine.localPosition = vrik.references.spine.localPosition;
        }
        if (vrik.references.chest && references.chest) {
            references.chest.localRotation = vrik.references.chest.localRotation;
            references.chest.localPosition = vrik.references.chest.localPosition;
        }
        if (vrik.references.neck && references.neck) {
            references.neck.localRotation = vrik.references.neck.localRotation;
            references.neck.localPosition = vrik.references.neck.localPosition;
        }
        if (vrik.references.head && references.head) {
            references.head.localRotation = vrik.references.head.localRotation;
            references.head.localPosition = vrik.references.head.localPosition;
        }
        if (vrik.references.leftShoulder && references.leftShoulder) {
            references.leftShoulder.localRotation = vrik.references.leftShoulder.localRotation;
            references.leftShoulder.localPosition = vrik.references.leftShoulder.localPosition;
        }
        if (vrik.references.leftUpperArm && references.leftUpperArm) {
            references.leftUpperArm.localRotation = vrik.references.leftUpperArm.localRotation;
            references.leftUpperArm.localPosition = vrik.references.leftUpperArm.localPosition;
        }
        if (vrik.references.leftForearm && references.leftForearm) {
            references.leftForearm.localRotation = vrik.references.leftForearm.localRotation;
            references.leftForearm.localPosition = vrik.references.leftForearm.localPosition;
        }
        if (vrik.references.leftHand && references.leftHand)
        {
            references.leftHand.localRotation = vrik.references.leftHand.localRotation;
            references.leftHand.localPosition = vrik.references.leftHand.localPosition;
        }
        if (vrik.references.rightShoulder && references.rightShoulder)
        {
            references.rightShoulder.localRotation = vrik.references.rightShoulder.localRotation;
            references.rightShoulder.localPosition = vrik.references.rightShoulder.localPosition;
        }
        if (vrik.references.rightUpperArm && references.rightUpperArm)
        {
            references.rightUpperArm.localRotation = vrik.references.rightUpperArm.localRotation;
            references.rightUpperArm.localPosition = vrik.references.rightUpperArm.localPosition;
        }
        if (vrik.references.rightForearm && references.rightForearm)
        {
            references.rightForearm.localRotation = vrik.references.rightForearm.localRotation;
            references.rightForearm.localPosition = vrik.references.rightForearm.localPosition;
        }
        if (vrik.references.rightHand && references.rightHand)
        {
            references.rightHand.localRotation = vrik.references.rightHand.localRotation;
            references.rightHand.localPosition = vrik.references.rightHand.localPosition;
        }
        if (vrik.references.leftThigh && references.leftThigh)
        {
            references.leftThigh.localRotation = vrik.references.leftThigh.localRotation;
            references.leftThigh.localPosition = vrik.references.leftThigh.localPosition;
        }
        if (vrik.references.leftCalf && references.leftCalf)
        {
            references.leftCalf.localRotation = vrik.references.leftCalf.localRotation;
            references.leftCalf.localPosition = vrik.references.leftCalf.localPosition;
        }
        if (vrik.references.leftFoot && references.leftFoot)
        {
            references.leftFoot.localRotation = vrik.references.leftFoot.localRotation;
            references.leftFoot.localPosition = vrik.references.leftFoot.localPosition;
        }
        if (vrik.references.leftToes && references.leftToes)
        {
            references.leftToes.localRotation = vrik.references.leftToes.localRotation;
            references.leftToes.localPosition = vrik.references.leftToes.localPosition;
        }
        if (vrik.references.rightThigh && references.rightThigh)
        {
            references.rightThigh.localRotation = vrik.references.rightThigh.localRotation;
            references.rightThigh.localPosition = vrik.references.rightThigh.localPosition;
        }
        if (vrik.references.rightCalf && references.rightCalf)
        {
            references.rightCalf.localRotation = vrik.references.rightCalf.localRotation;
            references.rightCalf.localPosition = vrik.references.rightCalf.localPosition;
        }
        if (vrik.references.rightFoot && references.rightFoot)
        {
            references.rightFoot.localRotation = vrik.references.rightFoot.localRotation;
            references.rightFoot.localPosition = vrik.references.rightFoot.localPosition;
        }
        if (vrik.references.rightToes && references.rightToes)
        {
            references.rightToes.localRotation = vrik.references.rightToes.localRotation;
            references.rightToes.localPosition = vrik.references.rightToes.localPosition;
        }
        
    }
}
