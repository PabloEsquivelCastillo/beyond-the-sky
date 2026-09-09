using UnityEngine;

public class PlayerInputReader : MonoBehaviour
{
    private NullBoundInputActions controls;

    public Vector2 Move { get; private set; }
    public Vector2 Look { get; private set; }

    private void OnEnable()
    {
        // Se crea el objeto de controles justo antes de activarlo
        if (controls == null)
        {
            controls = new NullBoundInputActions();
        }

        controls.Enable();
    }

    private void OnDisable()
    {
        // Solo lo desactiva si realmente fue creado
        if (controls != null)
        {
            controls.Disable();
        }
    }

    private void Update()
    {
        // Seguridad extra: si por alguna razón no existe, no ejecuta el Update
        if (controls == null) return;

        Move = controls.Player.Move.ReadValue<Vector2>();
        Look = controls.Player.Look.ReadValue<Vector2>();
    }
}
