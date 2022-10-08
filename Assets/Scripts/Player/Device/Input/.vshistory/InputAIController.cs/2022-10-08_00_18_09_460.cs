using UnityEngine;

public class InputAIController : MonoBehaviour
{
    public Rigidbody target;
    public bool isLeftSide;

    private readonly Axis axis = new();
    private float aiLastTargetUpdate = 0f;
    private readonly float aiTargetUpdateTimeout = 0.15f;
    private Vector3 aiControls = new();

    public Axis GetUpdatedAxis()
    {
        UpdateAxis();
        return axis;
    }

    private void UpdateAxis()
    {
        aiControls *= 0.975f;

        if (aiLastTargetUpdate + aiTargetUpdateTimeout > Time.time)
        {
            return;
        }

        aiLastTargetUpdate = Time.time;

        float extraTargetX = 0f;
        float extraTargetZ = 0f;

        if (transform.position.x < -3.25f || transform.position.x > 3.25f)
        {
            extraTargetX = -4f;
        }

        if (
            transform.position.x > -1.25f && isLeftSide
            || transform.position.x < 1.25f && isLeftSide
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

        float randomDeltaX = Random.Range(0.2f, 1f) + extraTargetX;
        float randomDeltaZ = Random.Range(0, transform.position.z) + extraTargetZ;

        Vector3 aiTargetPosition =
            target.position
            + target.velocity
            + new Vector3((isLeftSide ? -1f : 1f) * randomDeltaX, 0, randomDeltaZ);

        float distance = Mathf.Clamp(Vector3.Distance(transform.position, target.position) / 5f, -1f, 1f);
        float axisX = aiTargetPosition.x < transform.position.x ? -distance : distance;
        float axisY = aiTargetPosition.z < transform.position.z ? -distance : distance;
        aiControls += new Vector3(axisX, axisY, 0);

        axis.SetX(aiControls.x);
        axis.SetY(aiControls.y);

        Debug.Log(axisX + " : " + Mathf.Clamp((float)aiControls.x, -1f, 1f));
    }
}
