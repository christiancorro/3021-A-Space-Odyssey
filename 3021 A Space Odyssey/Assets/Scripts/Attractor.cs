using UnityEngine;

public class Attractor : MonoBehaviour {

    private GameObject starship;
    public float mass = 2;
    public float radius = 100;

    private Rigidbody starshipRigidbody;

    void Start() {
        starship = GameObject.FindGameObjectsWithTag("Starship")[0];
        starshipRigidbody = starship.GetComponent<Rigidbody>();
    }

    void Update() {

        if (GameStateManager.isInGame()) {

            // Apply gravity attraction to starship if inside radius
            Vector3 difference = this.transform.position - starship.transform.position;
            float dist = difference.magnitude;
            if (dist < radius) {
                Vector3 gravityDirection = difference.normalized;
                float gravity = 6.7f * (this.mass * 80) / (dist * dist);
                Vector3 gravityVector = (gravityDirection * gravity);
                starshipRigidbody.AddForce(starship.transform.forward, ForceMode.Acceleration);
                starshipRigidbody.AddForce(gravityVector, ForceMode.Acceleration);
            }

        }
    }
}
