using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewingAngle : MonoBehaviour
{
    public bool m_bDebugMode = false;

    [Header("View Config")]
    [Range(0.0f, 360.0f)]
    public float m_horizontalViewAngle = 0.0f;
    public float m_viewRadius = 1.0f;
    [Range(-180.0f, 180.0f)]
    public float m_viewRotateZ = 0f;

    public LayerMask m_viewTargetMask;
    public LayerMask m_viewObstacleMask;

    public List<Collider2D> hitedTargetContainer = new List<Collider2D>();
    public List<Collider2D> ImShi = new List<Collider2D>();
    public List<GameObject> Imshiobj = new List<GameObject>();

    public float m_horizontalViewHalfAngle = 0.0f;

    public bool CanTrace;
    public Enermy mycharacter;

    public void Awake()
    {
        m_horizontalViewHalfAngle = m_horizontalViewAngle * 0.5f;
    }

    public Vector3 AngleToDirZ(float angleInDegree)
    {
        float radian = (angleInDegree - transform.eulerAngles.z) * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), Mathf.Cos(radian), 0f);
    }

    public void Update()
    {
        FindViewTargets();
    }

    private void OnDrawGizmos()
    {
        if (m_bDebugMode)
        {
            m_horizontalViewHalfAngle = m_horizontalViewAngle * 0.5f;
            Vector3 originPos = new Vector2(transform.position.x, transform.position.y + 5);
            Gizmos.DrawWireSphere(originPos, m_viewRadius);

            Vector3 horizontalRightDir = AngleToDirZ(-m_horizontalViewHalfAngle + m_viewRotateZ);
            Vector3 horizontalLeftDir = AngleToDirZ(m_horizontalViewHalfAngle + m_viewRotateZ);

            Vector3 lookDir = AngleToDirZ(m_viewRotateZ);

            Debug.DrawRay(originPos, horizontalLeftDir * m_viewRadius, Color.cyan);
            Debug.DrawRay(originPos, lookDir * m_viewRadius, Color.green);
            Debug.DrawRay(originPos, horizontalRightDir * m_viewRadius, Color.cyan);
        }
    }

    public void resetm_horizontalViewAngle()
    {

    }

    public Collider2D[] FindViewTargets()
    {
        hitedTargetContainer.Clear();
        ImShi.Clear();
        Imshiobj.Clear();

        Vector2 originPos = new Vector2(transform.position.x, transform.position.y + 5);
        Collider2D[] hitedTargets = Physics2D.OverlapCircleAll(originPos, m_viewRadius, m_viewTargetMask);
        if (hitedTargets.Length == 0)
        {
            mycharacter.TraceTime -= 0.1f;
            if (mycharacter.FindPlayer == true)
            {
                mycharacter.FindPlayer = false;
                Vector3 pos = new Vector3(0, 0, 0);
                SpeechControler.Instance.AddSpeechBubble(transform, "??", SpeechbubbleType.THINKING, 3.0f, Color.white, pos);
            }
        }

        foreach (Collider2D hitedTarget in hitedTargets)
        {
            Vector2 targetPos = new Vector2(hitedTarget.transform.position.x, hitedTarget.transform.position.y + 5);
            Vector2 dir = (targetPos - originPos).normalized;
            Vector2 lookDir = AngleToDirZ(m_viewRotateZ);

            float dot = Vector2.Dot(lookDir, dir);
            float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

            if (angle <= m_horizontalViewHalfAngle)
            {
                RaycastHit2D rayHitedTarget = Physics2D.Raycast(originPos, dir, m_viewRadius, m_viewObstacleMask);
                
                if (rayHitedTarget.collider != hitedTarget  /*&& rayHitedTarget.collider != null*/ && rayHitedTarget)
                {
                    mycharacter.TraceTime -= 0.1f;
                    if (m_bDebugMode)
                    {
                        ImShi.Add(rayHitedTarget.collider);
                        Debug.DrawLine(originPos, rayHitedTarget.point, Color.yellow);
                    }
                    break;
                }
                else
                {
                    mycharacter.FindPlayer = true;
                    mycharacter.TraceTime = 10.0f;
                    hitedTargetContainer.Add(hitedTarget);
                    if (m_bDebugMode)
                    {
                        Debug.DrawLine(originPos, targetPos, Color.red);
                    }
                }
            }
            else
            {
                mycharacter.TraceTime -= 0.1f;
                if(mycharacter.FindPlayer == true)
                {
                    mycharacter.FindPlayer = false;
                }
            }
            bool check = false;
            for (int i = 0; i < hitedTargetContainer.Count; i++)
            {
                if (hitedTargetContainer[i].tag == "Player")
                {
                    CanTrace = true;
                    check = true;
                    break;
                }
            }
            if (check == false)
            {
                CanTrace = false;
            }
        }

        if (hitedTargetContainer.Count > 0)
            return hitedTargetContainer.ToArray();
        else
            return null;
    }
}
