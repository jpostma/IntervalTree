using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntervalTree
{
	class Program
	{
		static void Main(string[] args)
		{
			IntervalTree<string> tree = new IntervalTree<string>();

			tree.Insert(1, 2, "1-2");
			tree.Insert(1, 10, "1-10");
			tree.Insert(-5, 5, "1-10");

			tree.Insert(200, 205, "200-205");
			tree.Insert(199, 204, "199-204");

			tree.Merge();
		}
	}
}
