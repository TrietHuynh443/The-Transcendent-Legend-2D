using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.PlayerProperties{
    [Serializable]
    public class PlayerProperties
    {
        public PlayerInput Input = new();
        [SerializeField] public PlayerData Data;
        public PlayerStatus Status = new();  
    }

    public class PlayerInput{
        public float HorizontalInput = 0;
        public bool IsJumpInput = false;
        public bool IsAttackInput = false;
    }

    [Serializable]
    public class PlayerData{
        public float Speed;
        public float JumpForce;
        public float Attack;
        public float SpecialAttack;
        public int MaxJump;
    }

    public class PlayerStatus
    {
        public bool IsGrounded;
    }
}
