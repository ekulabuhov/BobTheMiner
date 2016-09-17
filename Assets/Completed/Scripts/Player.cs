using UnityEngine;
using System.Collections;

namespace Completed
{
	//Player inherits from MovingObject, our base class for objects that can move, Enemy also inherits from this.
	public class Player : MovingObject
	{
		//Start overrides the Start function of MovingObject
		protected override void Start ()
		{
			//Call the Start function of the MovingObject base class.
			base.Start ();
		}

        public void MovePlayer(int xDir, int yDir)
        {
            AttemptMove<Wall>(xDir, yDir);
        }
		
		//AttemptMove overrides the AttemptMove function in the base class MovingObject
		//AttemptMove takes a generic parameter T which for Player will be of the type Wall, it also takes integers for x and y direction to move in.
		protected override void AttemptMove <T> (int xDir, int yDir)
		{			
			//Call the AttemptMove method of the base class, passing in the component T (in this case Wall) and x and y direction to move.
			base.AttemptMove <T> (xDir, yDir);
			
			//Hit allows us to reference the result of the Linecast done in Move.
			RaycastHit2D hit;
			
			//If Move returns true, meaning Player was able to move into an empty space.
			Move (xDir, yDir, out hit);
		}
	}
}

