using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EInputEmitterGroup : MonoBehaviour
{
    [System.Serializable]
    private class GroupedEmitter
    {
        public GroupedEmitter(EInputEmitter _emitter, EInputEmitterGroup _parent, float _zRotationOnStart, float _zRotationRandomOffset = 0, float _randomDelay = 0)
        {
            emitter = _emitter;
            Parent = _parent;
            ZRotationOnStart = _zRotationOnStart;

            randomDelay = _randomDelay;
            zRotationRandomOffset = _zRotationRandomOffset;
        }

        public EInputEmitter emitter;
        [Space]
        public float zRotationRandomOffset = 0;
        public float randomDelay = 0;

        public float ZRotationOnStart { get; set; }
        public EInputEmitterGroup Parent { get; set; }

        public void Emit()
        {
            float delay = Random.Range(0, randomDelay);

            if (delay <= 0)
            {
                float randomOffset = Random.Range(-zRotationRandomOffset, zRotationRandomOffset);
                Vector3 newEulerRotation = new Vector3(0, 0, ZRotationOnStart + randomOffset);
                emitter.transform.eulerAngles = newEulerRotation;
                emitter.Emit();
            }
            else
            {
                Parent.StartCoroutine(EmitDelayed(delay));
            }
        }

        public IEnumerator EmitDelayed(float delay)
        {
            yield return new WaitForSeconds(delay);

            float randomOffset = Random.Range(-zRotationRandomOffset, zRotationRandomOffset);
            Vector3 newEulerRotation = new Vector3(0, 0, ZRotationOnStart + randomOffset);
            emitter.transform.eulerAngles = newEulerRotation;
            emitter.Emit();
            yield break;
        }
    }

    [SerializeField] List<GroupedEmitter> emitters = new List<GroupedEmitter>();

    private void Start()
    {
        foreach (GroupedEmitter emitter in emitters)
        {
            emitter.ZRotationOnStart = emitter.emitter.transform.eulerAngles.z;
            emitter.Parent = this;
        }
    }

    public void EmitAll()
    {
        foreach (GroupedEmitter emitter in emitters)
        {
            emitter.Emit();
        }
    }

    public void MultiEditRandomRotation(float value)
    {
        foreach (GroupedEmitter emitter in emitters)
        {
            emitter.zRotationRandomOffset = value;
        }
    }

    public void MultiEditRandomDelay(float value)
    {
        foreach (GroupedEmitter emitter in emitters)
        {
            emitter.randomDelay = value;
        }
    }

    public void GetEEmittersFromChildren()
    {
        EInputEmitter[] newEmitters = gameObject.GetComponentsInChildren<EInputEmitter>();

        emitters = new List<GroupedEmitter>();
        foreach (EInputEmitter emitter in newEmitters)
        {
            GroupedEmitter groupedEmitter = new GroupedEmitter(emitter, this, emitter.transform.eulerAngles.z);
            emitters.Add(groupedEmitter);
        }
    }
}
