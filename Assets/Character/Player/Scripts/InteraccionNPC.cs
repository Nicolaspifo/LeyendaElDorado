using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InteraccionNPC : MonoBehaviour
{
    public float interactionRange = 2f;
    public InputActionReference InteractuarDialogo;
    public InputActionReference Cancelar;

    public GameObject dialogCanvas;
    public TMP_Text nameText;
    public TMP_Text dialogText;
    public InputActionReference avanzarDialogo1; // Espacio
    public InputActionReference avanzarDialogo2; // Flecha derecha/abajo

    private NPC currentNPC;
    private int dialogIndex = 0;
    private bool isTalking = false;

    public float typingSpeed = 0.02f; // velocidad de escritura

    private Coroutine typingCoroutine;
    private bool isTyping = false;

    public NavegacionInventario navegacionInventario;
    public PersonajeMovimiento_02 movimiento;


    void Start()
    {
        if (navegacionInventario == null)
            navegacionInventario = FindFirstObjectByType<NavegacionInventario>();
            
        if (movimiento == null)
            movimiento = FindFirstObjectByType<PersonajeMovimiento_02>();
    }

    private void OnEnable()
    {
        InteractuarDialogo.action.Enable();
        Cancelar.action.Enable();
    }

    private void OnDisable()
    {
        InteractuarDialogo.action.Disable();
        Cancelar.action.Disable();
    }


    void Update()
    {
        bool interactuar = InteractuarDialogo.action.WasPressedThisFrame();
        bool avanzar1    = avanzarDialogo1 != null && avanzarDialogo1.action.WasPressedThisFrame();
        bool avanzar2    = avanzarDialogo2 != null && avanzarDialogo2.action.WasPressedThisFrame();

        if (interactuar || avanzar1 || avanzar2)
        {
            if (isTalking)
                NextDialog();
            else if (interactuar) // solo Enter/E abre el diálogo, no espacio
                TryInteract();
        }

        if (isTalking && currentNPC != null)
        {
            float distance = Vector3.Distance(transform.position, currentNPC.transform.position);
            if (distance > interactionRange + 1f)
                EndDialog();
        }

        if (Cancelar.action.WasPressedThisFrame())
            EndDialog();
    }

     public NPC GetNPCDetectado()
    {
        float radio = 0.5f;
        Vector3 origin = transform.position + Vector3.up * 1.5f;

        RaycastHit hit;

        if (Physics.SphereCast(origin, radio, transform.forward, out hit, interactionRange))
        {
            NPC npc = hit.collider.GetComponent<NPC>();
            if (npc != null)
                return npc;
        }

        Collider[] hits = Physics.OverlapSphere(transform.position, interactionRange);

        float mejorDot = 0.5f;
        NPC mejorNPC = null;

        foreach (Collider col in hits)
        {
            NPC npc = col.GetComponent<NPC>();
            if (npc == null) continue;

            Vector3 dirToNPC = (npc.transform.position - transform.position).normalized;
            float dot = Vector3.Dot(transform.forward, dirToNPC);

            if (dot > mejorDot)
            {
                mejorDot = dot;
                mejorNPC = npc;
            }
        }

        return mejorNPC;
    }

    void TryInteract()
    {
        NPC npc = GetNPCDetectado();

        if (npc != null)
        {
            StartDialog(npc);
        }

        
        if (npc != null && npc.yainteractuado == false)
        {
            npc.yainteractuado = true;
            if (navegacionInventario != null)
                navegacionInventario.crearPersonajeUI(npc.npcName);
        }
    }

    void StartDialog(NPC npc)
    {
        currentNPC = npc;
        dialogIndex = 0;
        isTalking = true;
        dialogCanvas.SetActive(true);
        nameText.text = npc.npcName;

        if (movimiento != null) movimiento.bloqueado = true;

        IndicadorNPC indicador = npc.GetComponent<IndicadorNPC>();
        if (indicador != null) indicador.dialogoActivo = true;

        ShowCurrentDialog();
        currentNPC.ReproducirSonidoDialogo();
    }

    void NextDialog()
    {
        if (isTyping)
        {
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);

            dialogText.text = currentNPC.dialogos[dialogIndex];
            isTyping = false;
            return;
        }

        dialogIndex++;

        if (dialogIndex >= currentNPC.dialogos.Length)
        {
            EndDialog();
            return;
        }

        ShowCurrentDialog();
    }

    void ShowCurrentDialog()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(currentNPC.dialogos[dialogIndex]));
    }
    IEnumerator TypeText(string text)
    {
        isTyping = true;
        dialogText.text = "";

        foreach (char letter in text)
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    void EndDialog()
    {
        if (currentNPC != null)
        {
            IndicadorNPC indicador = currentNPC.GetComponent<IndicadorNPC>();
            if (indicador != null) indicador.dialogoActivo = false;
        }

        if (movimiento != null) movimiento.bloqueado = false;

        isTalking = false;
        dialogCanvas.SetActive(false);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}