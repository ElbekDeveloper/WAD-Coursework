namespace Api.Contracts.V1
{
    public static class ApiRoutes
    {
        private const string _root = "api";
        private const string _version = "v1";
        private const string _base = _root + "/" + _version;

        public static class Posts
        {
            public const string GetAll =_base + "/" + "posts";

        }
    }
}
