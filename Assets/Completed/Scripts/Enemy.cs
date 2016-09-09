using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Completed
{
	using UnityEngine.UI;					//Allows us to use UI.

	//Enemy inherits from MovingObject, our base class for objects that can move, Player also inherits from this.
	public class Enemy : MovingObject
	{
		public int playerDamage; 							//The amount of food points to subtract from the player when attacking.
		public AudioClip attackSound1;						//First of two audio clips to play when attacking the player.
		public AudioClip attackSound2;						//Second of two audio clips to play when attacking the player.
		public int MaxG;									//This setting determines maximum path value for a star
		
		
		private Animator animator;							//Variable of type Animator to store a reference to the enemy's Animator component.
		private Transform target;							//Transform to attempt to move toward each turn.
		private bool skipMove;								//Boolean to determine whether or not enemy should skip a turn or move this turn.

		private AStar aStar = new AStar();
		private List<Point> waypoints = new List<Point>();             // A-Star path
		private BoardManager boardScript;		// Store a reference to our BoardManager which will set up the level.\

		private Text zombieText;									//Text to display zombie thoughts.
		
		
		//Start overrides the virtual Start function of the base class.
		protected override void Start ()
		{
			//Register this enemy with our instance of GameManager by adding it to a list of Enemy objects. 
			//This allows the GameManager to issue movement commands.
			GameManager.instance.AddEnemyToList (this);
			
			//Get and store a reference to the attached Animator component.
			animator = GetComponent<Animator> ();
			
			//Find the Player GameObject using it's tag and store a reference to its transform component.
			target = GameObject.FindGameObjectWithTag ("Player").transform;

			//Get a component reference to the attached BoardManager script
			boardScript = FindObjectOfType<BoardManager>();

			//Get a reference to our text LevelText's text component by finding it by name and calling GetComponent.
			//zombieText = GameObject.Find("ZombieText").GetComponent<Text>();
			
			//Call the start function of our base class MovingObject.
			base.Start ();
		}
		
		
		//Override the AttemptMove function of MovingObject to include functionality needed for Enemy to skip turns.
		//See comments in MovingObject for more on how base AttemptMove function works.
		protected override void AttemptMove <T> (int xDir, int yDir)
		{
			//Check if skipMove is true, if so set it to false and skip this turn.
			if(skipMove)
			{
				skipMove = false;
				return;
				
			}
			
			//Call the AttemptMove function from MovingObject.
			base.AttemptMove <T> (xDir, yDir);
			
			//Now that Enemy has moved, set skipMove to true to skip next move.
			skipMove = true;
		}
		
		
		//MoveEnemy is called by the GameManger each turn to tell each Enemy to try to move towards the player.
		public void MoveEnemy ()
		{
			//Declare variables for X and Y axis move directions, these range from -1 to 1.
			//These values allow us to choose between the cardinal directions: up, down, left and right.
			int xDir = 0;
			int yDir = 0;
			
			//If the difference in positions is approximately zero (Epsilon) do the following:
			if(Mathf.Abs (target.position.x - transform.position.x) < float.Epsilon)
				
				//If the y coordinate of the target's (player) position is greater than the y coordinate of this enemy's position set y direction 1 (to move up). If not, set it to -1 (to move down).
				yDir = target.position.y > transform.position.y ? 1 : -1;
			
			//If the difference in positions is not approximately zero (Epsilon) do the following:
			else
				//Check if target x position is greater than enemy's x position, if so set x direction to 1 (move right), if not set to -1 (move left).
				xDir = target.position.x > transform.position.x ? 1 : -1;
			
			//Call the AttemptMove function and pass in the generic parameter Player, because Enemy is moving and expecting to potentially encounter a Player
			//AttemptMove <Player> (xDir, yDir);
			var startPoint = new Point() { x = (int)transform.position.x, y = (int)transform.position.y };
			var endPoint = new Point () { x = (int)target.position.x, y = (int)target.position.y };

			var forrest = boardScript.forrest.Select ((go) => { 
				return new Point () { x = (int)go.transform.position.x, y = (int)go.transform.position.y };
			}).ToList ();

			forrest.Add (transformToPoint (boardScript.bank.transform));
			forrest.Add (transformToPoint (boardScript.barrels.transform));
			forrest.Add (transformToPoint (boardScript.wigwam.transform));
			forrest.Add (transformToPoint (boardScript.exit.transform));


			this.waypoints = aStar.calculatePath (forrest.ToArray(), startPoint, endPoint, MaxG);

			if (this.waypoints.Count > 0) {
				//zombieText.text = "Sherif Zombie: There's my Bob!";
			} else {
				//zombieText.text = "Sherif Zombie: On the lookout... ";
			}

			foreach (Transform child in boardScript.boardHolder)
			{
				var spriteRenderer = child.GetComponent<SpriteRenderer> ();
				// remove previous color
				if (spriteRenderer.color == Color.magenta) {
					spriteRenderer.color = Color.red;
				} else if (spriteRenderer.color == Color.cyan) {
					spriteRenderer.color = Color.white;
				}

				// if child is on the path
				bool onPath = waypoints.Any((p) => { return p.x == child.position.x && p.y == child.position.y; });
				if (onPath) {
					if (spriteRenderer.color == Color.red) {
						spriteRenderer.color = Color.magenta;
					} else {
						spriteRenderer.color = Color.cyan;
					}
				}
			}
		}

		private Point transformToPoint(Transform transform) {
			return  new Point () { x = (int)transform.position.x, y = (int)transform.position.y };
		}
		
		
		//OnCantMove is called if Enemy attempts to move into a space occupied by a Player, it overrides the OnCantMove function of MovingObject 
		//and takes a generic parameter T which we use to pass in the component we expect to encounter, in this case Player
		protected override void OnCantMove <T> (T component)
		{
			//Declare hitPlayer and set it to equal the encountered component.
			Player hitPlayer = component as Player;
			
			//Call the LoseFood function of hitPlayer passing it playerDamage, the amount of foodpoints to be subtracted.
			hitPlayer.LoseFood (playerDamage);
			
			//Set the attack trigger of animator to trigger Enemy attack animation.
			animator.SetTrigger ("enemyAttack");
			
			//Call the RandomizeSfx function of SoundManager passing in the two audio clips to choose randomly between.
			SoundManager.instance.RandomizeSfx (attackSound1, attackSound2);
		}
	}
}
