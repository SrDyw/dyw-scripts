using UnityEngine;

namespace DywFunctions
{
    namespace MathExtras
    {
        public static class MathE
        {
            //
            // Resumen:
            //     Returns the corresponding numeric value of a boolean.
            public static float BFloat(bool value) 
            {
                return value == true ? 1 : 0;
            }
            //
            // Resumen:
            //     Returns a direction vector of the angle.
            //
            // Params:
            //      angle:
            //          The angle for convert
            public static Vector2 GetVectorFromAngle(float angle) 
            {
                Vector2 vector = Vector2.zero;
                float radians = angle * Mathf.Deg2Rad;

                vector.x = Mathf.Cos(radians);
                vector.y = Mathf.Sin(radians);

                return vector;
            }
            //
            // Resumen:
            //     Return the angle from two vectors.
            //
            // Params:
            //      vectorSelf:
            //          The first vector
            //      vectorDest:
            //          The second vector
            public static float GetAngleFromVector(Vector2 vectorSelf, Vector2 vectorDest) 
            {
                float angle;

                float x = vectorDest.x - vectorSelf.x;
                float y = vectorDest.y - vectorSelf.y;

                angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;

                if (angle < 0) 
                {
                    angle *= -1;
                    angle = Mathf.Abs(angle - 360);
                }

                return angle;
            }
            //
            // Resumen:
            //     Returns the angle from one vector.
            //
            // Params:
            //      vector:
            //          The vector to convert.
            public static float GetAngleFromVector(Vector2 vector) 
            {
                float angle;
                float x = vector.x;
                float y = vector.y;

                angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;

                if (angle < 0) 
                {
                    angle *= -1;
                    angle = Mathf.Abs(angle - 360);
                }

                return angle;
            }

            public static Vector2 RotateVectorByAngle(this Vector2 v1, float angle) {
                float angleToRadians = angle * Mathf.Deg2Rad;
                float newVX = v1.x * Mathf.Cos(angleToRadians) - v1.y * Mathf.Sin(angleToRadians);
                float newVY = v1.x * Mathf.Sin(angleToRadians) + v1.y * Mathf.Cos(angleToRadians);

                return new(newVX, newVY);
            }     

            //
            // Resumen:
            //     Return a direction vector beetween two vectors.
            //
            // Params:
            //      vectorSelf:
            //          The first vector
            //      vectorDest:
            //          The second vector
            public static Vector2 VectorDirection(Vector2 vectorSelf, Vector2 vectorDes) 
            {
                Vector2 direction;

                float angle = GetAngleFromVector(vectorSelf, vectorDes);
                direction = GetVectorFromAngle(angle);

                return direction;
            }

            public static Vector2 VectorDiff(Vector2 vectorSelf, Vector2 vectorDes) 
            {
                return vectorDes - vectorSelf;
            }
            
            //
            // Resumen:
            //     Return a direction vector.
            //
            // Params:
            //      vectorSelf:
            //          The vector for calculate his direction
            public static Vector2 VectorDirection(Vector2 vectorSelf) 
            {
                Vector2 direction;

                float angle = GetAngleFromVector(Vector2.zero, vectorSelf);
                direction = GetVectorFromAngle(angle);

                return direction;
            }

            public static Vector3 To3D(this Vector2 self) {
                return new Vector3(self.x, 0, self.y);
            }

            public static Vector2 To2D(this Vector3 self) {
                return new Vector2(self.x, self.z);
            }

            //
            // Resumen:
            //     Linar interpolate between angle and angleDestiny.
            //
            // Params:
            //      angle:
            //          The start point.
            //      angleDestinty:
            //          The final point.
            //      transitionTime:
            //          Time of the interpolation.
            public static float LerpAngle(float angle, float angleDestiny, float transitionTime) 
            {
                float angleFinal = 0;

                Vector2 vecAngleSelf = GetVectorFromAngle(angle);
                Vector2 vecAngleDest = GetVectorFromAngle(angleDestiny);


                float xLerped = Mathf.Lerp(vecAngleSelf.x, vecAngleDest.x, transitionTime);
                float yLerped = Mathf.Lerp(vecAngleSelf.y, vecAngleDest.y, transitionTime);

                Vector2 vecAngleLerp = new Vector2(xLerped, yLerped);
                
                angleFinal = GetAngleFromVector(vecAngleLerp);

                return angleFinal;
            }

            public static float LerpSmooth(float value, float valueTarget, float lerpTime) {
                float valueLerped;

                if (value < valueTarget) {
                    valueLerped = Mathf.Min(value + lerpTime, valueTarget);
                }
                else {
                    valueLerped = Mathf.Max(value - lerpTime, valueTarget);
                }

                return valueLerped;

            }

            public static float SignExclusiveZero(float value) {
                return (value != 0)? Mathf.Sign(value) : -1;
            }

            public static float Choice(float[] choices)
            {
                int index = Random.Range(0, choices.Length);

                return choices[index];
            }

            public static T Choice<T>(T[] choices) {
                int index = Random.Range(0, choices.Length);

                return choices[index];
            }
        } 
    }     
}
