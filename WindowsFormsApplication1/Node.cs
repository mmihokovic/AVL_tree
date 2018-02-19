using System.Windows.Forms;

namespace NASP
{
    public class Node
    {
      public Node(int value)
        {
            Value = value;
            BalanceFactor = 0;
            Height = 0;
        }

      public Node Parent { get; set; }
      public Node RightChild { get; set; }
      public Node LeftChild { get; set; }
      public int BalanceFactor { get; set; }
      public int Value { get; set; }
      public int Height { get; set; }


      public TreeNode GetNodes()
        {
            var node = new TreeNode("Node value: " + Value +
                                " Balnce factor: " + BalanceFactor);
            if (RightChild != null)
            {
                node.Nodes.Add(RightChild.GetNodes());
            }
            else
            {
                node.Nodes.Add("*empty*");
            }

            if (LeftChild != null)
            {
                node.Nodes.Add(LeftChild.GetNodes());
            }
            else
            {
                node.Nodes.Add("*empty*");
            }

            return node;
        }
    

           

    }
}
