namespace EntityFX.Gdcame.Utils.Common.Hashing
{
    class Node
    {
        public Node()
        {
        }

        public Node(string serverAddress, int serverNumber)
        {
            ServerAddress = serverAddress;
            ServerNumber = serverNumber;
        }

        /**
	     * Server address written in servers.json. Usually IP. Should be put in "http://{0}:{1}/" instead of {0}.
         * See <link>EntityFX.Gdcame.Presentation.WebApiConsoleClient.ApiHelper.GetApiServerUri</link>
	     */

        public string ServerAddress { get; set; }

        public int ServerNumber { get; set; }
    }
}
