using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personal_Direction_Finder
{

    private Vector2 selfRef, target;
    private float precision;

    public Personal_Direction_Finder(Vector2 aSelfRef, Vector2 aTarget , float aPrecision)
    {
        selfRef = aSelfRef;
        target = aTarget;
        precision = aPrecision;

    }

    public Vector2 findDirToTarget()
    {
        /*calculate the nearest direction to a target,
         * 1. take current Player position
         * 2. add a cos sin  vector from the possible direction to the player position
         * 3. compare the distance from this new vector to the Boss , to the distance from the current "nearest" cos sin  vector to the Boss
         * 
         * precision between 0.05 - 1
         */

        Vector2 direction = Vector2.zero;


        if (precision > 1) {
            precision = 1;
        }

        if (precision < 0.05f) {
            precision = 0.05f;
        }

        Vector2 posToTest, currentNpos;

        //Optimized Version 
        //  divide pi cirlcle on 4 
        //  find where section we are
        //  only find the neares position in this section

        float[] startEnd = findPISection();


        for (float i = startEnd[0]; i < startEnd[1]; i = i + precision)
        {
            posToTest = new Vector2(selfRef.x + Mathf.Cos(i), selfRef.y + Mathf.Sin(i));
            currentNpos = new Vector2(selfRef.x + direction.x, selfRef.y + direction.y);


            if (Vector2.Distance(posToTest, target) < Vector2.Distance(currentNpos, target))
            {
                direction = new Vector2(Mathf.Cos(i), Mathf.Sin(i));
            }
        }


        return direction;


    }

    private float[] findPISection()
    {
        float[] startEnd = new float[2];


        if (target.x > selfRef.x && target.y > selfRef.y)
        {
            // Debug.Log("cross hair on X Positive, Y Positive");

            startEnd[0] = 0;
            startEnd[1] = (Mathf.PI / 2);

        }
        else if (target.x < selfRef.x && target.y > selfRef.y)
        {
            // Debug.Log("cross hair on X Negative, Y Positive");

            startEnd[0] = (Mathf.PI / 2);
            startEnd[1] = (Mathf.PI);
        }
        else if (target.x < selfRef.x && target.y < selfRef.y)
        {
            // Debug.Log("cross hair on X Negative, Y Negative");
            startEnd[0] = (Mathf.PI);
            startEnd[1] = ((3 * Mathf.PI) / 2);
        }
        else if (target.x > selfRef.x && target.y < selfRef.y)
        {
            //Debug.Log("cross hair on X Positive, Y Negative");

            startEnd[0] = ((3 * Mathf.PI) / 2);
            startEnd[1] = (Mathf.PI * 2);

        }
        else
        {
            Debug.Log("Not in PI cercle?");

            startEnd[0] = 0;
            startEnd[1] = (Mathf.PI * 2);

        }

        return startEnd;

    }
}
