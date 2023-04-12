using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHandler : MonoBehaviour
{
    public ProjectileSO ProjectileSO;

    public float ManaCost;

    public float Angle;

    public float NumberProjectiles;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack(Vector3 origin, Vector3 direction, bool isPlayer)
	{
        direction = direction.normalized;

        if (Angle == 0f)
		{
            Vector3 velocity = ProjectileSO.Speed * direction;

            if (NumberProjectiles == 1)
			{
                MobManager.Instance.SpawnProjectile(ProjectileSO, origin, velocity, isPlayer);
            }
            else
			{
                Vector3 normal = 0.5f * Vector3.Cross(direction, Vector3.forward).normalized;

                MobManager.Instance.SpawnProjectile(ProjectileSO, origin + normal, velocity, isPlayer);
                MobManager.Instance.SpawnProjectile(ProjectileSO, origin - normal, velocity, isPlayer);

                if (NumberProjectiles == 3)
				{
                    MobManager.Instance.SpawnProjectile(ProjectileSO, origin, velocity, isPlayer);
                }
            }
            
		}
		else
		{
            float angleInc = Angle / NumberProjectiles;

            Vector3 velocity = ProjectileSO.Speed * (Quaternion.AngleAxis(-Angle / 2, Vector3.forward) * direction);

            for (int i = 0; i < NumberProjectiles; i++)
			{
                MobManager.Instance.SpawnProjectile(ProjectileSO, origin, velocity, isPlayer);

                velocity = Quaternion.AngleAxis(angleInc, Vector3.forward) * velocity;
            }
		}
	}

}
