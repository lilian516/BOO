
NOMENCLATURE :

Constante : PRIVATE_CONSTANT_SNAKE_PASCAL_CASE

champ Static : s_camelCase

Nom de classe : PascalCase

Variable privée : int _camelCase

variable protected : int _camelCase

Variable publique : int PascalCase

variable delegate : IntEvent
variable event : OnNomdelEvent

private int MethodPascalCase(int paramCamelCase = 1)
{

        int variableCamelCase = paramCamelCase++;
        
        return variableCamelCase;
        
}

Création d'Assets / Object dans unity : initial du type_nom (M_monMaterial) sauf pour scène et script

Pour rajouter un son :
mettre son fichier au chemin d'accès -> Ressources/Audio/Catégorie/VotreSon.mp3

Commit Github:

[ADD] New feature

[UPT] Update feature

[FIX] Fix de feature

[REF] Refactor/Rework code

[CLEAN] Clean code
[DOC] Documentation code

Espacially for gameplay developper
[MAP] Level design
[BAL] Balancing the game

Only Git master
[MERGE] Commits de merge



// Informations importantes

Les Scènes importantes : 

Assets -> _Project -> Scenes 
Systems : load cette scène et lancer le jeu, vous pourrez tester le jeu ainsi

Assets -> _Project -> Scenes -> UI
MainMenu : le menu du jeu
UIInGame : les UI pendant le jeu

Assets -> _Project -> Scenes -> UI
MainScene : La scène principale avec tout l'environnement


Les scripts importants :

Assets -> _Project -> _Scripts -> Player -> StateMachine
StateMachine.cs : la logique de la state machine
State.cs : la logique des states

Assets -> _Project -> _Scripts -> Player -> StateMachine -> ConcreteStates
Touts les States du Joueur sont ici

Assets -> _Project -> _Scripts -> Manager -> Input
InputManager.cs : le script qui gère les inputs de l'Input Action

Assets -> _Project -> _Scripts -> Skills
Skill.cs : le script de base skill
On a ensuite les skills qui hérite de cette classe de base

