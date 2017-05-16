using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.Gdcame.Utils.Shared.Rendezvous_Hashing
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
            RendezvousHash.Logger(string.Format("Hash for [{0}:{1}] is [{2}]", node.ServerAddress, node.ServerNumber, hash));
            return hash;
        }

        /**
	     * Removes a node from the nodes list.
	     * 
	     * @return true if the node was in the list ?
	     */
        public bool Remove(List<Node> nodes, Node node)
        {
            RendezvousHash.Logger(string.Format("Remove [{0}:{1}] from {2}.", node.ServerAddress, node.ServerNumber, nodes));
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
        public void Add(List<Node> nodes, Node node)
        {
            nodes.Add(node);
        }
        public void Add(List<string> nodes, Node node)
        {
            RendezvousHash.Logger(string.Format("Add [{0}:{1}] to {2}.", node.ServerAddress, node.ServerNumber, nodes));
            Add(GetNodesList(nodes), node);
        }

        /**
	    * Return a node for a given key
	    */
        public Node DetermineResponsibleNode(String userKey, List<Node> nodes)
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
            RendezvousHash.Logger(string.Format("Responsable node for userKey = {0} is [{1}:{2}].",
                userKey, max.ServerAddress, max.ServerNumber));
            return max;
        }

        public Node DetermineResponsibleNode(String userKey, List<string> nodes)
        {
            return DetermineResponsibleNode(userKey, GetNodesList(nodes));
        }

        private List<Node> GetNodesList(List<string> nodes)
        {
            List<Node> nodesList = new List<Node>();
            for (int i = 0; i < nodes.Count; i++)
            {
                nodesList.Add(new Node(nodes[i], i));
            }
            return nodesList;
        }

        //todo delete
        public static void Logger(String lines)
        {

            // Write the string to a file.append mode is enabled so that the log
            // lines get appended to  test.txt than wiping content and writing the log

//            System.IO.StreamWriter file = new System.IO.StreamWriter("F:\\projects\\RG-Architects\\RG-Architects - Copy\\bin\\logs\\log.txt", true);
//            file.WriteLine(lines);
//
//            file.Close();

        }
    }
}
