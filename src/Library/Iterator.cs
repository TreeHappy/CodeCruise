using Library.Structure;

namespace Library
{
    public abstract class Iterator
    {
        protected abstract void OnProject(Project project);

        private void Iterate(Project project)
        {
            OnProject(project);

            foreach (var @namespace in project.Assembly.Namespaces.Values)
                Iterate(@namespace);
        }

        protected abstract void OnNamespace(Namespace @namespace);

        private void Iterate(Namespace @namespace)
        {
            OnNamespace(@namespace);

            foreach(var type in @namespace.Types.Values)
                Iterate(type);

            foreach(var @nestedNamespace in @namespace.Namespaces.Values)
                Iterate(@nestedNamespace);
        }

        protected abstract void OnType(Structure.Type type);

        private void Iterate(Structure.Type type)
        {
            OnType(type);
        }
    }
}
