namespace Annotation
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class AnnotationAttribute : Attribute
    {
        private readonly string usage;
        private readonly string cluster;

        AnnotationAttribute(string usage, string cluster)
        {
            this.usage = usage;
            this.cluster = cluster;
        }
    }
}
