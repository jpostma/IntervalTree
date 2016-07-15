using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntervalTree
{
	public class IntervalTree<T>
	{
		class Range
		{
			public long Min { set; get; }
			public long Max { set; get; }
		}
		public class Node
		{
			public Range range { set; get; }
			public List<Node> Children { set; get; }
			public List<T> Value { set; get; }
			public Node()
			{
				Children = new List<Node>();
				Value = new List<T>();
			}
			public void Insert(long min, long max, T node)
			{
				Debug.Assert(min < max);

				Node leftNode = Children.FirstOrDefault(child => child.IsInside(min, max));
				if(leftNode != null)
				{
					leftNode.Insert(min, max, node);
				}
				else
				{
					List<Node> inRegion = Children.Where(child => child.IsOccluding(min, max)).ToList();
					if(inRegion.Count() != 0)
					{
						Children.Add(new Node() {
							Children = inRegion,
							range = new Range() { Min = min,Max = max},
							Value = new List<T>() { node }
						});
						foreach(Node region in inRegion)
							Children.Remove(region);
					}
					else
					{
						Node overlapped = Children.FirstOrDefault(child => child.IsOverlapped(min, max));
						if(overlapped != null)
						{
							overlapped.Insert(min, max, node);
						}
						else
						{
							Children.Add(new Node() {
								range = new Range() { Min = min, Max = max } ,
								Value = new List<T>() { node }
								}
							);
						}

					}
				}

				//if(IsOverlapped(.)
			}
			class Sorter : IEqualityComparer<Node>
			{
				public bool Equals(Node x, Node y)
				{
//if(x.IsInside(y.Min,y.Max) || x.Is
					return false;
				}
				public int GetHashCode(Node obj)
				{
					return obj.GetHashCode();
				}

			}
			public List<Node> Merge()
			{
				List<Node> mergedTree = new List<Node>();
				if (Children.Count() != 0)
				{
					foreach (Node child in Children)
						mergedTree.AddRange(child.Merge());
					mergedTree.Add(this);

					mergedTree = mergedTree.OrderBy(node => node.range.Min).ToList();
					List<Node> result = new List<Node>();
					result.Add(mergedTree.First());
					for (int i = 1; i < mergedTree.Count(); ++i)
					{

						if (mergedTree[i - 1].range.Max >= mergedTree[i].range.Min)
						{
							result.Last().range.Max = Math.Max(mergedTree[i].range.Max,mergedTree[i - 1].range.Max);
							result.Last().Value.AddRange(mergedTree[i].Value);
							continue;
						}
						result.Add(mergedTree[i]);
					}
					return result;
				}
				else
					return new List<Node>() { this };
				//mergedTree.Distinct(new Sorter());
			}
			public bool IsOverlapped(long min, long max)
			{
				if (min < range.Min && max >= range.Min)
					return true;
				if (min <= range.Max && max > range.Max)
					return true;
				return false;
			}
			public bool IsInside(long min, long max)
			{
				return min >= range.Min && max <= range.Max;
			}
			public bool IsOccluding(long min, long max)
			{
				if (min <= range.Min && max > range.Max)
					return true;
				if (min < range.Min && max >= range.Max)
					return true;
				return false;
			}
		}
		Node RootNodes = new Node() { range = new Range() { Min = long.MinValue, Max = long.MaxValue } };

		public void Insert(long min, long max, T node)
		{
			Debug.Assert(RootNodes.IsInside(min, max));
			RootNodes.Insert(min, max, node);
		}

		public List<Node> Merge()
		{
			return RootNodes.Merge();
		}
	}

}