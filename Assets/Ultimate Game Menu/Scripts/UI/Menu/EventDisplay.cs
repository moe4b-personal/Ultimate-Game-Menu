using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class EventDisplay : MonoBehaviour {

    [SerializeField]
    GameObject[] enables;
    [SerializeField]
    GameObject[] disables;

    EventTrigger trigger;

	void Start()
	{
        trigger = GetComponent<EventTrigger>();

        if (!trigger)
            trigger = gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry pDownEve = new EventTrigger.Entry();
        pDownEve.callback.AddListener(delegate { Enable(); });
        pDownEve.eventID = EventTriggerType.PointerDown;

        EventTrigger.Entry pUpEve = new EventTrigger.Entry();
        pUpEve.callback.AddListener(delegate { Disable(); });
        pUpEve.eventID = EventTriggerType.PointerUp;

        trigger.triggers.Add(pDownEve);
        trigger.triggers.Add(pUpEve);
    }

    void Enable()
    {
        for (int i = 0; i < enables.Length; i++)
        {
            enables[i].SetActive(true);
        }
    }

    void Disable()
    {
        for (int i = 0; i < disables.Length; i++)
        {
            disables[i].SetActive(false);
        }
    }
}
