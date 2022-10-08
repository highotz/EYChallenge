using System.Linq.Expressions;

namespace EYChallenge.Utilities.SystemObjects.Entities
{
    public class RelationExpression<TIn, TOut>
    {
        public Expression<Func<TIn, object>> Path { get; set; } = null;
        public string NavigationPath { get; set; } = null;

        public RelationExpression(Expression<Func<TIn, object>> path)
        {
            Path = path;
        }

        public RelationExpression(string navigationPath)
        {
            if (string.IsNullOrEmpty(navigationPath))
                throw new ArgumentNullException();

            NavigationPath = navigationPath;
        }
    }
}
