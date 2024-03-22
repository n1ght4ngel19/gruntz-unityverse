using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GruntzUnityverse.Pathfinding {
/// <summary>
/// Pathfinder class incorporating multiple pathfinding algorithms (possibly).
/// </summary>
public static class Pathfinder {
	/// <summary>
	/// A* search algorithm.
	/// </summary>
	/// <param name="start">The Node to start the search from.</param>
	/// <param name="end">The Node to end the search at.</param>
	/// <param name="nodes">All nodes in the given context.</param>
	/// <param name="fly">Whether the search is for flying actors.</param>
	/// <returns>A list of nodes representing the path from start to end.</returns>
	public static List<Node> AstarSearch(Node start, Node end, HashSet<Node> nodes, bool fly) {
		if (end == start) {
			return new List<Node>();
		}

		// When the selected end node is not walkable, find a new end node if possible
		if (!end.walkable) {
			// When the target is right beside the start, return an empty list
			if (start.neighbours.Contains(end)) {
				return new List<Node>();
			}

			if (end.freeNeighbours.Count == 0) {
				return new List<Node>();
			}

			end = end.freeNeighbours.OrderBy(n => CalculateHeuristic(start, n)).First();
		}

		// Use a PriorityQueue to keep track of the nodes to check, since we need to sort Nodes in it anyway by their F cost,
		// and a PriorityQueue does this automatically
		PriorityQueue<Node, int> openSet = new PriorityQueue<Node, int>();

		// Use a HashSet to keep track of the nodes that have already been checked,
		// since we don't need to sort them, but need to check for duplicates
		HashSet<Vector2> closedSet = new HashSet<Vector2>();
		int counter = 0;

		openSet.Enqueue(start, start.f);

		foreach (Node node in nodes) {
			node.g = int.MaxValue;
			node.parent = null;
		}

		start.g = 0;
		start.h = CalculateHeuristic(start, end);

		while (openSet.Count > 0) {
			Node current = openSet.Dequeue();
			counter++;

			if (current == end) {
				Debug.Log($"Checked {counter} nodes");

				return RetracePath(start, end);
			}

			closedSet.Add(current.location2D);

			foreach (Node neighbour in current.neighbours) {
				if (closedSet.Contains(neighbour.location2D)) {
					continue;
				}

				if ((!neighbour.walkable && !fly)) {
					closedSet.Add(neighbour.location2D);

					continue;
				}

				int tentativeG = current.g + CalculateHeuristic(current, neighbour);

				if (tentativeG < neighbour.g) {
					neighbour.g = tentativeG;
					neighbour.h = CalculateHeuristic(neighbour, end);
					neighbour.parent = current;
				}

				// See if PriorityQueue contains neighbour, and only enqueue it if it doesn't
				if (openSet.UnorderedItems.ToList().Select(tuple => tuple.Item1).Contains(neighbour)) {
					continue;
				}

				openSet.Enqueue(neighbour, neighbour.f);
			}
		}

		Debug.Log($"No path found, checked {counter} nodes");

		return new List<Node>();
	}

	public enum Heuristic {
		Manhattan,
		Chebyshev,
		Euclidean,
		Octile,
	}

	private static int CalculateHeuristic(Node start, Node end, Heuristic heuristic = Heuristic.Manhattan) {
		return heuristic switch {
			Heuristic.Euclidean => Euclidean(start, end),
			Heuristic.Chebyshev => Chebyshev(start, end),
			Heuristic.Octile => Octile(start, end),
			_ => Manhattan(start, end),
		};
	}

	private static int Manhattan(Node start, Node end) {
		return
			Math.Abs(start.location2D.x - end.location2D.x)
			+ Math.Abs(start.location2D.y - end.location2D.y);
	}

	private static int Chebyshev(Node start, Node end) {
		return Math.Max(
			Math.Abs(start.location2D.x - end.location2D.x),
			Math.Abs(start.location2D.y - end.location2D.y)
		);
	}

	private static int Euclidean(Node start, Node end) {
		int dxE = Math.Abs(start.location2D.x - end.location2D.x);
		int dyE = Math.Abs(start.location2D.y - end.location2D.y);

		int euclideanDistance = (int)Math.Sqrt(dxE * dxE + dyE * dyE);

		return euclideanDistance;
	}

	private static int Octile(Node start, Node end) {
		int dX = Math.Abs(start.location2D.x - end.location2D.x);
		int dY = Math.Abs(start.location2D.y - end.location2D.y);

		return dX + dY + (int)((Math.Sqrt(2) - 2) * Math.Min(dX, dY));
	}

	/// <summary>
	/// Retraces the path from start to end, reversing the initial path produced by the A* algorithm.
	/// </summary>
	/// <param name="start">The start of the path.</param>
	/// <param name="end">The end of the path.</param>
	/// <returns>A list of nodes representing the path from start to end.</returns>
	private static List<Node> RetracePath(Node start, Node end) {
		List<Node> path = new List<Node>();
		Node current = end;

		while (current != start) {
			path.Add(current);
			current = current.parent;
		}

		path.Reverse();

		return path;
	}
}
}
