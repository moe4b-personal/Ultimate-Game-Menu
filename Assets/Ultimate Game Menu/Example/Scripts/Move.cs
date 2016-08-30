using UnityEngine;
using System.Collections;

namespace UGM
{
    public class Move : MonoBehaviour
    {
        [SerializeField]
        bool raw = false;

        [SerializeField]
        float speed;

        [SerializeField]
        float growRate;

        [SerializeField]
        Vector3 direction;

        [SerializeField]
        Axis walkAxis;
        [SerializeField]
        Axis strafeAxis;

        void Start()
        {
            walkAxis.SetUp();
            strafeAxis.SetUp();
        }

        void Update()
        {
            if(raw)
            {
                direction.z = walkAxis.GetRawValue();
                direction.x = strafeAxis.GetRawValue();
            }
            else
            {
                direction.z = walkAxis.GetValue();
                direction.x = strafeAxis.GetValue();
            }

            transform.position += direction * speed * Time.deltaTime;

            if (InputManager.GetBind("On Foot.Jump"))
                transform.localScale = Vector3.MoveTowards(transform.localScale, new Vector3(2, 2, 2), growRate * Time.deltaTime);
            else
                transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.one, growRate * Time.deltaTime);
        }
    }
}
