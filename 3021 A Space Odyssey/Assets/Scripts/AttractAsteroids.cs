using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttractAsteroids : MonoBehaviour {

    public float mass = 1;
    public float radius = 20;
    public GameObject[] asteroids;
    public List<GameObject> nearAsteroids;
    public ParticleSystem attractor;

    void Start() {
        attractor.Stop();
        try {
            UpdateAsteroids();
        } catch { }
    }

    void Update() {

        if (Input.GetButton("Attract") && GameStateManager.canStarShipMove()) {
            attractor.Play();
            try {
                UpdateAsteroids();

                // Apply gravity attraction to asteroids inside a radius saved in nearAsteroids
                for (int i = 0; i < nearAsteroids.Count; i++) {
                    Vector3 difference = this.transform.position - nearAsteroids[i].transform.position;
                    float dist = difference.magnitude;
                    Vector3 gravityDirection = difference.normalized;
                    float gravity = 6.7f * (mass * 80) / (dist * dist);
                    Vector3 gravityVector = (gravityDirection * gravity);
                    // nearAsteroids[i].GetComponent<Rigidbody>().AddForce(nearAsteroids[i].transform.forward, ForceMode.Acceleration);
                    nearAsteroids[i].GetComponent<Rigidbody>().AddForce(gravityVector, ForceMode.Acceleration);
                }
            } catch { }
        } else {
            attractor.Stop();
        }
    }

    void UpdateAsteroids() {
        nearAsteroids.Clear();
        asteroids = FindGameObjectsWithTags("Asteroids", "Fuel");
        for (int i = 0; i < asteroids.Length; i++) {
            if ((asteroids[i].transform.position - this.transform.position).magnitude < radius) {
                nearAsteroids.Add(asteroids[i]);
            }
        }
    }

    GameObject[] FindGameObjectsWithTags(params string[] tags) {
        var all = new List<GameObject>();
        foreach (string tag in tags) {
            all.AddRange(GameObject.FindGameObjectsWithTag(tag).ToList());
        }
        return all.ToArray();
    }
}
