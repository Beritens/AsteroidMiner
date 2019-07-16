using UnityEngine;
 
 public static class Vector2Extension {
     
     public static Vector2 Rotate(this Vector2 v, float degrees) {
         float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
         float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);
         
         float tx = v.x;
         float ty = v.y;
         v.x = (cos * tx) - (sin * ty);
         v.y = (sin * tx) + (cos * ty);
         return v;
     }
    public static Vector2 RadianToVector2(float radian)
     {
         return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
     }
      
     public static Vector2 DegreeToVector2(this float degree)
     {
         return RadianToVector2(degree * Mathf.Deg2Rad);
     }
 }
