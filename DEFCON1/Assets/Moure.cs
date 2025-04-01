using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class Moure : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 movement;
    public Animator animator;
    private bool isMoving = false;
    private Vector2 targetPosition;
    private string currentAnimation;
    private float movementStartTime;
    public float maxMovementTime = 1f;
    private float speed;
    public ParamsPersonatges paramsPersonatges;
    public int idEleccio;


    private PosaBombes posaBombes;


    public Tilemap cementTilemap;
    public Tilemap brickTilemap;


    public TileBase cementObstacleTile;
    public TileBase brickObstacleTile;

    public KeyCode adalt;
    public KeyCode dreta;
    public KeyCode abaix;
    public KeyCode esquerra;
    public KeyCode posarBomba;
    public Jugador1Script jugador1Script;
    public Jugador2Script jugador2Script;
    public int numeroJugador;
    
    private PartidaManagerScript partidaManagerScript;
    void Start()
    {
        partidaManagerScript = FindObjectOfType<PartidaManagerScript>();
        rb = GetComponent<Rigidbody2D>();
        targetPosition = rb.position;
        posaBombes = GetComponentInChildren<PosaBombes>();
        StartCoroutine(WaitForParamsPersonatges());
        if (numeroJugador == 1)
        {
            jugador1Script = FindObjectOfType<Jugador1Script>();
            idEleccio = jugador1Script.eleccio;
        }
        if (numeroJugador == 2)
        {
            jugador2Script = FindObjectOfType<Jugador2Script>();
            idEleccio = jugador2Script.eleccio;
        }
    }

    IEnumerator WaitForParamsPersonatges()
    {
        while (paramsPersonatges == null || paramsPersonatges.personatges.Count <= idEleccio)
        {
            paramsPersonatges = FindObjectOfType<ParamsPersonatges>();
            yield return null;
        }
        Personatge personatgeSeleccionat = (Personatge)paramsPersonatges.personatges[idEleccio];
        speed = personatgeSeleccionat.velocitat;
        posaBombes.maxBombes = personatgeSeleccionat.bombesSimultanies;
        posaBombes.radiExplosio = personatgeSeleccionat.forcaExplosions;
    }

    void Update()
    {
        if (!isMoving)
        {
            movement = Vector2.zero;

            if (Input.GetKey(adalt) || Input.GetKey(abaix))
            {
                if (Input.GetKey(adalt))
                {
                    movement.y = 1;
                    SetAnimationState("Adalt");
                }
                else if (Input.GetKey(abaix))
                {
                    movement.y = -1;
                    SetAnimationState("Abaix");
                }
            }
            else if (Input.GetKey(dreta) || Input.GetKey(esquerra))
            {
                if (Input.GetKey(dreta))
                {
                    movement.x = 1;
                    SetAnimationState("Dreta");
                }
                else if (Input.GetKey(esquerra))
                {
                    movement.x = -1;
                    SetAnimationState("Esquerra");
                }
            }

            if (movement != Vector2.zero)
            {
                if (CanMoveTo(targetPosition + movement))
                {
                    targetPosition = rb.position + movement;
                    isMoving = true;
                    movementStartTime = Time.time;
                }
                else
                {
                    animator.SetBool("Parat", true);
                }
            }
        }

        if (isMoving)
        {
            MoveToTarget();
            partidaManagerScript.AugmentarComptadorDistancia();
        }

        if (Input.GetKeyDown(posarBomba))
        {
            posaBombes?.PosarBomba();
            partidaManagerScript.AugmentarComptadorBombes();
            animator.SetBool("AnimacioBomba", true);
            Invoke(nameof(ResetAnimacioBomba), 0.23f);
        }
    }

    void ResetAnimacioBomba()
    {
        animator.SetBool("AnimacioBomba", false);
    }

    void MoveToTarget()
    {

        float distance = speed * Time.deltaTime;
        rb.position = Vector2.MoveTowards(rb.position, targetPosition, distance);

        if (rb.position == targetPosition)
        {
            isMoving = false;
            animator.SetBool("Parat", true);
        }
    }

    void SetAnimationState(string direction)
    {

        animator.SetBool("Adalt", false);
        animator.SetBool("Abaix", false);
        animator.SetBool("Dreta", false);
        animator.SetBool("Esquerra", false);


        animator.SetBool(direction, true);
        animator.SetBool("Parat", false);


        currentAnimation = direction;
    }

    bool CanMoveTo(Vector2 position)
    {
        Vector3Int cellPosition = cementTilemap.WorldToCell(position);
        TileBase cementTile = cementTilemap.GetTile(cellPosition);
        TileBase brickTile = brickTilemap.GetTile(cellPosition);

        if ((cementTile == cementObstacleTile) || (brickTile == brickObstacleTile))
        {
            return false;
        }

        Collider2D bombCollider = Physics2D.OverlapBox(position, new Vector2(0.5f, 0.5f), 0f, LayerMask.GetMask("Bomba"));
        return bombCollider == null;
    }

    public void AugmentarSpeed()
    {
        speed++;
        partidaManagerScript.AugmentarComptadorPowerups();
    }
    public void DisminuirSpeed()
    {
        if (speed != 1)
        {
            speed--;
            partidaManagerScript.AugmentarComptadorPowerups();
        }
    }
    public void AugmentarFoc()
    {
        posaBombes.AugmentarRadiExplosio();
        partidaManagerScript.AugmentarComptadorPowerups();
    }

    public void DisminuirFoc()
    {
        posaBombes.DisminuirRadiExplosio();
        partidaManagerScript.AugmentarComptadorPowerups();
    }
    public void AugmentarBombes()
    {
        posaBombes.AugmentarMaxBombes();
        partidaManagerScript.AugmentarComptadorPowerups();
    }
    public void DisminuirBombes()
    {
        posaBombes.DisminuirMaxBombes();
        partidaManagerScript.AugmentarComptadorPowerups();
    }
}
