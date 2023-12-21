namespace DDD.WebApi.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class UnitOfWorkAttribute : Attribute
    {
        public Type[] DbContextTypes { get; set; }

        public UnitOfWorkAttribute(params Type[] dbContextTypes) 
        {
            DbContextTypes = dbContextTypes;
        }


    }
}
