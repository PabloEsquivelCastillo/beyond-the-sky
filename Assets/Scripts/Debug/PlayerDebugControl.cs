using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDebugControls : MonoBehaviour
{
    [SerializeField] private PlayerProgression player;

    private void Awake()
    {
        // Si olvidaste arrastrarlo en el Inspector, el código lo busca solo
        if (player == null)
        {
            player = GetComponent<PlayerProgression>();
        }
    }

    private void Update()
    {
        // Seguridad: si aún así no existe, no ejecuta nada para evitar crasheos
        if (player == null) return;

        if (Keyboard.current.kKey.wasPressedThisFrame)
        {
            player.ReceiveDamage(new DamageInfo(10, gameObject));
        }

        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            player.AddExperience(25);
        }
    }
}
