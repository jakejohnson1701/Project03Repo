using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyBehavior : MonoBehaviour
{
    Transform tr_CommandPost;
    float f_RotSpeed = 3.0f, f_MoveSpeed = 3.0f;

    void Start()
    {
        tr_CommandPost = GameObject.FindGameObjectWithTag("Post").transform;
    }

    // Update is called once per frame
    void Update()
    {
        //look at command post
        transform.rotation = Quaternion.Slerp(transform.rotation
                                              , Quaternion.LookRotation(tr_CommandPost.position - transform.position)
                                              , f_RotSpeed * Time.deltaTime);

        //move toward command post
        transform.position += transform.forward * f_MoveSpeed * Time.deltaTime;
    }

    //function that is called to destroy enemy object when it is hit with player raycast
    public void Kill()
    {
        Destroy(gameObject);
    }
}
