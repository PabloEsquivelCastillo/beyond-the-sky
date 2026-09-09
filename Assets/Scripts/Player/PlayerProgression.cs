using System;
using UnityEngine;

public class PlayerProgression : MonoBehaviour
{
    [Header("Vida")]
    [SerializeField] private int maxHealth = 100;

    [Header("Progresión")]
    [SerializeField] private int startingLevel = 1;
    [SerializeField] private int experienceToFirstLevel = 100;

    public int CurrentHealth { get; private set; }
    public int MaxHealth => maxHealth;
    public int CurrentLevel { get; private set; }
    public int CurrentExperience { get; private set; }
    public int ExperienceToNextLevel { get; private set; }
    public bool IsDead { get; private set; }

    public event Action<DamageInfo> DamageReceived;
    public event Action<int, int> HealthChanged;
    public event Action Died;
    public event Action<int, int> ExperienceChanged;
    public event Action<int> LevelChanged;

    private void Awake()
    {
        CurrentHealth = maxHealth;
        CurrentLevel = startingLevel;
        ExperienceToNextLevel = experienceToFirstLevel;

        Debug.Log($"<color=cyan>[Progreso Inicial]</color> Vida: {CurrentHealth}/{maxHealth} | Nivel: {CurrentLevel}");
    }

    public void ReceiveDamage(DamageInfo damage)
    {
        if (IsDead || damage.Amount <= 0)
        {
            return;
        }

        CurrentHealth = Mathf.Max(CurrentHealth - damage.Amount, 0);

        // Mensaje de daño recibido
        Debug.Log($"<color=orange>[Daño]</color> Recibió {damage.Amount} de daño de {damage.Source.name}. Vida restante: {CurrentHealth}/{maxHealth}");

        DamageReceived?.Invoke(damage);
        HealthChanged?.Invoke(CurrentHealth, maxHealth);

        if (CurrentHealth == 0)
        {
            IsDead = true;
            Debug.Log("<color=red><b>[MUERTE] El jugador ha muerto. Evento Died disparado.</b></color>");
            Died?.Invoke();
        }
    }

    public void AddExperience(int amount)
    {
        if (IsDead || amount <= 0)
        {
            return;
        }

        CurrentExperience += amount;
        Debug.Log($"<color=yellow>[XP Ganada]</color> +{amount} XP. Total actual: {CurrentExperience}/{ExperienceToNextLevel}");

        while (CurrentExperience >= ExperienceToNextLevel)
        {
            CurrentExperience -= ExperienceToNextLevel;
            CurrentLevel++;
            ExperienceToNextLevel = CalculateExperienceForNextLevel();

            Debug.Log($"<color=green><b>[LEVEL UP] ¡Subiste de nivel! Nivel actual: {CurrentLevel}</b></color>");
            LevelChanged?.Invoke(CurrentLevel);
        }

        ExperienceChanged?.Invoke(
            CurrentExperience,
            ExperienceToNextLevel
        );
    }

    private int CalculateExperienceForNextLevel()
    {
        return experienceToFirstLevel + (CurrentLevel - 1) * 50;
    }
}
