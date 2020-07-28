using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandPostBehavior : MonoBehaviour
{
    bool PlayerInArea = false;
    bool EnemyInArea = false;
    [SerializeField] GameObject Enemy;
    [SerializeField] Light CommandPostLight;
    [SerializeField] Image ProgressBarColor;
    int increment = 1;
    int ProgressCounter = 0;
    int TimeUntilCapture = 10;
    int TimeUntilNeutral = 10;
    [SerializeField] ProgressBar progressBar;
    int minProgress = 0;
    int currentProgress;
    [SerializeField] AudioClip PostChangeSound;
    [SerializeField] AudioClip PostProgressSound;
    [SerializeField] AudioClip PostInterruptSound;
    

    void Start()
    {
        CommandPostLight.color = Color.white;
        currentProgress = minProgress;
        progressBar.SetMinProgress(minProgress);
    }


    void FixedUpdate()
    {
        CheckForEnemy();

    }

    void MakeProgress(int progress)
    {
        currentProgress += progress;
        progressBar.SetProgress(currentProgress);
    }

    void LoseProgress(int progress)
    {
        currentProgress -= progress;
        progressBar.SetProgress(currentProgress);
    }

    void OnTriggerEnter(Collider other)
    {
        
        //first check if the object that entered was a the player or the enemy and set their respective boolean variables to true 
        //based on which script was found on the entering object
        BasicPlayerMovement basicPlayerMovement = other.GetComponent<BasicPlayerMovement>();
        if(basicPlayerMovement != null)
        {
            PlayerInArea = true;
        }

        BasicEnemyBehavior basicEnemyBehavior = other.GetComponent<BasicEnemyBehavior>();
        if (basicEnemyBehavior != null)
        {
            
            EnemyInArea = true;
        }

        //if the player is in the area and the enemy is not and the command post is not under allied control
        //start ally capture sequence
        if ((PlayerInArea == true && EnemyInArea == false) && CommandPostLight.color != Color.blue)
        {
            ProgressCounter = 0;
            StartCoroutine(AllyCaptureSequence());
            
        }

        //if the enemy is in the area and the player is not and the command post is not under enemy control
        if((EnemyInArea == true && PlayerInArea == false) && CommandPostLight.color != Color.red)
        {
            
            StartCoroutine(EnemyCaptureSequence());
        }

    }

   

    //this function is continuously called in the fixed update function to check if the enemy has been destroyed
    void CheckForEnemy()
    {
        if(Enemy == null)
        {
            EnemyInArea = false;
        }

    }

    IEnumerator AllyCaptureSequence()
    {
        while ((enabled && ProgressCounter < 10) && CommandPostLight.color != Color.white)
        {
            //wait for one second then increase progress counter by 1
            //decrease progress bar by 10 percent and play audio feedback as well
            yield return new WaitForSeconds(increment);
            ProgressCounter++;
            LoseProgress(10);
            AudioHelper.PlayClip2D(PostProgressSound, 1);
            Debug.Log(ProgressCounter);
            //if progress counter is equal to the time needed to make the command post neutral, change the light color to white
            //reset progress counter to zero, change the light color to white, and change the fill color to blue
            if (PlayerInArea == true && ProgressCounter == TimeUntilNeutral)
            {
                CommandPostLight.color = Color.white;
                AudioHelper.PlayClip2D(PostChangeSound, 1);
                ProgressCounter = 0;
                ProgressBarColor.color = Color.blue;
            }
        }

        while ((enabled && ProgressCounter < 10) && CommandPostLight.color != Color.blue)
        {
            //wait for one second then increase progress counter by 1
            //increase progress bar by 10 percent and play audio feedback as well
            yield return new WaitForSeconds(increment);
            ProgressCounter++;
            MakeProgress(10);
            AudioHelper.PlayClip2D(PostProgressSound, 1);
            Debug.Log(ProgressCounter);
            //if player is in area and the progress counter has reached the time needed to capture the command post, change the light color to blue;
            //reset progress counter to zero
            //TODO add Audio and progress bar feedback each that plays each time though the loop
            //TODO add audio and visual feedback that plays upon a successful capture
            if (PlayerInArea == true && ProgressCounter == TimeUntilCapture)
            {
                CommandPostLight.color = Color.blue;
                AudioHelper.PlayClip2D(PostChangeSound, 1);
                ProgressCounter = 0;


            }

        }
        
    }

    //coroutine for EnemyCaptureSequence
    IEnumerator EnemyCaptureSequence()
    {
        while (enabled && ProgressCounter < 10)
        {
            //wait for one second then increase progress counter by 1
            //increase progress bar by 10 percent and play audio feedback as well
            yield return new WaitForSeconds(increment);
            ProgressCounter++;
            MakeProgress(10);
            AudioHelper.PlayClip2D(PostProgressSound, 1);
            Debug.Log(ProgressCounter);
            //if enemy is in area and the progress counter has reached the time needed to capture the command post, change the light color from white to red;
            //TODO add Audio and progress bar feedback each that plays each time though the loop
            //TODO add audio and visual feedback that plays upon a successful capture
            if (EnemyInArea == true && ProgressCounter == TimeUntilCapture)
            {
                CommandPostLight.color = Color.red;
                AudioHelper.PlayClip2D(PostChangeSound, 1);
                


            }
           
        }

    }
}
