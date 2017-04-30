using System;
using System.Collections.Generic;

namespace EntityFX.Gdcame.Utils.Shared
{
    class RendezvousHash
    {
        private IHashFunction hasher;
        private HashSet<Node> nodes;

        public RendezvousHash(IHashFunction hasher, HashSet<Node> init)
        {
            this.hasher = hasher;
            this.nodes = init;
        }

        private long ComputeScore(Node node, String userKey)
        {
            long hash = hasher.GetHash(node.NodeId + "." + userKey);
            return hash;
        }

        /**
	     * Removes a node from the nodes list.
	     * 
	     * @return true if the node was in the list ?
	     */
        public bool Remove(Node node)
        {
            return nodes.Remove(node);
        }

        /**
         * Add a new node to the nodes list
         * 
         * @return true if node did not previously exist in the list ?
         */
        public bool Add(Node node)
        {
            return nodes.Add(node);
        }

        /**
	    * Return a node for a given key
	    */
        public Node DetermineResponsibleNode(String userKey)
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
    }
}
