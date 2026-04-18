using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InteraccionNPC : MonoBehaviour
{
    public float interactionRange = 2f;
    public KeyCode interactionKey = KeyCode.E;
    public InputActionReference Interactuar;
    public InputActionReference Cancelar;

    public GameObject dialogCanvas;
    public TMP_Text nameText;
    public TMP_Text dialogText;

    private NPC currentNPC;
    private int dialogIndex = 0;
    private bool isTalking = false;

    public float typingSpeed = 0.02f; // velocidad de escritura

    private Coroutine typingCoroutine;
    private bool isTyping = false;

    private void OnEnable()
    {
        Interactuar.action.Enable();
        Cancelar.action.Enable();
    }

    private void OnDisable()
    {
        Interactuar.action.Disable();
        Cancelar.action.Disable();
    }


    void Update()
    {
        if (Interactuar.action.WasPressedThisFrame())
        {
            if (isTalking)
            {
                NextDialog();
            }
            else
            {
                TryInteract();
            }
        }

        if (isTalking && currentNPC != null)
        {
            float distance = Vector3.Distance(transform.position, currentNPC.transform.position);

            if (distance > interactionRange + 1f) // margen extra opcional
            {
                EndDialog();
            }
        }

        if (Cancelar.action.WasPressedThisFrame())
        {
            EndDialog();
        }
    }

    void TryInteract()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, interactionRange);

        foreach (Collider hit in hits)
        {
            NPC npc = hit.GetComponent<NPC>();

            if (npc != null)
            {
                Vector3 directionToNPC = (npc.transform.position - transform.position).normalized;
                float dot = Vector3.Dot(transform.forward, directionToNPC);

                if (dot > 0.7f)
                {
                    StartDialog(npc);
                    return;
                }
            }
        }
    }

    void StartDialog(NPC npc)
    {
        currentNPC = npc;
        dialogIndex = 0;
        isTalking = true;

        dialogCanvas.SetActive(true);
        nameText.text = npc.npcName;

        ShowCurrentDialog();
    }

    void NextDialog()
    {
        // 👇 si aún está escribiendo → termina instantáneamente
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
        isTalking = false;
        dialogCanvas.SetActive(false);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}