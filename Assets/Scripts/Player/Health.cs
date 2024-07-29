using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Player
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private float maxHP = 100;
        [SerializeField] private UnityEvent onDeath;
        [SerializeField] private UnityEvent<float, float> onHealthChange;
        [SerializeField] private Rigidbody rb;

        [SerializeField] private float hp;

        [SerializeField] private int amountRestore;

        [SerializeField] private bool autoHeal;

        [SerializeField] private float autoHealStartTime = 10;
        [SerializeField] private float autoHealStartCooldown = 1;
        [SerializeField] private float autoHealAmount = 3;

        [SerializeField] private AudioSource soundOnDamage;
        [SerializeField] private AudioSource soundOnDeath;

        [SerializeField] private MilkShake.ShakePreset preset;

        [SerializeField] private ParticleSystem blood;
        [SerializeField] private ParticleSystem deathParticles;

        public float HP => hp;
        public void Initialize(float value)
        {
            hp = value;
            onHealthChange.Invoke(hp, maxHP);
            
        }
        private bool player;

        private void Start()
        {
            hp = maxHP;
            player = gameObject.CompareTag("Player");
        }
    
        void Heal()
        {
            ChangeHealth(autoHealAmount);
        }

        private Vector3 _buff;
        public void DealDamage(float damage, Vector3 direction, float kickForce , Vector3? point = null)
        {
            if (rb) rb.AddForce(-direction * kickForce, ForceMode.Impulse);

            if (soundOnDamage != null){
                soundOnDamage.pitch = Random.Range(0.75f, 1.25f);
                soundOnDamage.Play();
            }

            if (blood){
                //blood.transform.position = _buff;
                if (point != null)
                {
                    _buff = blood.transform.position;
                    blood.transform.position = point.Value;
                }
                if (!player){
                    blood.transform.LookAt(direction);
                }
                else if (Camera.main != null) 
                    blood.transform.forward = Camera.main.transform.forward;

                blood.Play();
            }

            if (player && preset != null) MilkShake.Shaker.ShakeAll(preset);

            if (autoHeal){
                CancelInvoke(nameof(Heal));
                InvokeRepeating(nameof(Heal), autoHealStartTime, autoHealStartCooldown);
            }
            ChangeHealth(-damage);
        }

        public void ChangeHealth(float delta)
        {
            hp += delta;
            if (hp >= maxHP){
                hp = maxHP;
                CancelInvoke(nameof(Heal));
            }

            if (hp > 0)
            {
                onHealthChange.Invoke(hp, maxHP);
                return;
            }
            if (deathParticles != null){
                deathParticles.transform.SetParent(null);
                deathParticles.Play(true);
                soundOnDeath.Play();
                Destroy(deathParticles.gameObject, 3);
            }
            onDeath.Invoke();
        }
    }
}