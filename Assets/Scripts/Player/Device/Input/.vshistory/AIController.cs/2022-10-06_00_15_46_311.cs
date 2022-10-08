using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisAIController : MonoBehaviour
{
    public Axis axis = new Axis();
    public GameObject targetObject;

    private float aiLastTargetUpdate = 0;
    private float aiTargetUpdateTimeout = 0.15f;
    private Vector3 aiControls = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        aiControls *= 0.975f;

        if (Time.time - aiLastTargetUpdate < aiTargetUpdateTimeout)
        {
            return;
        }

        aiLastTargetUpdate = Time.time;

        int isLeftSide = transform.position.x < 0 ? -1 : 1;
        float extraTargetX = 0f;
        float extraTargetZ = 0f;

        if (
            transform.position.x < -3.25f
            || transform.position.x > 3.25f
        )
        {
            extraTargetX = -4f;
        }

        if (
            transform.position.x > -1.25f && isLeftSide == -1
            || transform.position.x < 1.25f && isLeftSide == 1
        )
        {
            extraTargetX = 3f;
        }

        if (transform.position.z < -3.5f)
        {
            extraTargetZ += 4f;
        }
        else if (transform.position.z > 3.5f)
        {
            extraTargetZ -= 4f;
        }

        float randomDeltaX = UnityEngine.Random.RandomRange(0.2f, 1f) + extraTargetX;
        float randomDeltaZ = UnityEngine.Random.RandomRange(0, transform.position.z) + extraTargetZ;

        Vector3 aiTargetPosition =
            targetObject.position
            + targetObject.velocity
            + new Vector3(isLeftSide * randomDeltaX, 0, randomDeltaZ);

        var distance = Mathf.Clamp01(Vector3.Distance(transform.position, ball.position) / 5);

        var axisX = aiTargetPosition.x < transform.position.x ? -distance : distance;
        var axisY = aiTargetPosition.z < transform.position.z ? -distance : distance;
        aiControls += new Vector3(axisX, axisY, 0);

        axis.setX(Mathf.Clamp01(aiControls.x));
        axis.setY(Mathf.Clamp01(aiControls.y));
    }
}
