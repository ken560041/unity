using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Monster : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject Target;
    public Rigidbody rb;
    public float MaxSpeed;
    private float Speed;
    private Collider[] hitCollider;
    private RaycastHit Hit;
    private bool seePlayer;
    public float DetectionRange;
    public float SightRanger;

    void Start()
    {
        Speed = MaxSpeed;
    }

    // Update is called once per frame
    void Update()
    {


        if (!seePlayer)
        {
            hitCollider = Physics.OverlapSphere(transform.position, DetectionRange);
            foreach(var HitCollider in hitCollider)
            {
                if (HitCollider.tag == "Player")
                {
                    Target=HitCollider.gameObject;
                    seePlayer = true;
                }
            }
        }
        else
        {
            if (Physics.Raycast(transform.position, (Target.transform.position - transform.position), out Hit, SightRanger))
            {
                if (Hit.collider.tag != "Player")
                {
                    seePlayer=false;
                }
                else
                {
                    var Heading=Target.transform.position-transform.position;
                    var Distance = Heading.magnitude;
                    var Direcation = Heading / Distance;

                    Vector3 Move = new Vector3(Direcation.x * Speed, 0, Direcation.z * Speed);
                    rb.velocity = Move;

                }
            }
        }
    }
}
