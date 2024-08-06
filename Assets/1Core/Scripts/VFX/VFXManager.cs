using System.Collections;
using Core.Scripts.Tools.Extensions;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] _particles;

    public IEnumerator PlayVFX(float zPosition)
    {
        var rand = Random.Range(0, _particles.Length);
        _particles[rand].transform.position = _particles[rand].transform.position.SetZ(zPosition);
        _particles[rand].gameObject.SetActive(true);
        _particles[rand].Play();
        AudioManager.instance.PlaySoundEffect((SoundType)rand);
        yield return new WaitWhile(() => _particles[rand].IsAlive(true));

        _particles[rand].Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        _particles[rand].gameObject.SetActive(false);
    }
}