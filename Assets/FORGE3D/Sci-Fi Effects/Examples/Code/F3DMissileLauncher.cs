/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class F3DMissileLauncher : MonoBehaviour {

    public static F3DMissileLauncher instance;

    public Transform missilePrefab;
    public Transform target;

    public Transform explosionPrefab;

    F3DMissile.MissileType missileType;

    public Text missileTypeLabel;

	// Use this for initialization
	void Start ()
    {
        instance = this;
        missileType = F3DMissile.MissileType.Unguided;
        missileTypeLabel.text = "Missile type: Unguided";
	}
	
    public void SpawnExplosion(Vector3 position)
    {
        F3DPool.instance.Spawn(explosionPrefab, position, Quaternion.identity, null);
    }

    void ProcessInput()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Transform tMissile = F3DPool.instance.Spawn(missilePrefab, transform.position + Vector3.up * 2, Quaternion.identity, null);

            if (tMissile != null)
            {
                F3DMissile missile = tMissile.GetComponent<F3DMissile>();

                missile.missileType = missileType;

                if(target != null)
                    missile.target = target;                
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            missileType = F3DMissile.MissileType.Unguided;
            missileTypeLabel.text = "Missile type: Unguided";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            missileType = F3DMissile.MissileType.Guided;
            missileTypeLabel.text = "Missile type: Guided";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            missileType = F3DMissile.MissileType.Predictive;
            missileTypeLabel.text = "Missile type: Predictive";
        }
    }

	// Update is called once per frame
	void Update () 
    {
        ProcessInput();
	}
}
