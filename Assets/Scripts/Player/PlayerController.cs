using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // La vitesse des personnages individuels
    [SerializeField] private int m_playerAgentSpeed;
    // Le nombre de personnages, modifie cette valeur pour ajouter ou enlever des personnages, le code fait le reste !
    [SerializeField] private int m_playerNumber;
    // Le rayon du cercle dans lequel les personnages spawnent
    [SerializeField] private int m_playerRadius;
    // Le perfab des personnages, c'est eux qui vont tirrer et c'est eux qui suivent le joueur
    [SerializeField] private GameObject m_playerAgentPrefab;
    [SerializeField] private GameObject m_gameOver;

    // 🔹 Variables du déplacement 
    [SerializeField] private float speed = 5f; // Vitesse du joueur


    private List<Vector2> m_circlePoints = new List<Vector2>();
    private List<PlayerAgent> m_agents = new List<PlayerAgent>();
    private float m_phi = (1 + Mathf.Sqrt(5)) / 2;
    private int m_lastNumber = 0;


    public void AddAgent(int agentNumber)
    {
        m_playerNumber += agentNumber; // += ajoute le nombre exacte que tu veux alors que == ajoute 1
        RespawnPlayers();
    }

    public void DeleteAgent(PlayerAgent hitAgent)
    {
        //PlayerAgent agent = m_agents[m_playerNumber - 1];
        m_agents.Remove(hitAgent);
        Destroy(hitAgent.gameObject);
        m_playerNumber--;
        
        if (m_playerNumber <= 0)
        {
            m_gameOver.SetActive(true);
            Time.timeScale = 0;
        }

    }

    private Rigidbody rb;
   
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        RespawnPlayers();
    }

    void Update()
    {

    // deplacements

        float moveInput = 0f;

        if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)))
            moveInput = -1f; // Correction du sens du déplacement

        if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.Q)))
            moveInput = 1f;

            rb.velocity = new Vector3(moveInput * speed, rb.velocity.y, 0);


        // Normalement tu devrais pas trop avoir a toucher au reste de update !

        // On récupère une liste de points dans un cercle, qu'on utilise pour faire spawn les personnages



        // Déplacent les personnages vers leur position cible autour du joueur (le point qui est généré avec GeneratePointsInSphere et qui leur est assigné)
        for (int i = 0; i < m_playerNumber; i++)
        {
            if (m_agents.Count >= i)
            {

                Vector2 position = new Vector2((m_circlePoints[i + 1].x * m_playerRadius) + transform.position.x, (m_circlePoints[i + 1].y * m_playerRadius) + transform.position.z);

                float radiusFactor = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), position) / m_playerRadius;
                radiusFactor = Mathf.Clamp(radiusFactor, 0, 1);
                float interpRadiusFactor = Mathf.Lerp(1, 0.3f, radiusFactor);
                m_agents[i].m_radiusFactor = interpRadiusFactor;


                m_agents[i].m_targetPos = position;
                
            }
        }
    }

    // Affiche la position de spawn des personnages, tu peux le supprimer si tu veux
    private void OnDrawGizmos()
    {

        foreach (Vector2 point in m_circlePoints)
        {
            Vector3 position = new Vector3((point.x * m_playerRadius) + transform.position.x, transform.position.y, (point.y * m_playerRadius) + transform.position.z);
            Gizmos.DrawWireSphere(position, 1);
        }
        Gizmos.DrawWireSphere(transform.position, m_playerRadius);
    }

    // Génère les points dans un cercle, ne t'embête pas avec, c'est des maths compliquées que je ne comprends pas non plus
    public List<Vector2> GeneratePointsInSphere(int count, float radius)
    {
        List<Vector2> points = new List<Vector2>(count);
        float angleStride = 360 * m_phi;
        int b = (int)Mathf.Round(radius * Mathf.Sqrt(count));
        for (int i = 0; i < count + 1; i++)
        {
            float r = Radius(i, count, b);
            float theta = i * angleStride;
            points.Add(new Vector2(r * Mathf.Cos(theta), r * Mathf.Sin(theta)));
        }
        return points;
    }
    // Aussi nécessaire pour générer les points
    public float Radius(float k, float n, float b)
    {
        if (k > n - b)
        {
            return 1;
        }
        else
        {
            return Mathf.Sqrt(k - 0.5f) / Mathf.Sqrt(n - (b + 1) / 2);
        }
    }

    private void RespawnPlayers()
    {
        m_circlePoints.Clear();
        m_circlePoints = GeneratePointsInSphere(m_playerNumber, 1);

        m_lastNumber = m_playerNumber;
        foreach (PlayerAgent agent in m_agents)
        {
            Destroy(agent.gameObject);
        }

        m_agents.Clear();
        for (int i = 0; i < m_playerNumber; i++)
        {
            Vector2 position = new Vector2((m_circlePoints[i + 1].x * m_playerRadius) + transform.position.x, (m_circlePoints[i + 1].y * m_playerRadius) + transform.position.z);

            m_agents.Add(Instantiate(m_playerAgentPrefab).GetComponent<PlayerAgent>());
            m_agents[i].controller = this;

            m_agents[i].transform.position = transform.position;
            m_agents[i].m_targetPos = position;
            m_agents[i].m_moveSpeed = m_playerAgentSpeed;
        }
    }
}
