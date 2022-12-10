using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IDamagable
    {
        
        public float Health { set; get; }
        public void Death();
        public void OnHit(float damage);
        public void Hurt();
    }

