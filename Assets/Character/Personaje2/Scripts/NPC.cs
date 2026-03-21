using UnityEngine;

public class NPC : MonoBehaviour
{
    public enum NPCType
    {
        NPC1,
        NPC2,
        NPC3,
        NPC4,
        NPC5,
        NPC6
    }

    public NPCType npcType;

    public void Interact()
    {
        switch (npcType)
        {
            case NPCType.NPC1:
                Debug.Log("Hola soy Npc 1");
                break;

            case NPCType.NPC2:
                Debug.Log("Hola soy Npc 2");
                break;

            case NPCType.NPC3:
                Debug.Log("Hola soy Npc 3");
                break;

            case NPCType.NPC4:
                Debug.Log("Hola soy Npc 4");
                break;

            case NPCType.NPC5:
                Debug.Log("Hola soy Npc 5");
                break;

            case NPCType.NPC6:
                Debug.Log("Hola soy Npc 6");
                break;


        }
    }
}
