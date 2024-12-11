using System.Collections.Generic;
using GameEvent;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Achievement
{
    public enum EAchievementType
    {
        AchievementWalk = 0,
        AchievementJump,
        AchievementAttack,
        AchievementToxicBall,
        AchievementTrap,
        AchievementGraveyard,
    }

    public class AchievementData
    {
        public string Name { get; private set; }
        public string Description { get; private set; }

        public Sprite Resource { get; private set; }

        public AchievementData(string name, string description, Sprite resource)
        {
            Name = name;
            Description = description;
            Resource = resource;
        }
    }

    public class AchievementManager : UnitySingleton<AchievementManager>,
        IGameEventListener<MoveAchievementProgress>,
        IGameEventListener<JumpAchievementProgress>,
        IGameEventListener<AttackAchievementProgress>,
        IGameEventListener<ToxicBallAchievementProgress>,
        IGameEventListener<TrapEncounterAchievementProgress>,
        IGameEventListener<LevelCompletionAchievementProgress>

    {
        private Dictionary<EAchievementType, AchievementData> achievementDictionary;

        private Vector2 _movedDistance = new Vector2(0,0);
        private int _jumpCount = 0;
        private int _attackCount = 0;
        private float _accumulatedAttackDamage = 0;
        private int _toxicBallCount = 0;
        private float _accumulatedToxicBallDamage = 0;
        private int _getHurtByTrapCount = 0;
        private List<string> _completedScenes = new List<string>();


        void Start()
        {
            EventAggregator.Register<MoveAchievementProgress>(this);
            EventAggregator.Register<JumpAchievementProgress>(this);
            EventAggregator.Register<AttackAchievementProgress>(this);
            EventAggregator.Register<ToxicBallAchievementProgress>(this);
            EventAggregator.Register<TrapEncounterAchievementProgress>(this);
            EventAggregator.Register<LevelCompletionAchievementProgress>(this);
            InitializeAchievements();
        }

        private void InitializeAchievements()
        {
            achievementDictionary = new Dictionary<EAchievementType, AchievementData>
            {
                {
                    EAchievementType.AchievementWalk,
                    new AchievementData("A Thousand Miles", "Walk a thousand miles", Resources.Load<Sprite>("Achievement/Walk"))
                },
                {
                    EAchievementType.AchievementJump,
                    new AchievementData("High Hopes", "Jump 20 times", Resources.Load<Sprite>("Achievement/Jump"))
                },
                {
                    EAchievementType.AchievementAttack,
                    new AchievementData("My War", "Used melee 10 times", Resources.Load<Sprite>("Achievement/Attack"))
                },
                {
                    EAchievementType.AchievementToxicBall,
                    new AchievementData("Toxic", "Used toxic ball 10 times", Resources.Load<Sprite>("Achievement/Ball"))
                },
                {
                    EAchievementType.AchievementTrap,
                    new AchievementData("The Ordinary Road", "Trigger trap 7 times", Resources.Load<Sprite>("Achievement/Trap"))
                },
                {
                    EAchievementType.AchievementGraveyard,
                    new AchievementData("Gravedigger", "Complete your first level", Resources.Load<Sprite>("Achievement/Graveyard"))
                },
            };
        }

        public AchievementData GetAchievementData(EAchievementType achievement)
        {
            return achievementDictionary.TryGetValue(achievement, out AchievementData data) ? data : null;
        }

        public void Handle(MoveAchievementProgress @event)
        {
            _movedDistance += @event.Distance;
            if (_movedDistance.x >= 100000f)
            {
                EventAggregator.RaiseEvent<AchievementSastifaction>(new AchievementSastifaction()
                {
                    Type = EAchievementType.AchievementWalk,
                });
            }
        }

        public void Handle(JumpAchievementProgress @event)
        {
            _jumpCount += @event.Count;
            if (_jumpCount >= 20)
            {
                EventAggregator.RaiseEvent<AchievementSastifaction>(new AchievementSastifaction()
                {
                    Type = EAchievementType.AchievementJump,
                });
            }
        }

        public void Handle(AttackAchievementProgress @event)
        {
            _attackCount += @event.Count;
            _accumulatedAttackDamage += @event.Damage;
            if (_attackCount >= 10)
            {
                EventAggregator.RaiseEvent<AchievementSastifaction>(new AchievementSastifaction()
                {
                    Type = EAchievementType.AchievementAttack,
                });
            }
        }

        public void Handle(ToxicBallAchievementProgress @event)
        {
            _toxicBallCount += @event.Count;
            _accumulatedToxicBallDamage += @event.Damage;
            if (_toxicBallCount >= 10)
            {
                EventAggregator.RaiseEvent<AchievementSastifaction>(new AchievementSastifaction()
                {
                    Type = EAchievementType.AchievementToxicBall,
                });
            }
        }

        public void Handle(TrapEncounterAchievementProgress @event)
        {
            _getHurtByTrapCount += @event.Count;
            if (_getHurtByTrapCount >= 7)
            {
                EventAggregator.RaiseEvent<AchievementSastifaction>(new AchievementSastifaction()
                {
                    Type = EAchievementType.AchievementTrap,
                });
            }
        }

        public void Handle(LevelCompletionAchievementProgress @event)
        {
            if (!_completedScenes.Contains(@event.LevelName))
            {
                _completedScenes.Add(@event.LevelName);
                if (@event.LevelName == "Scenes/Chapter 1/Cemetary Graveyard")
                {
                    EventAggregator.RaiseEvent<AchievementSastifaction>(new AchievementSastifaction()
                    {
                        Type = EAchievementType.AchievementGraveyard,
                    });
                }
            }
        }
    }
}