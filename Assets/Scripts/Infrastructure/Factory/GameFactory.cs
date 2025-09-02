namespace Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private static GameFactory _instance;
        public static GameFactory Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameFactory();
                }

                return _instance;
            }
        }
    }
}