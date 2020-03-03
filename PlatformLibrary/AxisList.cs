using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformLibrary
{
    class EndPoint
    {
        public Box Box;
        public bool IsStart;
        public float Value;
    }

    class Box
    {
        public EndPoint Start;
        public EndPoint End;
        public IBoundable GameObject;
    }

    public class AxisList
    {
        /// <summary>
        /// The tracked game objects
        /// </summary>
        Dictionary<IBoundable, Box> boxes = new Dictionary<IBoundable, Box>();

        /// <summary>
        /// The axis list
        /// </summary>
        List<EndPoint> endPoints = new List<EndPoint>();

        /// <summary>
        /// Adds a game object to those being tracked by the 
        /// axis list
        /// </summary>
        /// <param name="gameObject">The game object to track</param>
        public void AddGameObject(IBoundable gameObject)
        {
            var box = new Box()
            {
                GameObject = gameObject
            };
            EndPoint start = new EndPoint()
            {
                Box = box,
                IsStart = true,
                Value = gameObject.Bounds.X
            };
            box.Start = start;
            EndPoint end = new EndPoint()
            {
                Box = box,
                IsStart = false,
                Value = gameObject.Bounds.X + gameObject.Bounds.Width
            };
            box.End = end;
            boxes.Add(gameObject, box);
            endPoints.Add(start);
            endPoints.Add(end);
            Sort();
        }

        /// <summary>
        /// Updates the provided game object's position in the axis list
        /// </summary>
        /// <param name="gameObject">The updated game object</param>
        public void UpdateGameObject(IBoundable gameObject)
        {
            var box = boxes[gameObject];
            box.Start.Value = gameObject.Bounds.X;
            box.End.Value = gameObject.Bounds.X + gameObject.Bounds.Width;
            Sort();
        }

        /// <summary>
        /// Sorts the endpoints array using insertion sort.
        /// This has good performance if the array is nearly sorted
        /// </summary>
        void Sort()
        {
            int i = 1;
            while (i < endPoints.Count)
            {
                int j = i;
                while (j > 0 && endPoints[j - 1].Value > endPoints[j].Value)
                {
                    // swap [j-1] and [j]
                    var tmp = endPoints[j - 1];
                    endPoints[j - 1] = endPoints[j];
                    endPoints[j] = tmp;
                }
                i++;
            }
        }

        public IEnumerable<IBoundable> QueryRange(float start, float end)
        {
            List<IBoundable> open = new List<IBoundable>();
            foreach(var point in endPoints)
            {
                // Stop building the list once we 
                // reach the first endpoint past the 
                // query region
                if (point.Value > end) break;

                // For each start we encounter, add IBoundable
                // to the open list
                if(point.IsStart)
                {
                    open.Add(point.Box.GameObject);
                }

                // For each end we encounter before the start,
                // remove the IBoundable from the open list
                else if(point.Value < start)
                {
                    open.Remove(point.Box.GameObject);
                }
            }

            // Return the open list 
            return open;
        }

        /// <summary>
        /// Gets an enumeration of potentially colliding IBoundables
        /// </summary>
        /// <returns>An enumeration of IBoundable pairs</returns>
        public IEnumerable<Tuple<IBoundable, IBoundable>> GetCollisionPairs()
        {
            List<IBoundable> open = new List<IBoundable>();
            List<Tuple<IBoundable, IBoundable>> pairs = new List<Tuple<IBoundable, IBoundable>>();
            foreach(EndPoint point in endPoints)
            {
                if (point.IsStart)
                {
                    // Create a collision pair between this IBoundable 
                    // and all IBoundables in the open list
                    foreach(var other in open)
                    {
                        pairs.Add(new Tuple<IBoundable, IBoundable>(point.Box.GameObject, other));
                    }
                    // Add this endpoint's IBoundable to the open list 
                    open.Add(point.Box.GameObject);
                }
                else
                {
                    // remove the corresponding IBoundable from the open list
                    open.Remove(point.Box.GameObject);
                }
            }
            return pairs;
        }
    }
}
