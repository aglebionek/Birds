using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour {
    int DNALength = 5;
    public DNA dna;
    public GameObject eyes;
    bool seeDownWall = false;
    bool seeUpWall = false;
    bool seeBottom = false;
    bool seeTop = false;
    Vector3 startPosition;
    public float timeAlive = 0;
    public float distanceTravelled = 0;
    public int crash = 0;
    bool alive = true;
    Rigidbody2D rb;

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "dead") {
            alive = false;
        }
    }

    void OnCollisionStay2D(Collision2D col) {
        if (col.gameObject.tag == "top" ||
        col.gameObject.tag == "bottom" ||
        col.gameObject.tag == "upwall" ||
        col.gameObject.tag == "downwall") {
            crash++;
        } 
    }

    public void Init() {
        //0 top_n
        //1 bottom_n
        //2 top
        //3 bottom
        //4 force to apply if nothing is in sight
        dna = new DNA(DNALength, 100);
        this.transform.Translate(0, 0, 0);
        startPosition = this.transform.position;
        rb = this.GetComponent<Rigidbody2D>();
    }

    private void Update() {
        if (!alive)return;

        seeUpWall = false;
        seeDownWall = false;
        seeBottom = false;
        seeUpWall = false;
        RaycastHit2D hit = Physics2D.Raycast(eyes.transform.position, eyes.transform.forward, 2.0f);

        Debug.DrawRay(eyes.transform.position, eyes.transform.forward *1.0f, Color.red);
        Debug.DrawRay(eyes.transform.position, eyes.transform.up *1.0f, Color.red);
        Debug.DrawRay(eyes.transform.position, -eyes.transform.forward *1.0f, Color.red);

        if (hit.collider != null) {
            if (hit.collider.gameObject.tag == "upwall") seeUpWall = true;
            else if (hit.collider.gameObject.tag == "downwall") seeDownWall = true;
        }

        hit = Physics2D.Raycast(eyes.transform.position, eyes.transform.up, 2.0f);
        if (hit.collider != null) {
            if (hit.collider.gameObject.tag == "up") seeTop = true;
        }

        hit = Physics2D.Raycast(eyes.transform.position, -eyes.transform.up, 2.0f);
        if (hit.collider != null) {
            if (hit.collider.gameObject.tag == "bottom") seeBottom = true;
        }
    }

    void FixedUpdate() {
        if (!alive)return;

        float downForce = 0;
        float forwardForce = 1.0f;

        if(seeUpWall) downForce = dna.GetGene(0);
        else if(seeDownWall) downForce = dna.GetGene(1);
        else if(seeTop) downForce = dna.GetGene(2);
        else if(seeBottom) downForce = dna.GetGene(3);
        else downForce = dna.GetGene(4);

        rb.AddForce(this.transform.right * forwardForce);
        rb.AddForce(this.transform.up * downForce * 0.1f);
        //distanceTravelled = Vector3.Distance(startPosition, this.transform.position);
        distanceTravelled = this.transform.position.x - startPosition.x;
    }
}