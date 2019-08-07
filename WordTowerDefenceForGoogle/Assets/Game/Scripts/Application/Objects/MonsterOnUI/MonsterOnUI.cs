using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;
public class MonsterOnUI : MonoBehaviour {

    protected UnityArmatureComponent armature;

    protected virtual void Awake()
    {
        this.transform.position = new Vector3(-468, -185);
        armature = GetComponent<UnityArmatureComponent>();
        armature.animation.Play("walk");
    }
}
