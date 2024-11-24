using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Core;
using GruntzUnityverse.Utils.Extensionz;
using UnityEngine;

namespace GruntzUnityverse.Pathfinding {
/// <summary>
/// Custom representation of a node used for pathfinding.
/// </summary>
public class Node : MonoBehaviour {
	public GameManager gameManager;

	/// <summary>
	/// The location of this Node in 2D space.
	/// </summary>
	public Vector2Int location2D;

	/// <summary>
	/// The collider used for checking interactions with this Node.
	/// </summary>
	public CircleCollider2D circleCollider2D;

	public Grunt grunt => gameManager.gruntz.FirstOrDefault(gr => Vector2Int.RoundToInt(gr.transform.position) == location2D);

	// --------------------------------------------------
	// Pathfinding
	// --------------------------------------------------

	#region Pathfinding
	/// <summary>
	/// The parent Node of this Node, used for retracing paths.
	/// </summary>
	[Header("Pathfinding")]
	public Node parent;

	/// <summary>
	/// Cost from start to this node.
	/// </summary>
	public int g;

	/// <summary>
	/// Heuristic (cost from this Node to end).
	/// </summary>
	public int h;

	/// <summary>
	/// Total cost (g + h).
	/// </summary>
	public int f => g + h;

	/// <summary>
	/// The neighbours of this Node.
	/// </summary>  
	public List<Node> neighbours;

	/// <summary>
	/// The diagonal neighbours of this Node.
	/// </summary>
	public List<Node> diagonalNeighbours;

	/// <summary>
	/// The orthogonal neighbours of this Node.
	/// </summary>
	public List<Node> orthogonalNeighbours;

	/// <summary>
	/// The free neighbours of this Node.
	/// </summary>
	public List<Node> freeNeighbours => neighbours.Where(n => n.walkable).ToList();

	/// <summary>
	/// Directional representation of the neighbours of this Node.
	/// </summary>
	public NeighbourSet neighbourSet;
	#endregion

	// --------------------------------------------------
	// Flagz
	// --------------------------------------------------

	#region Flagz
	/// <summary>
	/// If true, this Node is occupied by an actor.
	/// </summary>
	[Header("Flagz")]
	public bool occupied;

	/// <summary>
	/// If true, this Node is reserved by an actor for its next move.
	/// </summary>
	public Grunt reservedBy => FindObjectsByType<Grunt>(FindObjectsSortMode.None).FirstOrDefault(grunt => grunt.next == this);

	/// <summary>
	/// If true, this Node is blocked by a wall or other obstacle.
	/// </summary>
	public bool isBlocked;

	public bool isWater;
	public bool isFire;
	public bool isVoid;

	/// <summary>
	/// If true, this Node can be walked upon safely.
	/// </summary>
	public bool walkable => !occupied && !isBlocked && !isWater && !isFire && !isVoid;

	/// <summary>
	/// If true, this Node can be swam on with a Toob equipped.
	/// </summary>
	public bool toobSwimmable => !occupied && !isBlocked && !isFire && !isVoid && !reservedBy;

	/// <summary>
	/// If true, this Node can be flown over with Wingz equipped.
	/// </summary>
	public bool wingzFlyable => !occupied && !reservedBy;

	/// <summary>
	/// If true, this Node is a hard corner, which means that this Node prevents diagonal movement to its neighbours.
	/// </summary>
	public bool hardCorner;
	#endregion

	public TileType tileType;

	/// <summary>
	/// Sets up this Node with the given parameters.
	/// </summary>
	/// <param name="position">The position of this Node in the grid. </param>
	/// <param name="tileName">The name of the tile this Node is based on.</param>
	/// <param name="nodes">The list of all nodes in the grid.</param>
	public void SetupNode(Vector3Int position, string tileName) {
		transform.position = position - Vector3Int.one / 2;
		location2D = new Vector2Int(position.x, position.y);
		circleCollider2D = GetComponent<CircleCollider2D>();

		AssignTileType(tileName);
	}

	/// <summary>
	/// Assigns the tile type of this Node based on the given tile name.
	/// </summary>
	/// <param name="tileName"></param>
	private void AssignTileType(string tileName) {
		if (tileName.Contains("Ground")) {
			tileType = TileType.Ground;
		} else if (tileName.Contains("Collision")) {
			tileType = TileType.Collision;
			isBlocked = true;
		} else if (tileName.Contains("Water")) {
			tileType = TileType.Water;
			isWater = true;
		} else if (tileName.Contains("Fire")) {
			tileType = TileType.Fire;
			isFire = true;
		} else if (tileName.Contains("Void")) {
			tileType = TileType.Void;
			isVoid = true;
		} else {
			tileType = TileType.Unknown;
		}
	}

	/// <summary>
	/// Returns whether this Node can be reached diagonally from the given Node.
	/// </summary>
	/// <param name="toCheck"></param>
	/// <returns></returns>
	public bool CanReachDiagonally(Node toCheck) {
		return !diagonalNeighbours.Contains(toCheck)
			|| !neighbours.Any(n => n.hardCorner && toCheck.neighbours.Contains(n));
	}

	/// <summary>
	/// Assigns the neighbours of this Node based from the given list of nodes.
	/// </summary>
	/// <param name="nodes"></param>
	public void AssignNeighbours(HashSet<Node> nodes) {
		neighbourSet = new NeighbourSet {
			up = nodes.FirstOrDefault(n => n.location2D.Equals(location2D.Up())),
			upRight = nodes.FirstOrDefault(n => n.location2D.Equals(location2D.UpRight())),
			right = nodes.FirstOrDefault(n => n.location2D.Equals(location2D.Right())),
			downRight = nodes.FirstOrDefault(n => n.location2D.Equals(location2D.DownRight())),
			down = nodes.FirstOrDefault(n => n.location2D.Equals(location2D.Down())),
			downLeft = nodes.FirstOrDefault(n => n.location2D.Equals(location2D.DownLeft())),
			left = nodes.FirstOrDefault(n => n.location2D.Equals(location2D.Left())),
			upLeft = nodes.FirstOrDefault(n => n.location2D.Equals(location2D.UpLeft())),
		};

		neighbours = neighbourSet.AsList();

		diagonalNeighbours = neighbourSet.AsList().Where(n => n.location2D.x != location2D.x && n.location2D.y != location2D.y).ToList();

		orthogonalNeighbours = neighbourSet.AsList().Except(diagonalNeighbours).ToList();
	}

	private void Start() {
		gameManager = FindFirstObjectByType<GameManager>();
	}

	private async void OnTriggerEnter2D(Collider2D other) {
		if (!circleCollider2D.isTrigger) {
			return;
		}

		if (other.TryGetComponent(out Grunt grunt)) {
			if (this.grunt != null && this.grunt != grunt && !this.grunt.between) {
				this.grunt.Die(AnimationManager.instance.squashDeathAnimation);
			}

			grunt.node = this;
			grunt.transform.position = transform.position;
			grunt.spriteRenderer.sortingOrder = 10;

			if (isBlocked && !grunt.canFly) {
				grunt.Die(AnimationManager.instance.explodeDeathAnimation, false, false);

				return;
			}

			if (isFire && !grunt.canFly) {
				grunt.Die(AnimationManager.instance.burnDeathAnimation, false, false);

				return;
			}

			if (isWater && !grunt.canFly) {
				grunt.Die(AnimationManager.instance.sinkDeathAnimation, false, false);

				return;
			}

			if (isVoid && !grunt.canFly) {
				grunt.Die(AnimationManager.instance.fallDeathAnimation, false, false);

				return;
			}

			grunt.between = false;
			occupied = true;

			if (grunt.stateHandler.goToState is StateHandler.State.Playing or StateHandler.State.Committed) {
				return;
			}

			if (grunt.interactionTarget != null) {
				grunt.GoToState(StateHandler.State.Interacting);
			} else if (grunt.attackTarget != null) {
				grunt.GoToState(StateHandler.State.Attacking);
			} else if (grunt.giveTarget != null) {
				grunt.GoToState(StateHandler.State.Giving);
			} else if (grunt.travelGoal != null) {
				grunt.GoToState(StateHandler.State.Walking);
			} else {
				grunt.GoToState(StateHandler.State.Idle);
			}
		}

		if (other.TryGetComponent(out RollingBall ball)) {
			ball.node = this;
			ball.transform.position = transform.position;
			ball.next = neighbourSet.NeighbourInDirection(ball.direction);

			switch (tileType) {
				case TileType.Water:
					// Landingz
					if (!isWater) {
						break;
					}

					ball.moveSpeed *= 5;
					await ball.animancer.Play(ball.sinkAnim);

					Destroy(ball.gameObject);

					return;
				case TileType.Collision:
					ball.moveSpeed *= 5;
					await ball.animancer.Play(ball.breakAnim);

					ball.enabled = false;

					ball.GetComponent<SpriteRenderer>().sortingLayerName = "AlwaysBottom";
					ball.GetComponent<SpriteRenderer>().sortingOrder = 6;

					return;
				case TileType.Void:
					ball.enabled = false;
					await ball.animancer.Play(ball.sinkAnim);

					Destroy(ball.gameObject);

					return;
			}

			if (isBlocked) {
				ball.moveSpeed *= 5;
				await ball.animancer.Play(ball.breakAnim);

				ball.enabled = false;

				ball.GetComponent<SpriteRenderer>().sortingLayerName = "AlwaysBottom";
				ball.GetComponent<SpriteRenderer>().sortingOrder = 6;

				return;
			}

			if (this.grunt != null && !this.grunt.between) {
				this.grunt.Die(AnimationManager.instance.squashDeathAnimation);
			}
		}
	}

	private void OnTriggerExit2D(Collider2D other) {
		if (other.TryGetComponent(out Grunt grunt)) {
			grunt.between = true;
			occupied = false;
			grunt.spriteRenderer.sortingOrder = 12;
		}
	}
}

}
