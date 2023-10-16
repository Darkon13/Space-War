public static class Utils
{
    public static float GetSignedAngle(float angle)
    {
        angle = angle % 360;

        if (angle >= 180)
        {
            return -(360 - angle);
        }
        else
        {
            return angle;
        }
    }
}
