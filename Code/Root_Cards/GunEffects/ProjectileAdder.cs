using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProjectileAdder: MonoBehaviour {
    public ProjectilesToSpawn[] projectiles;

    public void Start(){
        Gun gun = GetComponentInParent<WeaponHandler>().gun;
        List<ProjectilesToSpawn> gunProjectiles = gun.projectiles.ToList();
        gunProjectiles.AddRange(projectiles);
        gun.projectiles = gunProjectiles.ToArray();
    }
    public void OnDestroy(){
        
        Gun gun = GetComponentInParent<WeaponHandler>().gun;
        List<ProjectilesToSpawn> gunProjectiles = gun.projectiles.ToList();
        foreach(var projectile in projectiles)
            gunProjectiles.Remove(projectile);
        gun.projectiles = gunProjectiles.ToArray();
    }

}
