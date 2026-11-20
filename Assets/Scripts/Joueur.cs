using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using UnityEditor;
using Unity.Mathematics;

public class Joueur : MonoBehaviour
{
    [Header("Détection sol")]
    [SerializeField] LayerMask masqueSol;
    [SerializeField] float distance = 1000f;
    [SerializeField] float vitesseRotation = 5f;

    [Header("Tir")]
    [SerializeField] GameObject projectile;

    [Header("Gestion des touches")]
    [SerializeField] InputAction curseurAction;
    [SerializeField] InputAction tirAction;
    NavMeshAgent agent;
    Quaternion rotationCible;
    void OnEnable()
    {
        curseurAction.Enable();
        tirAction.Enable();

    }
    void OnDisable()
    {
        curseurAction.Disable();
        tirAction.Disable();
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false; //On gère la rotation manuellement
        rotationCible = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (GestionnaireJeu.instance.etatJeu != EtatJeu.JEU)
        {
            return;
        }




        if (curseurAction.WasPressedThisFrame())
        {
            Vector2 positionCurseur = Mouse.current.position.ReadValue();
            Ray rayon = Camera.main.ScreenPointToRay(positionCurseur);

            bool collision = Physics.Raycast(rayon, out RaycastHit hit, distance, masqueSol);
            if (collision)
            {
                agent.SetDestination(hit.point);
                Vector3 direction = hit.point - transform.position;
                rotationCible = Quaternion.LookRotation(direction);
            }
        }


        if (tirAction.WasPressedThisFrame())
        {
            GameObject clone = Instantiate(projectile, transform.position, transform.rotation);
            clone.SetActive(true);
        }

        // Chaque image, on tourne progressivement vers la rotation cible
        transform.rotation = Quaternion.Slerp(transform.rotation, rotationCible, vitesseRotation * Time.deltaTime);
    }

}
