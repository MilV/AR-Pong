using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine;
using System;

public class BallScript : NetworkBehaviour {
    public GameObject ballPrefab;
    public Transform ballSpawn;
    private Vector3 lastFrameVelocity;
    private Rigidbody rb;
    private bool wurdeAngestoßen;

    private int stuckCounter;
    private int temp;

    // Use this for initialization
    void Start () {
    
        Debug.Log("creating ball");
       // GameObject ball = (GameObject)Instantiate(ballPrefab, ballSpawn.position, ballSpawn.rotation);
        stuckCounter = 0;
        wurdeAngestoßen = false;
        rb = GetComponent<Rigidbody>();
       // rb = GetComponent<Rigidbody>();
        StartCoroutine(Waiter());
    }

    /***********-- FixedUpdate --****************************************************************
     * erkennt ob der Ball sich zu langsam in X-Richtung bewegt und leitet ggf den Reset ein    *
     * erkennt ob --warum auch immer-- der Ball zu langsam wird und leitet ggf den Reset ein    *
     * *****************************************************************************************/
    void FixedUpdate () {
   
        lastFrameVelocity = rb.velocity;

        if ((rb.velocity.magnitude < 20f) && (this.wurdeAngestoßen == true))    
        {
            this.wurdeAngestoßen = false;
            StartCoroutine(Waiter());
            Debug.Log(message:"Ball zu langsam");
        }

        if (stuckCounter > 8){
            this.wurdeAngestoßen = false;
            rb.Sleep();
            stuckCounter = 0;
            StartCoroutine(Waiter());
            Debug.Log(message:"PingPongStuck");
        }
    }


    /********************--Collisionserkennung--*********************************************************************
     * erkennt mit welchem Spielobjekt kollidiert wurde und startet abhängig vom getroffenen Objekt die Funktionen  *
     * Bounce, SpeedupBounce oder Waiter                                                                            *
     * zählt den "StuckCounter" hoch                                                                                *
     ***************************************************************************************************************/
    private void OnCollisionEnter(Collision col){
    
        switch (col.gameObject.name){

             case "SouthBoundary":
                 Debug.Log(message:"SouthBoundaryHit");
                Bounce(col.contacts[0].normal);
                stuckCounter++;
                 break;

             case "NorthBoundary":
                 Debug.Log(message:"NorthBoundaryHit");

                Bounce(col.contacts[0].normal);
                stuckCounter++;
                break;

             case "ClientGoal":
                //give host a goal
                Scoreboard_Controller.instance.HostScored();
                this.wurdeAngestoßen = false;
                 Debug.Log(message:"ClientGoalHit");
                stuckCounter = 0;

                StartCoroutine(Waiter());
                break;

             case "HostGoal":
                //give client a point
                Scoreboard_Controller.instance.ClientScored();
                this.wurdeAngestoßen = false;
                 Debug.Log(message:"HostGoalHit");
                stuckCounter = 0;

                StartCoroutine(Waiter());
                break;

            case "HostBalken":
                //count the score for the Singleplayer mode
                Scoreboard_Controller.instance.SingleplayerScored();
                Debug.Log(message:"HostBalkenHit");
                stuckCounter = 0;
                SpeedupBounce(col.contacts[0].normal);
                break;

            case "ClientBalken":
                Debug.Log(message:"ClientBalkenHit");
                stuckCounter = 0;
                SpeedupBounce(col.contacts[0].normal);

                break;
        }

    }

    /**************************------Bounce------**************************************************************************************
     * Bekommt normale des Aufprallpunktes übergeben                                                                                  *
     * speichert letzte Geschwindigkeit und Richtung vor dem Aufprall zwischen                                                        *
     * Errechnet neue Geschwindigkeit(vektoriell) aus der übergebenen Normalen und der normalisierten Geschwindigkeit vor dem Aufprall*
     * Setzt neue Geschwindigkeit (vektoriell) nach dem aufprall                                                                      *
     **********************************************************************************************************************************/
    private void Bounce(Vector3 collisionNormal)
    {
        if (isLocalPlayer)
        {
            return;
        }
        var speed = lastFrameVelocity.magnitude;
        var direction = Vector3.Reflect(lastFrameVelocity.normalized, collisionNormal);

      //  Debug.Log(message:"Out Direction: " + direction);
        rb.velocity = direction * speed;
    }

    private void SpeedupBounce(Vector3 collisionNormal)
    {
    
        var speed = lastFrameVelocity.magnitude;
        var direction = Vector3.Reflect(lastFrameVelocity.normalized, collisionNormal);

       // Debug.Log(message:"Out Direction: " + direction);
        rb.velocity = direction * Mathf.Min((speed * 1.4f), 75f);
    }

    /*************************----------WAITER-----------********************************
     * Stoppt den Ball                                                                  *
     * Platziert den Ball in der Mitte des Spielfeldes                                  *
     * Errechnet zufällige Richtung und Stärke mit der der Ball neu angestoßen wird     *
     ************************************************************************************/
    private IEnumerator Waiter(){
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.Sleep();

        yield return new WaitForSeconds(3);
        rb.MovePosition(new Vector3(0, 1.352f, 0));
        yield return new WaitForSeconds(3);

        if (UnityEngine.Random.Range(0, 51) % 2 == 0){
            Vector3 random = new Vector3(UnityEngine.Random.Range(500f, 800f), 0, UnityEngine.Random.Range(500f, 800f)); 
            if (UnityEngine.Random.Range(0, 51) % 2 == 0)
            {
                rb.AddForce(random);
            }
            else
            {
                rb.AddForce(-random);
            }
        }
        else
        {
            Vector3 random = new Vector3(UnityEngine.Random.Range(500f, 800f), 0, UnityEngine.Random.Range(-500f, -800f));  
            if (UnityEngine.Random.Range(0, 51) % 2 == 0)
            {
                rb.AddForce(random);
            }
            else
            {
                rb.AddForce(-random);
            }
        }
        yield return new WaitForSeconds(seconds: 5);
        if (rb.velocity.magnitude > 20f) {
            this.wurdeAngestoßen = true;
        }
    }
}
