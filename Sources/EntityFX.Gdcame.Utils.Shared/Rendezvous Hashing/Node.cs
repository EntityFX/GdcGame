using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.Gdcame.Utils.Shared.Rendezvous_Hashing
{
    class Node
    {
        public Node()
        {
        }

        public Node(string serverAddress, int serverNumber)
        {
            _serverAddress = serverAddress;
            _serverNumber = serverNumber;
        }

        /**
	     * Server address written in servers.json. Usually IP. Should be put in "http://{0}:{1}/" instead of {0}.
         * See <link>EntityFX.Gdcame.Presentation.WebApiConsoleClient.ApiHelper.GetApiServerUri</link>
	     */
        private string _serverAddress;

        public string ServerAddress
        {
            get { return _serverAddress; }
            set { _serverAddress = value; }
        }

        private int _serverNumber;

        public int ServerNumber
        {
            get { return _serverNumber; }
            set { _serverNumber = value; }
        }
    }
}
