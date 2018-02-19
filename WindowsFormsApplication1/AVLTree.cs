using System;
using System.Windows.Forms;

namespace NASP
{
  public class AVLTree
  {
    private Node _root;
    public AVLTree()
    {
      _root = null;
    }

    /// <summary>
    /// Add node to tree
    /// </summary>
    /// <param name="value"></param>
    public void Add(int value)
    {
      Node freeNode = null;
      var tempNode = _root;

      // if tree is empty
      if (_root == null)
      {
        _root = new Node(value);
      }

      else
      {
        // find empty space for node
        while (tempNode != null)
        {
          freeNode = tempNode;
          tempNode = value < tempNode.Value ? tempNode.LeftChild : tempNode.RightChild;
        }

        // insert node in free space
        if (value < freeNode.Value)
        {
          freeNode.LeftChild = new Node(value) { Parent = freeNode };
          // refresh balance faxctors
          ResetBalanceFactorAfterAdding(freeNode.LeftChild);

        }
        else
        {
          freeNode.RightChild = new Node(value) { Parent = freeNode };
          // podesi faktore ravnoteže i uravnoteži stablo
          ResetBalanceFactorAfterAdding(freeNode.RightChild);
        }
      }
    }

    #region tree balance

    private void Balance(Node currentNode)
    {
      if (currentNode.Parent == null)
        return;
      if (Math.Abs(currentNode.Parent.BalanceFactor) == 2)
      {
        if (currentNode.Parent.BalanceFactor == -2)
        {
          if (currentNode.BalanceFactor == -1)
            RightRotation(currentNode.Parent);
          else
            LeftRightRotation(currentNode);
        }
        else
        {
          if (currentNode.BalanceFactor == 1)
            LeftRotation(currentNode.Parent);
          else
            RightLeftRotation(currentNode);
        }
      }
    }

    private void BalanceRemoval(Node currentNode)
    {
      if (currentNode == null)
        return;
      if (Math.Abs(currentNode.BalanceFactor) == 2)
      {
        if (currentNode.BalanceFactor == -2)
        {
          if (currentNode.LeftChild != null)
          {
            if (currentNode.LeftChild.BalanceFactor == -1 ||
                currentNode.LeftChild.BalanceFactor == 0)
            {
              RightRotation(currentNode);
              return;
            }

            if (currentNode.LeftChild.BalanceFactor == 1)
            {
              LeftRightRotation(currentNode.LeftChild);
              return;
            }
          }
          if (currentNode.RightChild != null)
          {
            if (currentNode.RightChild.BalanceFactor == -1 ||
                currentNode.RightChild.BalanceFactor == 0)
            {
              RightRotation(currentNode);
            }
            else if (currentNode.RightChild.BalanceFactor == 1)
            {
              LeftRightRotation(currentNode.RightChild);
            }
          }
        }

        else if (currentNode.BalanceFactor == 2)
        {
          if (currentNode.LeftChild != null)
          {
            if (currentNode.LeftChild.BalanceFactor == -1
                || currentNode.LeftChild.BalanceFactor == 0)
            {
              LeftRotation(currentNode);
              return;
            }
            if (currentNode.LeftChild.BalanceFactor == 1)
            {
              RightLeftRotation(currentNode.LeftChild);
              return;
            }
          }
          if (currentNode.RightChild != null)
          {
            if (currentNode.RightChild.BalanceFactor == -1
                || currentNode.RightChild.BalanceFactor == 0)
            {
              RightLeftRotation(currentNode.RightChild);
            }
            else if (currentNode.RightChild.BalanceFactor == 1)
            {
              LeftRotation(currentNode);
            }
          }
        }
      }
    }

    #endregion


    #region Rotations
    /// <summary>
    /// Right rotation, replace node in clockwise direction.
    /// </summary>
    /// <param name="currentNode"></param>
    private void RightRotation(Node currentNode)
    {
      if (currentNode.LeftChild == null)
        throw new Exception("Right rotation: error");

      var childNode = currentNode.LeftChild;
      currentNode.LeftChild = childNode.RightChild;

      if (childNode.RightChild != null)
      {
        childNode.RightChild.Parent = currentNode;
      }

      childNode.Parent = currentNode.Parent;

      if (currentNode.Parent == null)
      {
        _root = childNode;
      }
      else if (currentNode == currentNode.Parent.RightChild)
      {
        currentNode.Parent.RightChild = childNode;
      }
      else
      {
        currentNode.Parent.LeftChild = childNode;
      }

      childNode.RightChild = currentNode;
      currentNode.Parent = childNode;

      RestoreBalanceFactor(currentNode);
    }

    /// <summary>
    /// Left rotation, replace nodes in opposite  clockwork.
    /// </summary>
    /// <param name="currentNode"></param>
    private void LeftRotation(Node currentNode)
    {
      if (currentNode.RightChild == null)
        throw new Exception("Left rotation: error");

      var childNode = currentNode.RightChild;
      currentNode.RightChild = childNode.LeftChild;

      if (childNode.LeftChild != null)
        childNode.LeftChild.Parent = currentNode;

      childNode.Parent = currentNode.Parent;

      if (currentNode.Parent == null)
        _root = childNode;
      else if (currentNode == currentNode.Parent.LeftChild)
        currentNode.Parent.LeftChild = childNode;
      else
        currentNode.Parent.RightChild = childNode;

      childNode.LeftChild = currentNode;
      currentNode.Parent = childNode;

      RestoreBalanceFactor(currentNode);
    }

    private void RightLeftRotation(Node currentNode)
    {
      RightRotation(currentNode);
      LeftRotation(currentNode.Parent.Parent);
    }

    private void LeftRightRotation(Node trenutniCvor)
    {
      LeftRotation(trenutniCvor);
      RightRotation(trenutniCvor.Parent.Parent);
    }

    #endregion

    #region restoration of balance factor
    /// <summary>
    /// Restoration of balance factor
    /// </summary>
    /// <param name="node"></param>
    private void RestoreBalanceFactor(Node node)
    {
      var leftHeight = 0;
      var rightHeight = 0;

      if (node.LeftChild != null)
      {
        leftHeight = node.LeftChild.Height + 1;
      }

      if (node.RightChild != null)
      {
        rightHeight = node.RightChild.Height + 1;
      }

      node.Height = Math.Max(rightHeight, leftHeight);
      node.BalanceFactor = rightHeight - leftHeight;

      node = node.Parent;
      if (node == null) { return; }

      leftHeight = 0;
      rightHeight = 0;

      if (node.LeftChild != null)
      {
        leftHeight = node.LeftChild.Height + 1;
      }

      if (node.RightChild != null)
      {
        rightHeight = node.RightChild.Height + 1;
      }

      node.Height = Math.Max(rightHeight, leftHeight);
      node.BalanceFactor = rightHeight - leftHeight;

    }



    /// <summary>
    /// Resets balance factors after adding a new node.
    // It simultaneously restores the balance factor of the current node and its parent.
    // In case the tree is out of balance, it balances the tree.
    /// </summary>
    /// <param name="currentNode"></param>
    private void ResetBalanceFactorAfterAdding(Node currentNode)
    {
      if (currentNode == null)
      {
        return;
      }

      currentNode.Height = 0;

      while (currentNode.Parent != null)
      {
        var leftHeightParents = 0;
        var rightHeightParents = 0;

        if (currentNode.Parent.LeftChild != null)
        {
          leftHeightParents = currentNode.Parent.LeftChild.Height + 1;
        }

        if (currentNode.Parent.RightChild != null)
        {
          rightHeightParents = currentNode.Parent.RightChild.Height + 1;
        }

        currentNode.Parent.Height = Math.Max(leftHeightParents, rightHeightParents);

        var leftHeight = 0;
        var rightHeight = 0;
        if (currentNode.LeftChild != null)
        {
          leftHeight = currentNode.LeftChild.Height + 1;
        }
        if (currentNode.RightChild != null)
        {
          rightHeight = currentNode.RightChild.Height + 1;
        }
        currentNode.Height = Math.Max(leftHeight, rightHeight);
        currentNode.BalanceFactor = rightHeight - leftHeight;
        currentNode.Parent.BalanceFactor = rightHeightParents - leftHeightParents;

        Balance(currentNode);

        if (rightHeightParents == leftHeightParents)
        {
          break;
        }

        currentNode = currentNode.Parent;
        if (currentNode == null)
        {
          break;
        }
      }
    }

    /// <summary>
    /// Restores balance factors after deleting node.
    /// </summary>
    /// <param name="currentNode"></param>
    private void ResetBalanceAfterDeletion(Node currentNode)
    {
      if (currentNode == null)
      {
        return;
      }

      currentNode.Height = 0;
      while (currentNode != null)
      {
        var leftHeight = 0;
        var rightHeight = 0;

        if (currentNode.LeftChild != null)
        {
          leftHeight = currentNode.LeftChild.Height + 1;
        }
        if (currentNode.RightChild != null)
        {
          rightHeight = currentNode.RightChild.Height + 1;
        }

        currentNode.Height = Math.Max(leftHeight, rightHeight);
        currentNode.BalanceFactor = rightHeight - leftHeight;

        if (Math.Abs(currentNode.BalanceFactor) == 1)
        {
          break;
        }
        if (Math.Abs(currentNode.BalanceFactor) == 2)
        {
          BalanceRemoval(currentNode);
          if (Math.Abs(currentNode.BalanceFactor) == 1)
          {
            break;
          }
        }

        currentNode = currentNode.Parent;
      }
    }
    #endregion

    /// <summary>
    /// Clears node from tree with set value.
    /// </summary>
    /// <param name="value"></param>
    public void RemovesNode(int value)
    {
      var removedNode = FindNode(value);
      if (removedNode == null)
      {
        return;
      }

      // if has no children, it is last node
      if (removedNode.LeftChild == null && removedNode.RightChild == null)
      {
        var deletedNodeParent = removedNode.Parent;
        // remove node
        ReplaceChildParent(removedNode.Parent, removedNode, null);
        ResetBalanceAfterDeletion(deletedNodeParent);
      }

      // only one child
      //  ^ == EXOR
      else if (removedNode.LeftChild == null ^ removedNode.RightChild == null)
      {
        var roditeljBrisanog = removedNode.Parent;
        if (removedNode.LeftChild != null)
        {
          // remove node
          ReplaceChildParent(removedNode.Parent, removedNode, removedNode.LeftChild);
          ResetBalanceAfterDeletion(roditeljBrisanog);
        }
        else
        {
          // remove node
          ReplaceChildParent(removedNode.Parent, removedNode, removedNode.RightChild);
          ResetBalanceAfterDeletion(roditeljBrisanog);
        }
      }

      // if has two children
      else
      {
        // pronađi najveći manji od brisanog čvora
        var biggestLess = removedNode.LeftChild;
        while (biggestLess.RightChild != null)
        {
          biggestLess = biggestLess.RightChild;
        }

        // copy data
        removedNode.Value = biggestLess.Value;

        // move the left child to the smallest to the right place
        removedNode.LeftChild.RightChild = biggestLess.LeftChild;
        if (removedNode.LeftChild.RightChild != null)
        {
          removedNode.LeftChild.RightChild.Parent = removedNode.LeftChild;
        }
        // remove node of biggest
        var biggestLessParent = biggestLess.Parent;
        ReplaceChildParent(biggestLess.Parent, biggestLess, null);

        ResetBalanceAfterDeletion(biggestLessParent);
      }

    }

    #region helper functions
    /// <summary>
    /// Look for a node inside the tree that contains the set value.
    /// </summary>
    /// <param name="value"></param>
    /// <returns>Returns first found node.</returns>
    private Node FindNode(int value)
    {
      var tempNode = _root;
      while (tempNode != null)
      {
        if (value < tempNode.Value)
        {
          tempNode = tempNode.LeftChild;
        }
        else if (value > tempNode.Value)
        {
          tempNode = tempNode.RightChild;
        }
        else if (value == tempNode.Value)
        {
          return tempNode;
        }
      }
      return null;
    }

    /// <summary>
    /// Replace node with new node
    /// </summary>
    /// <param name="parrent">Node above currentNode</param>
    /// <param name="currentNode">Node for replacing with new node.</param>
    /// <param name="newNode">New node to insert of place of currentNode</param>
    private void ReplaceChildParent(Node parrent, Node currentNode, Node newNode)
    {
      if (parrent == null)
      {
        if (_root == currentNode)
        {
          _root = newNode;
          if (newNode != null)
          {
            _root.Parent = null;
          }
        }
        return;
      }

      if (parrent.LeftChild == currentNode)
      {
        parrent.LeftChild = newNode;
        if (newNode != null)
        {
          parrent.LeftChild.Parent = parrent;
        }
      }
      else
      {
        parrent.RightChild = newNode;
        if (newNode != null)
        {
          parrent.RightChild.Parent = parrent;
        }
      }

    }

    /// <summary>
    /// Navigates the tree and copies the nodes from the AVL tree to the tree to display in the graphical interface.
    /// </summary>
    /// <returns></returns>
    public TreeNode GetNodes()
    {
      if (_root == null)
      {
        return new TreeNode();
      }
      var cvor = new TreeNode("Node value: " + _root.Value +
                          " Balance factor: " + _root.BalanceFactor);
      if (_root.RightChild != null)
      {
        cvor.Nodes.Add(_root.RightChild.GetNodes());
      }
      else
      {
        cvor.Nodes.Add("*empty*");
      }

      if (_root.LeftChild != null)
      {
        cvor.Nodes.Add(_root.LeftChild.GetNodes());
      }
      else
      {
        cvor.Nodes.Add("*empty*");
      }

      return cvor;
    }

    #endregion

  }
}
