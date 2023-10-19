using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private Animator animator;
    public Camera cam;
    private PlayerController playerController;
    public float cooldownTime = 2f;
    private float nextFireTime = 0f;
    public static int noOfClicks = 0;
    float lastClickedTime = 0;
    public float maxComboDelay = 1;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if( animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("PunchRight"))
        {
            animator.SetBool("punchR", false);
        }
        if( animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("PunchLeft"))
        {
            animator.SetBool("punchL", false);
            noOfClicks = 0;
        }

        if (Time.time - lastClickedTime > maxComboDelay)
        {
            noOfClicks = 0;
        }
        if(Time.time > nextFireTime)
        {
            if(Input.GetMouseButton(0))
            {
                OnClick();
            }
        }
        if(Input.GetMouseButton(1))
        {
            ZoomView();
        }
        if(Input.GetMouseButtonUp(1))
        {
            OnRelease();
        }
    }

    void OnClick()
    {
        lastClickedTime = Time.time;
        noOfClicks++;
        var attack1 = animator.GetBool("punchR");

        if (noOfClicks == 1)
        {
            animator.SetBool("punchR", true);
        }
        noOfClicks = Mathf.Clamp(noOfClicks, 0, 2);
        
        if (noOfClicks >= 2 && !attack1)
        {
            animator.SetBool("punchL", true);
        }
    }

    void OnRelease()
    {
        PlayerController.Instance.SetAiming(false);
    }

    void ZoomView()
    {
        PlayerController.Instance.SetAiming(true);
        animator.SetBool("isMoving", false);
        var mousePos = Input.mousePosition;
        var playerPos = cam.WorldToScreenPoint(transform.position);
        var direction = mousePos - playerPos;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.down);
    }
}
