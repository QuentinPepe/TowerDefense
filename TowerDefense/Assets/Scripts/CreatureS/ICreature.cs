using UnityEngine;

namespace CreatureS
{
    public interface ICreature
    {
        CreatureSO Data { get; }
        int CurrentHealth { get; }
        void Initialize(CreatureSO creatureData);
        void TakeDamage(int damage);
        void MoveTo(Vector3 target);
        void TeleportTo(Vector3 position);
        void SetActive(bool active);
        bool IsActive();
    }
}