using System;
using System.Collections.Generic;

namespace Mulder.Base.Domain
{
	public class StaticFile : IEquatable<StaticFile>
	{
		readonly Item item;
		readonly Stack<string> snapShots;
		readonly Queue<string> filterNameQueue;
		
		public Item Item { get { return item; } }
		public IEnumerable<string> SnapShots { get { return snapShots; } }
		public IEnumerable<string> FilterNameQueue { get { return filterNameQueue; } }
		
		public string Path { get; set; }
		public Layout Layout { get; set; }
		
		public StaticFile(Item item)
		{
			this.item = item;
			
			snapShots = new Stack<string>(new [] { item.Content });

			filterNameQueue = new Queue<string>();
		}
		
		public void AddFilter(string filterName)
		{
			filterNameQueue.Enqueue(filterName);
		}

		public string GetNextFilterName()
		{
			return filterNameQueue.Dequeue();
		}
		
		public string GetLastSnapShot()
		{
			return snapShots.Peek();
		}
		
		public void CreateSnapShot(string content)
		{
			snapShots.Push(content);
		}
		
		public override int GetHashCode()
		{
			int hash = 17;
			hash = hash * 23 + item.GetHashCode();
			hash = hash * 23 + snapShots.GetHashCode();
			hash = hash * 23 + filterNameQueue.GetHashCode();
			return hash;
		}
		
		public override bool Equals(object obj)
		{
			if (obj is StaticFile) return Equals((StaticFile)obj);
			
			return false;
		}
		
		public bool Equals(StaticFile other)
		{
			if (other == null)
				return false;
			
			return item == other.item
				&& snapShots == other.snapShots
				&& filterNameQueue == other.filterNameQueue;
		}
	}
}
