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

    public GameObject battleDoor;
    public bool inBattle = false; 
    public bool gameOver = false;

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
        CheckForGameOver();
    }

    private bool isTransitioning = false;

    public void PageButtonClicked(string pageName)
    {
        if (isTransitioning) return;
        //Audio
        AudioManager.instance.PlaySFX("Button1");
        StartCoroutine(PageTransitionSequence(pageName));
    }

    private IEnumerator PageTransitionSequence(string pageName)
    {
        isTransitioning = true;

        Transition.instance.FadeOut();
        yield return new WaitForSeconds(0.2f);

        OpenPage(pageName);

        isTransitioning = false;
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

    public void LobbyState() //open lobby page
    {
        // Reset gameplay
        enemySpawner.ClearAllSpawnedEnemies();
        specialSpawner.ClearAllSpawnedEnemies();
        enemySpawner.spawnerActive = false;
        specialSpawner.spawnerActive = false;
        TeamManager.instance.ResetCatPosition(); //it has the debuffremoval here.
        TeamManager.instance.ResetHealthAllCats();
        

        TeamManager.instance.ClearDeadCatsList(); //delete all worldGOcat that died.
        TeamManager.instance.DisableAllCats();

        //reset cat Icons.
        CatIconUI.instance.ResetAllIconToInitialIndex();

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
        if (AudioManager.instance != null && Introduction.instance.inIntroduction == false)
        {
            AudioManager.instance.PlayTheme("Lobby");
        }

        DialogueManager.instance.resetDeathTriggers();
        gameOver = false;
        inBattle = false;

    }

    public void StartMission() // battle door pressed
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
        CanvasGroup canvasGRP = battleDoor.GetComponent<CanvasGroup>();
        canvasGRP.alpha = 0;
        canvasGRP.interactable = false;
        
        CatRoamLobby.instance.AllCatsMoveToBattleDoor();
        AudioManager.instance.PlaySFX("BattleStart");
        yield return new WaitForSeconds(1.2f);

        Transition.instance.FadeOut(); //auto fade om
        yield return new WaitForSeconds(0.2f);
        CloseAllPages();

        AudioManager.instance.PlayTheme("Battle");

        TeamManager.instance.EnableAllCats();
        CatRoamLobby.instance.DisableAllLobbyCat();

        CloseAllPages();
        // Wait for 2 seconds before enabling travel
        yield return new WaitForSeconds(0.2f);

        canvasGRP.alpha = 1;
        canvasGRP.interactable = true;//enable battle door again

        if (Tutorial.instance.inTutorial) //if in tutorial.
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
        inBattle = true;
        ResumeGameplay();
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
        ResumeGameplay();
        //remove all couroutine
        AudioManager.instance.PlaySFX("Retreat");
        yield return new WaitForSeconds(0.5f);
        AudioManager.instance.PlaySFX("Retreat2");

        // Open lobby page
        Transition.instance.FadeOut(); //auto fade om
        yield return new WaitForSeconds(0.2f);
        CloseAllPages();
        lobbyPage.pageObject.transform.position = lobbyPage.openPos.position;
        currentPage = lobbyPage;

        // Play lobby audio
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayTheme("Lobby");
        }

        LobbyState();

        //kill all enemies
        KillAllEnemies();
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
        DisableAllCatAbilities();
    }

    void DisableAllCatAbilities()
    {
        foreach(CatUnit cat in TeamManager.instance.cats)
        {
            if(cat.skill != null)
            {
                cat.skill.isActive = false;
            }
        }
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
        EnableAllCatAbilities();
    }
    void EnableAllCatAbilities()
    {
        foreach (CatUnit cat in TeamManager.instance.cats)
        {
            if(cat.skill != null)
            {
                cat.skill.isActive = true;
            }
        }
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

    public void CheckForGameOver()
    {
        //if all cats are dead
        if (inBattle && !gameOver)
        {
            if(TeamManager.instance.cats.Count == 0)
            {
                inBattle = false;
                gameOver = true;
                //Trigger the retreat sequence!
                DialogueManager.instance.BeginTalk(3);
            }
        }
        //force a retreat

        //prompt a dialogue.
    }


    public void KillAllEnemies()
    {
        SpawnerManager.instance.enemySpawner.ClearAllSpawnedEnemies();
        SpawnerManager.instance.specialEnemySpawner.ClearAllSpawnedEnemies();
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
