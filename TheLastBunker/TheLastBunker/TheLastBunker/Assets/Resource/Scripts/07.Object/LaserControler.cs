using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MovingLaser
{
    public Vector2 StartVec;
    public Vector2 EndVec;
}
public class LaserControler : MonoBehaviour
{
    public Laser StartLaser;
    public Laser EndLaser;
    public RaycastHit2D hit;
    public Vector2 LaserTarget;
    public MovingLaser StartMoving;
    public MovingLaser EndMoving;
    public ParticleSystem particlesystem;
    public bool MovingSupport;
    public int ControlerNumber;

    public void Start()
    {
        particlesystem = GetComponent<ParticleSystem>();
        MovingSupport = true;
        //StartCoroutine(LaserMoving());
        StartCoroutine(FireLaser());
    }

    public void Update()
    {
        this.transform.position = new Vector2(StartLaser.transform.position.x, StartLaser.transform.position.y + 6);
    }

    IEnumerator FireLaser()
    {
        while (true)
        {
            Ray2D ray = new Ray2D(StartLaser.transform.position, transform.up);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, Vector2.up, 60.0f);
            if (hit.collider != null)
            {
                if(hit.collider.tag == "Enermy")
                {
                    StartCoroutine(hit.collider.transform.parent.GetComponent<Enermy>().KnockbackDamage(4.0f, 0.0f, true));
                }
                else if(hit.collider.tag == "Player")
                {
                    if(PlayerControl.Instance.Invincibility == false && PlayerControl.Instance.Die == false)
                    {
                        StartCoroutine(hit.collider.GetComponent<Player>().DamagePlayer());
                    }
                }
            }
            yield return null;
        }
    }

    IEnumerator LaserMoving()
    {
        while (true)
        {
            yield return new WaitForSeconds(3.0f);
            StartLaser.TargetVector = StartMoving.StartVec;
            EndLaser.TargetVector = StartMoving.EndVec;
            yield return new WaitForSeconds(3.0f);
            StartLaser.TargetVector = EndMoving.StartVec;
            EndLaser.TargetVector = EndMoving.EndVec;
        }
    }
}
