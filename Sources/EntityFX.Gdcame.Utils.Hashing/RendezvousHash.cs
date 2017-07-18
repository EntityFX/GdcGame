using System;
using System.Collections.Generic;
using System.Linq;

namespace EntityFX.Gdcame.Utils.Hashing
{
    class RendezvousHash
    {
        private IHashFunction hasher;

        public RendezvousHash(IHashFunction hasher)
        {
            this.hasher = hasher;
        }

        private long ComputeScore(Node node, String userKey)
        {
            long hash = hasher.GetHash(node.ServerNumber + "." + userKey);
            return hash;
        }

        /**
	     * Removes a node from the nodes list.
	     * 
	     * @return true if the node was in the list ?
	     */
        public bool Remove(IList<Node> nodes, Node node)
        {
            return nodes.Remove(node);
        }
        public bool Remove(List<string> nodes, Node node)
        {
            return Remove(GetNodesList(nodes), node);
        }

        /**
         * Add a new node to the nodes list
         * 
         * @return true if node did not previously exist in the list ?
         */
        public void Add(IList<Node> nodes, Node node)
        {
            nodes.Add(node);
        }

        public void Add(IList<string> nodes, Node node)
        {
            Add(GetNodesList(nodes), node);
        }

        /**
	    * Return a node for a given key
	    */
        public Node DetermineResponsibleNode(String userKey, IEnumerable<Node> nodes)
        {
            long maxValue = Int64.MinValue;//=Long.MinValue?
            Node max = null;
            foreach (Node node in nodes)
            {
                long nodesHash = ComputeScore(node, userKey);
                if (nodesHash > maxValue)
                {
                    max = node;
                    maxValue = nodesHash;
                }
            }
            return max;
        }

        public Node DetermineResponsibleNode(String userKey, List<string> nodes)
        {
            return DetermineResponsibleNode(userKey, GetNodesList(nodes));
        }

        private IList<Node> GetNodesList(IEnumerable<string> nodes)
        {
            return nodes.Select((n, i) => new Node(n, i)).ToList();
        }

    }
}
