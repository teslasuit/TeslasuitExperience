using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class AnimEvaluate : MonoBehaviour
{
    public AnimationClip anim;

    public Transform src;
    public Transform dest;
    public Transform pose;

    [Range(0f, 1f)]
    public float time = 0.0f;

    private Dictionary<int, Matrix4x4> mInitialPose = new Dictionary<int, Matrix4x4>();
    private Dictionary<int, Matrix4x4> mWorldToLocal = new Dictionary<int, Matrix4x4>();
    private Dictionary<int, Matrix4x4> mLocalToWorld = new Dictionary<int, Matrix4x4>();
    private Dictionary<int, Matrix4x4> mCumMatrix = new Dictionary<int, Matrix4x4>();
    private Dictionary<int, Quaternion> qWorld = new Dictionary<int, Quaternion>();

    private Node rootNode;


    class Node
    {
        public int instance;
        public Matrix4x4 mMatrix;
        public Node parent;
        List<Node> children = new List<Node>();

        public void SetParent(Node n)
        {
            parent = n;
            if (n != null)
            {
                parent.children.Add(this);
            }
        }

        public Matrix4x4 CumMatrix()
        {
            var curr = mMatrix;
            var currPar = parent;
            while(currPar != null)
            {
                curr *= currPar.mMatrix;
                currPar = currPar.parent;
            }
            return curr;
        }

        public Matrix4x4 ParentMatrix()
        {
            var curr = Matrix4x4.identity;
            var currPar = parent;
            while(currPar != null)
            {
                curr *= currPar.mMatrix;
                currPar = currPar.parent;
            }
            return curr;
        }
    }

    private Dictionary<int, Vector3> mOffs = new Dictionary<int, Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Initialize()
    {
        Node prev = null;
        ForeachRecursive(pose, (item)=>
        {
            var matrix = Matrix4x4.TRS(item.localPosition, item.localRotation, item.localScale);
            mInitialPose[item.GetInstanceID()] = Matrix4x4.TRS(item.position, item.rotation, item.lossyScale);
            mWorldToLocal[item.GetInstanceID()] = item.worldToLocalMatrix;
            mLocalToWorld[item.GetInstanceID()] = item.localToWorldMatrix;

            Node current = new Node(){instance=item.GetInstanceID(), mMatrix = matrix};

            current.SetParent(prev);

            prev = current;

            mCumMatrix[item.GetInstanceID()] = current.ParentMatrix();
        });
    }

    private void ForeachRecursive(Transform begin, Action<Transform> action)
    {
        action(begin);
        foreach(Transform child in begin)
        {
            ForeachRecursive(child, action);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if(!anim || !dest || !src)
        {
            return;
        }

        if(mInitialPose.Count == 0)
        {
            Initialize();
        }

        var bindings = AnimationUtility.GetCurveBindings(anim);

        float[] quatF = new float[4];  

        foreach(var binding in bindings)
        {
            var curve = AnimationUtility.GetEditorCurve(anim, binding);
            var srcTransform = AnimationUtility.GetAnimatedObject(src.gameObject, binding) as Transform;
            var targetTransform = FindChild(dest, srcTransform.name);
            var poseTransform = FindChild(pose, srcTransform.name);
            
            var key = curve.keys[(int)(time * curve.length)];
            var path = binding.path;
            if(binding.propertyName.Contains("Rotation"))
            {
                if(binding.propertyName.Contains(".x"))
                {
                    quatF[0] = key.value;
                }
                else
                if(binding.propertyName.Contains(".y"))
                {
                    quatF[1] = key.value;
                }
                else
                if(binding.propertyName.Contains(".z"))
                {
                    quatF[2] = key.value;
                }
                else
                if(binding.propertyName.Contains(".w"))
                {
                    quatF[3] = key.value;

                    Quaternion q = new Quaternion(quatF[0], quatF[1], quatF[2], quatF[3]);
                    
                    //var initPose = mInitialPose[poseTransform.GetInstanceID()];
                    //var initParentWorld = mInitialPose[poseTransform.parent.GetInstanceID()];
                    //var qParent = Quaternion.identity;
                    //
                    //qWorld[poseTransform.GetInstanceID()] = q;
                    //
                    //if(qWorld.ContainsKey(poseTransform.parent.GetInstanceID()))
                    //{
                    //    qParent = qWorld[poseTransform.parent.GetInstanceID()];
                    //}
                    //
                    //var qL = Quaternion.Inverse(qParent) * q;
                    //targetTransform.localRotation = initParentWorld.inverse.rotation * qL * initPose.rotation;

                    q = new Quaternion(q[one], q[two], q[three], q.w);
                    q = new Quaternion(q.x, q.y, q.z, q.w);

                    targetTransform.rotation = Quaternion.identity;
                }
            }
        }

    }

    public int one = 0;
    public int two = 1;
    public int three = 2;

    public float heading = 0.0f;

    private Transform FindChild(Transform parent, string name)
    {
        if (parent == null) return null;
        var result = parent.Find(name);
        if (result != null)
            return result;
        foreach (Transform child in parent)
        {
            result = FindChild(child, name);
            if (result != null)
                return result;
        }
        return null;
    }
}
