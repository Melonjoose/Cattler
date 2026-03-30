using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public EnemySpawner enemySpawner;    // reference to spawner if needed
    public SpecialEnemySpawner specialSpawner; // reference to special spawner if needed

    public PlayerData playerData;

    [System.Serializable]
    public class Page
    {
        public string name;               // e.g. "Lobby"
        public GameObject pageObject;     // the panel
        public Transform openPos;         // target position when opened
        public Transform closedPos;       // target position when closed
    }

    [Header("All Pages")]
    public List<Page> pages = new List<Page>();

    [SerializeField]private Page currentPage;

    void Awake()
    {
        instance = this;

        //default state at lobby.
        
    }
    private void Start()
    {
        // Start at lobby
        LobbyState();
    }
    void Update()
    {

    }


    public void OpenPage(string pageName)
    {
        // close current
        if (currentPage != null)
        {
            currentPage.pageObject.transform.position = currentPage.closedPos.position;
        }

        // find new page
        Page newPage = pages.Find(p => p.name == pageName);
        if (newPage != null)
        {
            newPage.pageObject.transform.position = newPage.openPos.position;
            currentPage = newPage;
        }
        else
        {
            Debug.LogWarning($"Page {pageName} not found!");
        }

        if (currentPage.name == "Lobby")
        {
            CatRoamLobby.instance.EnableAllLobbyCat();
        }
        else
        {
            CatRoamLobby.instance.DisableAllLobbyCat();
        }
            //Audio
            AudioManager.instance.PlaySFX("Button1");

    }

    public void CloseCurrentPage()
    {
        if (currentPage != null)
        {
            currentPage.pageObject.transform.position = currentPage.closedPos.position;
            currentPage = null;
        }

        if (currentPage.name == "Lobby")
        {
            CatRoamLobby.instance.EnableAllLobbyCat();
        }
        else
        {
            CatRoamLobby.instance.DisableAllLobbyCat();
        }
    }

    public void CloseAllPages()
    {
        foreach (Page p in pages)
        {
            p.pageObject.transform.position = p.closedPos.position;
        }
        currentPage = null;
    }

    public void CloseTab(GameObject Tab)
    {
        if(Tab != null)
        {
            Tab.SetActive(false);
        }
    }
    public void OpenTab(GameObject Tab)
    {
        if (Tab != null)
        {
            Tab.SetActive(true);
        }
    }

    public Page lobbyPage; // assign in inspector

    public void LobbyState()
    {
        // Reset gameplay
        enemySpawner.ClearAllSpawnedEnemies();
        specialSpawner.ClearAllSpawnedEnemies();
        enemySpawner.spawnerActive = false;
        specialSpawner.spawnerActive = false;
        TeamManager.instance.ResetCatPosition();
        TeamManager.instance.ResetHealthAllCats();
        


        TeamManager.instance.ClearDeadCatsList(); //delete all worldGOcat that died.
        TeamManager.instance.DisableAllCats();
        
        TravelManager.instance.ResetToStart();
        TravelManager.instance.DisableTravel();

        // Close all pages
        CloseAllPages();

        // Open lobby page
        lobbyPage.pageObject.transform.position = lobbyPage.openPos.position;
        currentPage = lobbyPage;

        if (CatRoamLobby.instance.catInLobby != null && CatRoamLobby.instance.catInLobby.Count > 0)
        {
            CatRoamLobby.instance.EnableAllLobbyCat();
        }
        else
        {
            Debug.Log("No cats in lobby to enable.");
        }


        // Play lobby audio
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayTheme("Lobby");
        }

    }

    public void StartMission()
    {
        if(TeamManager.instance.cats.Count == 0) //if there is no cats in cats list, cannot start mission.
        {
            Debug.LogWarning("Cannot start mission with no cats in the team!");
            CommentaryManager.instance.AddDialogueToQueue(7); // No cats dialogue
            return;
        }


        StartCoroutine(GameStartSequence());

        Currency.instance.ResetCurrencyForNewMission(); // Reset mission-specific currency tracking

        //ResumeGamePlay();
        // spawner active
        enemySpawner.spawnerActive = true;
        specialSpawner.spawnerActive = true;


        // make sure ink & core is same from lobbystate.

        // travel manager active

        //optional. catkeeper words of encouragement.
    }
    IEnumerator GameStartSequence()
    {
        CatRoamLobby.instance.AllCatsMoveToBattleDoor();
        AudioManager.instance.PlaySFX("BattleStart");
        yield return new WaitForSeconds(1f);
        
        Transition.instance.FadeOut();
        yield return new WaitForSeconds(1.5f);
        AudioManager.instance.PlayTheme("Battle");

        TeamManager.instance.EnableAllCats();
        CatRoamLobby.instance.DisableAllLobbyCat();

        CloseAllPages();
        Transition.instance.FadeIn();
        // Wait for 2 seconds before enabling travel
        yield return new WaitForSeconds(0.2f);




        if (Tutorial.instance.inTutorial)
        {
            TravelManager.instance.DisableTravel(); // cannot move during tutorial
             Tutorial.instance.TutorialLevelStart();
        }
        else
        {
            TravelManager.instance.EnableTravel();
            TravelManager.instance.ResetToStart();
            SpawnerManager.instance.StartLevelOne();
        }
    }


    public void RetreatButton()         //When button is clicked.
    {
        //pause the game.
        FreezeGameplay();
        //confirm button pops up.
    }



    public void ReturnToBase() // sequence when confirm button is clicked.
    {
        ResumeGameplay();
        // When confirm button is clicked.
        // goes to a summary page.
        //summary page shows rewards gained from the mission.
        //shows total ink earned. shows items gained. shows core gained.
        //shows cats that survived.
        //shows cats that died.
        //converts cats to ink.(money)
    }

    public void RetreatConfirmPressed() //Confirm pressed inside retreat page.
    {
        StartCoroutine(RetreatSequence());
    }

    IEnumerator RetreatSequence()
    {
        TeamManager.instance.MakeAllCatImmortal();
        //remove all couroutine
        Transition.instance.FadeOut();
        AudioManager.instance.PlaySFX("Retreat");
        yield return new WaitForSeconds(0.5f);
        AudioManager.instance.PlaySFX("Retreat2");
        yield return new WaitForSeconds(1.5f);
        ResumeGameplay();
        // Open lobby page
        lobbyPage.pageObject.transform.position = lobbyPage.openPos.position;
        currentPage = lobbyPage;

        // Play lobby audio
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayTheme("Lobby");
        }

        LobbyState();

        Transition.instance.FadeIn();
        // Wait for 2 seconds before enabling travel
        yield return new WaitForSeconds(0.2f);

    }

    public void ResetDemo()
    {
        // Reload it by name
        SceneManager.LoadScene(0);

    }

    public void Freeze()
    {
        Time.timeScale = 0f; // Freeze the game
    }
    public void UnFreeze()
    {
        Time.timeScale = 1f; // UnFreeze the game
    }

    public void FreezeGameplay()
    {
        EnemySpawner.instance.spawnerActive = false;
        SpecialEnemySpawner.instance.spawnerActive = false;
        //travel is disabled.
        TravelManager.instance.DisableTravel();
        //find all enemy, stop their movement and attack.
        //stop all enemy animation.
        foreach (var enemy in EnemySpawner.instance.spawnedEnemies)
        {
            EnemyUnit enemyUnit = enemy.GetComponent<EnemyUnit>();
            enemyUnit.canWalk = false;
            enemyUnit.canAttack = false;
            enemyUnit.skeletonAnimation.AnimationState.SetAnimation(0, "Walk", true);
        }

        foreach (var enemy in SpecialEnemySpawner.instance.spawnedEnemies)
        {
            EnemyUnit enemyUnit = enemy.GetComponent<EnemyUnit>();
            enemyUnit.canWalk = false;
            enemyUnit.canAttack = false;
            enemyUnit.skeletonAnimation.AnimationState.SetAnimation(0, "Walk", true);
        }
        //stop all cat movement and attack.
        //stop all cat animation.
        foreach (CatUnit cat in TeamManager.instance.cats)
        {
            cat.canWalk = false; 
            cat.canAttack = false;
            if (cat.skeletonAnimation != null)
            {
                cat.skeletonAnimation.AnimationState.SetAnimation(0, "Idle", true);
            }
            //movement drag enabled false.
            MovementDrag moveDrag = cat.GetComponent<MovementDrag>();
            if (moveDrag != null)
            {
                moveDrag.enabled = false;
            }
        }
        //disable controls. skills disabled.
        CatIconUI.instance.DisableAllSkills();
    }

    public void ResumeGameplay()
    {
        EnemySpawner.instance.spawnerActive = true;
        SpecialEnemySpawner.instance.spawnerActive = true;
        TravelManager.instance.EnableTravel();
        //resume all enemy movement and attack.
        foreach (var enemy in EnemySpawner.instance.spawnedEnemies)
        {
            EnemyUnit enemyUnit = enemy.GetComponent<EnemyUnit>();
            enemyUnit.canWalk = true;
            enemyUnit.canAttack = true;
            enemyUnit.skeletonAnimation.AnimationState.SetAnimation(0, "Walk", true);
        }

        foreach (var enemy in SpecialEnemySpawner.instance.spawnedEnemies)
        {
            EnemyUnit enemyUnit = enemy.GetComponent<EnemyUnit>();
            enemyUnit.canWalk = true;
            enemyUnit.canAttack = true;
            enemyUnit.skeletonAnimation.AnimationState.SetAnimation(0, "Walk", true);
        }

        foreach (CatUnit cat in TeamManager.instance.cats)
        {
            cat.canWalk = true;
            cat.canAttack = true;
            if (cat.skeletonAnimation != null)
            {
                cat.skeletonAnimation.AnimationState.SetAnimation(0, "Walk", true);
            }
            //movement drag enabled false.
            MovementDrag moveDrag = cat.GetComponent<MovementDrag>();
            if (moveDrag != null)
            {
                moveDrag.enabled = true;
            }
        }
        CatIconUI.instance.EnableAllSkills();
    }

    public void EnableAllCats() //movement only. for tutorial.
    {
        foreach (CatUnit cat in TeamManager.instance.cats)
        {
            cat.canWalk = true;
            cat.canAttack = false;
            if (cat.skeletonAnimation != null)
            {
                cat.skeletonAnimation.AnimationState.SetAnimation(0, "Idle", true);
            }
            //movement drag enabled false.
            MovementDrag moveDrag = cat.GetComponent<MovementDrag>();
            if (moveDrag != null)
            {
                moveDrag.enabled = true;
            }
        }
    }

    /// ------------------------------- < Gameplay> ------------------------------------///
    //phases of the game during travel.
    //1. Level 1. 0 - 5km
    //2. Level 2. 5 - 10km
    //3. MINI BOSS SPAWN POINT (10KM)
    //4. Level 3. 10 - 15km
    //5. Level 4. 15 - 20km
    //6. BOSS SPAWN POINT (20KM)
    // End Demo.
}
