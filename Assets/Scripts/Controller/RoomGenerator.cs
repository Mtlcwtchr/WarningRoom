using UnityEngine;


namespace Controller
{
   public class RoomGenerator : MonoBehaviour
   {
   
       [SerializeField] private GameObject roomPlatform;
       [SerializeField] private GameObject wallSample;
       [SerializeField] private float noiseScale;
       [SerializeField] private float wallGenFactor;
       [SerializeField] private float wallScaleFactor;
   
       private void OnEnable()
       {
           FillRoomByWalls();
       }
   
   
       private void FillRoomByWalls()
       {
           Transform roomPlatformTransform = roomPlatform.transform;
           Vector3 roomPosition = roomPlatformTransform.position;
           Vector3 roomScale = roomPlatformTransform.localScale;
           
           for (int i = 0; i < roomScale.x; ++i)
           {
               for (int j = 0; j < roomScale.z; ++j)
               {
                   float x = (float)i / roomScale.x * noiseScale;
                   float z = (float)j / roomScale.z * noiseScale;
   
                   float noise = Mathf.PerlinNoise(x, z);
                   if (noise > wallGenFactor)
                   {
                       GameObject wallUnit = Instantiate(wallSample, roomPlatform.transform);
                       
                       Vector3 wallUnitScale = wallUnit.transform.localScale;
                       wallUnitScale.y *= wallScaleFactor * (noise * noise);
                       wallUnitScale.x /= roomScale.x;
                       wallUnitScale.z /= roomScale.z;
                       wallUnit.transform.localScale = wallUnitScale;
                       
                       Vector3 wallUnitPosition = wallUnit.transform.position;
                       wallUnitPosition.x = i + roomPosition.x - roomScale.x / 2;
                       wallUnitPosition.z = j + roomPosition.z - roomScale.z / 2;
                       wallUnitPosition.y = roomPosition.y + wallUnitScale.y / 2;
                       wallUnit.transform.position = wallUnitPosition;
                   }
               }
           }
       }
   } 
}
